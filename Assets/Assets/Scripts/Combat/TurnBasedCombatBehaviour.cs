using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;
using Sandbox.RPG;

public class TurnBasedCombatBehaviour : MonoBehaviour
{
    public EntityBehaviour target;
    public List<string> tagMask;

    public EntityType self;
    public EffectType currentMove;

    public AnimatorMessenger animator;

    private IEnumerator combatLoop;

    private void Reset()
    {
        EntityBehaviour entity = GetComponent<EntityBehaviour>();
        if (entity != null)
            self = entity.type;

        animator = GetComponent<AnimatorMessenger>();
    }

    public void CombatLoop()
    {
        if (target == null || currentMove == null)
            return;

        target.ApplyEffect(currentMove);
        if (animator != null)
            animator.SetTrigger(currentMove.animationTrigger);
    }
}