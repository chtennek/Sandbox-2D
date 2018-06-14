using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public class CombatBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private bool runCombatLoop;

    [SerializeField]
    private bool resetTargetOnExecute;

    [SerializeField]
    private bool resetMoveOnExecute;

    [Header("Combat Logic")]
    [SerializeField]
    private EntityType self;

    [SerializeField]
    private EntityBehaviour target;

    [SerializeField]
    private EffectType ability;

    [Header("References")]
    public DialogueBehaviour dialogueBox; // Stream combat dialogue

    [SerializeField]
    private WaypointControl movement; // Leave null to ignore range limitations

    [SerializeField]
    private AbilityMultiDisplay display;

    [SerializeField]
    private AnimatorMessenger animator;

    private IEnumerator combatLoop;

    public void SetTarget(Transform target)
    {
        SetTarget(target?.GetComponent<EntityBehaviour>());
    }

    public void SetTarget(EntityBehaviour entity)
    {
        target = entity;
    }

    public void SetCurrentMove(EffectType move)
    {
        ability = move;
    }

    private void Reset()
    {
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

        if (self != null && display != null)
            display.Display(self.abilities);
    }

    // [TODO] Make a templating system for dialogue
    public void SendDialogue()
    {
        if (dialogueBox == null)
            return;

        string text = string.Format("{0} used {1} on {2}!", name, ability.name, target.name);
        Line line = new Line(text);
        dialogueBox.AddLine(line);
    }

    public IEnumerator Coroutine_ExecuteMove()
    {
        if (animator != null)
            animator.SetTrigger(ability.chargeTrigger);
        if (dialogueBox != null)
            SendDialogue();

        yield return new WaitForSeconds(ability.chargeTime);
        target.ApplyEffect(ability);
        yield return new WaitForSeconds(ability.cooldown);
    }

    protected IEnumerator Coroutine_CombatLoop()
    {
        while (true)
        {
            // Wait for a target or message from manager
            if (enabled == false || target == null || ability == null)
            {
                yield return null;
                continue;
            }

            // Move if we need to
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (movement != null)
            {
                if (distance > ability.maxRange)
                {
                    movement.waypointRadius = ability.maxRange;
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
                ability = null;
        }
    }
}