using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehaviour : MonoBehaviour
{
    [Header("Stats")]
    public float lifetime = Mathf.Infinity;
    public bool destroyOnDeath = true;
    public bool damageCausesDeath = false;
    public StatusBar lifebar;

    [Header("Effects")]
    public int damage = 0;

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
            OnDeath();
        }
    }

    public void Damage() { Damage(1); }
    public void Damage(Transform other)
    {
        EntityBehaviour entity = other.GetComponent<EntityBehaviour>();
        if (entity == null)
            return;

        entity.Damage(damage);
        Damage(entity.damage);
    }
    public void Damage(int damage)
    {
        if (damageCausesDeath) {
            OnDeath();
            return;
        }
        if (lifebar == null)
            return;
        if (damage <= 0)
            return;

        lifebar.currentValue -= damage;
        if (lifebar.currentValue > 0)
            OnDamage();
        else
            OnDeath();
    }

    public void OnDamage()
    {
        onDamage.Invoke();
    }

    public void OnDeath()
    {
        onDeath.Invoke();
        if (destroyOnDeath == true)
            Destroy(gameObject);
    }
}
