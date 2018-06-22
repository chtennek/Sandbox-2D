using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool allOrNothing = false;

    protected HashSet<GridEntity> entities = new HashSet<GridEntity>();
    protected SortedDictionary<int, HashSet<GridEntity>> entityLookup = new SortedDictionary<int, HashSet<GridEntity>>();

    private Dictionary<GridEntity, Vector3Int> moveLookup = new Dictionary<GridEntity, Vector3Int>();
    private Dictionary<GridEntity, Vector3Int> undoLookup = new Dictionary<GridEntity, Vector3Int>();

    private bool isCurrentGroupReady;
    private HashSet<GridEntity> m_currentOrderGroup = new HashSet<GridEntity>();
    private HashSet<GridEntity> CurrentOrderGroup
    {
        get { return m_currentOrderGroup; }
        set
        {
            m_currentOrderGroup = value;
            isCurrentGroupReady = m_currentOrderGroup.IsSubsetOf(moveLookup.Keys);
        }
    }

    public static GridManager main;

    private void Awake()
    {
        if (main != null)
            Warnings.DuplicateSingleton(this);
        else
            main = this;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            foreach (int order in entityLookup.Keys)
            {
                CurrentOrderGroup = entityLookup[order];

                bool success = false;

                while (success == false)
                {
                    yield return new WaitUntil(() => isCurrentGroupReady);
                    isCurrentGroupReady = false;
                    success = ProcessCurrentGroup();
                }
            }
            yield return null;
        }
    }

    private bool ProcessCurrentGroup()
    {
        // Move all entities tenatively to new locations
        undoLookup.Clear();
        foreach (GridEntity entity in CurrentOrderGroup)
        {
            undoLookup[entity] = entity.GridPosition;

            entity.Warp(moveLookup[entity], force: true, moveVisual: false);
        }

        // Test for validity
        bool allValid = true;
        SortedSet<GridEntity> invalidEntities = new SortedSet<GridEntity>(new PriorityComparer());
        do
        {
            // Find invalid moves
            invalidEntities.Clear();
            foreach (GridEntity entity in CurrentOrderGroup)
                if (entity.IsCurrentPositionLegal() == false)
                {
                    invalidEntities.Add(entity);
                    allValid = false;
                }

            if (allOrNothing && allValid == false)
            {
                foreach (GridEntity entity in invalidEntities)
                    moveLookup.Remove(entity);
                break;
            }

            // Reset invalid positions and remove move submission for invalids
            foreach (GridEntity entity in invalidEntities)
            {
                Debug.Log(entity);
                entity.WarpTo(undoLookup[entity], force: true, moveVisual: false);
                moveLookup.Remove(entity);
            }
        }
        while (invalidEntities.Count > 0); // Recheck for invalids after positions reset

        // Reset all positions
        foreach (GridEntity entity in CurrentOrderGroup)
            entity.WarpTo(undoLookup[entity], force: true, moveVisual: false);

        // Move properly for all remaining valid moves
        foreach (GridEntity entity in CurrentOrderGroup)
            if (moveLookup.ContainsKey(entity))
            {
                entity.Move(moveLookup[entity], force: true);
                moveLookup.Remove(entity);
            }

        return allValid || allOrNothing == false;
    }

    public void SubmitMove(GridEntity entity, Vector3Int move)
    {
        moveLookup[entity] = move;
        isCurrentGroupReady = CurrentOrderGroup.IsSubsetOf(moveLookup.Keys);
    }

    public void Register(GridEntity entity)
    {
        entities.Add(entity);

        if (entityLookup.ContainsKey(entity.order) == false)
            entityLookup.Add(entity.order, new HashSet<GridEntity>());

        //if (entityLookup[entity.order].ContainsKey(entity.priority) == false)
        //    entityLookup[entity.order].Add(entity.priority, new HashSet<GridEntity>());

        entityLookup[entity.order].Add(entity);
    }

    public void Deregister(GridEntity entity)
    {
        entities.Remove(entity);

        entityLookup[entity.order].Remove(entity);
    }
}
