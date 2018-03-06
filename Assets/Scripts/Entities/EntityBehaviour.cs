using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public string damageTrigger = "hitstun";
    public string destroyTrigger = "death";
    public float currentHP = 1;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Damage() { Damage(1); }
    public void Damage(float damage)
    {
        currentHP -= damage;
        if (currentHP > 0) {
            if (anim != null && damageTrigger != "")
                anim.SetTrigger(damageTrigger);
        }
        else
        {
            if (anim != null && destroyTrigger != "")
                anim.SetTrigger(destroyTrigger);
            else
                Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
