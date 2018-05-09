using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;
using Sandbox.RPG;

public class CombatBehaviour : MonoBehaviour
{
    public EntityBehaviour target;
    public List<string> tagMask;

    public EntityType self;
    public EffectType currentMove;

    public AnimatorMessenger animator;
    public InputReceiver input;
    public float mouseRaycast = 1000;

    private IEnumerator combatLoop;
    private Mouse mouse;

    private void Reset()
    {
        EntityBehaviour entity = GetComponent<EntityBehaviour>();
        if (entity != null)
            self = entity.type;

        animator = GetComponent<AnimatorMessenger>();
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        mouse = ReInput.controllers.Mouse;
    }

    private void Start()
    {
        combatLoop = Coroutine_CombatLoop();
        StartCoroutine(combatLoop);
    }

    private void Update()
    {
        if (mouse.GetButtonDown(0))
        {
            target = GetMouseTarget();
        }
    }

    private IEnumerator Coroutine_CombatLoop()
    {
        while (true)
        {
            if (target == null || currentMove == null) {
                yield return null;
                continue;
            }

            Debug.Log("Attack");
            target.ApplyEffect(currentMove);
            if (animator != null)
                animator.SetTrigger(currentMove.animationTrigger);
            yield return new WaitForSeconds(currentMove.cooldown);
        }
    }

    private EntityBehaviour GetMouseTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, mouseRaycast) == false)
            return null;

        if (tagMask.Count > 0 && tagMask.Contains(hit.transform.tag) == false)
            return null;

        return hit.transform.GetComponent<EntityBehaviour>();
    }
}