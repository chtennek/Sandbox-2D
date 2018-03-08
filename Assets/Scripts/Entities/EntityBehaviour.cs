using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    [Header("Animation")]
    public string damageTrigger = "hitstun";
    public string destroyTrigger = "destroy";

    [Header("Parameters")]
    public float lifetime = Mathf.Infinity;
    public bool destroyAtZeroHP = true;
    public float currentHP = 1;

    private float spawnTimestamp;

    private Animator anim;

    private void Awake()
    {
        spawnTimestamp = Time.time;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time - spawnTimestamp > lifetime)
        {
            OnDeath();
        }
    }

    public void Damage() { Damage(1); }
    public void Damage(float damage)
    {
        currentHP -= damage;
        if (currentHP > 0)
            OnDamage();
        else
            OnDeath();
    }

    public void OnDamage()
    {
        if (anim != null && damageTrigger != "")
            anim.SetTrigger(damageTrigger);
    }

    public void OnDeath()
    {
        if (anim != null && destroyTrigger != "")
            anim.SetTrigger(destroyTrigger);

        if (destroyAtZeroHP == true)
            Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
