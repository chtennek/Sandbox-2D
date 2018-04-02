using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehaviour : MonoBehaviour
{
    [Header("Animation")]
    public string damageTrigger = "hitstun";
    public string destroyTrigger = "destroy";

    [Header("Parameters")]
    public bool invokeDeathAfterLifetime = true;
    public float lifetime = Mathf.Infinity;
    public StatusBar lifebar;
    public bool destroyOnDeath = true;

    public UnityEvent onDamage;
    public UnityEvent onDeath;

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
            if (invokeDeathAfterLifetime)
                OnDeath();
            else
                Destroy();
        }
    }

    public void Damage() { Damage(1); }
    public void Damage(int damage)
    {
        if (damage <= 0)
            return;

        if (lifebar == null)
        {
            OnDeath();
            return;
        }

        lifebar.currentValue -= damage;
        if (lifebar.currentValue > 0)
            OnDamage();
        else
            OnDeath();
    }

    public void OnDamage()
    {
        if (anim != null && damageTrigger != "")
            anim.SetTrigger(damageTrigger);

        onDamage.Invoke();
    }

    public void OnDeath()
    {
        if (anim != null && destroyTrigger != "")
            anim.SetTrigger(destroyTrigger);

        if (destroyOnDeath == true)
            Destroy();

        onDeath.Invoke();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
