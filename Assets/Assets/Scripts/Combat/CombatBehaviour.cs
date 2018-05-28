using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;
using Sandbox.RPG;

public class CombatBehaviour : MonoBehaviour
{
    [Header("Input")]
    public int mouseButton = 1;
    public LayerMask layerMask = ~0;
    public List<string> tagMask;
    public float mouseRaycast = 1000;

    [Header("Properties")]
    public bool runCombatLoop;
    public bool resetTargetOnExecute;
    public bool resetMoveOnExecute;

    public EntityType self;
    public EntityBehaviour target;
    public EffectType currentMove;

    [Header("References")]
    public DialogueBehaviour dialogueBox; // Stream combat dialogue
    public WaypointControl movement; // Leave null to ignore range limitations
    public AnimatorMessenger animator;

    private IEnumerator combatLoop;

    public void SetTarget(EntityBehaviour entity)
    {
        target = entity;
    }

    public void SetCurrentMove(EffectType move)
    {
        currentMove = move;
    }

    private void Reset()
    {
        EntityBehaviour entity = GetComponent<EntityBehaviour>();
        if (entity != null)
            self = entity.type;

        movement = GetComponent<WaypointControl>();
        animator = GetComponent<AnimatorMessenger>();
    }

    protected void Start()
    {
        if (runCombatLoop)
        {
            combatLoop = Coroutine_CombatLoop();
            StartCoroutine(combatLoop);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            target = GetMouseTarget();
        }
    }

    // [TODO] Make a templating system for dialogue
    public void SendDialogue()
    {
        if (dialogueBox == null)
            return;

        string text = string.Format("{0} used {1} on {2}!", name, currentMove.name, target.name);
        Line line = new Line(text);
        dialogueBox.AddLine(line);
    }

    public IEnumerator Coroutine_ExecuteMove()
    {
        if (animator != null)
            animator.SetTrigger(currentMove.animationTrigger);
        if (dialogueBox != null)
            SendDialogue();

        yield return new WaitForSeconds(currentMove.chargeTime);
        target.ApplyEffect(currentMove);
        yield return new WaitForSeconds(currentMove.cooldown);
    }

    protected IEnumerator Coroutine_CombatLoop()
    {
        while (true)
        {
            // Wait for a target or message from manager
            if (enabled == false || target == null || currentMove == null)
            {
                yield return null;
                continue;
            }

            // Move if we need to
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (movement != null)
            {
                if (distance > currentMove.maxRange)
                {
                    movement.waypointRadius = currentMove.maxRange;
                    movement.ClearWaypoints();
                    movement.AddWaypoint(target.transform);
                    yield return null; // [TODO] performance
                    continue;
                }
                else
                    movement.ClearWaypoints();
            }

            yield return StartCoroutine(Coroutine_ExecuteMove());
            if (resetTargetOnExecute)
                target = null;
            if (resetMoveOnExecute)
                currentMove = null;
        }
    }

    private EntityBehaviour GetMouseTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, mouseRaycast, layerMask) == false)
            return null;

        if (tagMask.Count > 0 && tagMask.Contains(hit.transform.tag) == false)
            return null;

        return hit.transform.GetComponent<EntityBehaviour>();
    }
}