using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Rewired;
using Sandbox.RPG;

public class TurnBasedCombatManager : MonoBehaviour
{
    [Header("Parameters")]
    public bool runCombatLoopImmediately;

    [Header("References")]
    public List<CombatBehaviour> entities;
    public DialogueBehaviour dialogueBox;

    public UnityEvent onTurnEnd;

    private HashSet<CombatBehaviour> readyEntities;
    private IEnumerator combatLoop;

    private void Awake()
    {
        readyEntities = new HashSet<CombatBehaviour>();
        if (dialogueBox != null)
            foreach (CombatBehaviour entity in entities)
                entity.dialogueBox = dialogueBox;
    }

    private void Start()
    {
        combatLoop = Coroutine_CombatLoop();
        StartCoroutine(combatLoop);
    }

    public void ExecuteTurn()
    {
        StartCoroutine(Coroutine_ExecuteTurn());
    }

    public IEnumerator Coroutine_ExecuteTurn()
    {
        foreach (CombatBehaviour entity in entities)
            yield return StartCoroutine(entity.Coroutine_ExecuteMove());

        readyEntities.Clear();
        onTurnEnd.Invoke();
    }

    public IEnumerator Coroutine_CombatLoop()
    {
        while (true)
        {
            if (readyEntities.Count < entities.Count)
            {
                yield return null;
                continue;
            }
            yield return StartCoroutine(Coroutine_ExecuteTurn());
        }
    }

    public void MarkAsReady(CombatBehaviour entity)
    {
        if (entities.Contains(entity))
            readyEntities.Add(entity);
    }
}