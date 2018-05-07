using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sandbox.RPG;

public class EntityBehaviour : MonoBehaviour
{
    [Header("Data")]
    public EntityType type;
    public GameValue[] stats;

    [Header("Behaviour")]
    public float lifetime = Mathf.Infinity;
    public bool destroyOnDeath = true;

    [Header("Effects")]
    public EffectType effectOnCollision; // Used by projectile entities

    public UnityEvent onDamage;
    public UnityEvent onDeath;

    private float spawnTimestamp;
    private Animator anim;

    private void Awake()
    {
        spawnTimestamp = Time.time;
        anim = GetComponent<Animator>();

        if (type != null)
        {
            foreach (Stat stat in type.baseStats)
            {
                foreach (GameValue gv in stats)
                {
                    if (gv.stat.type == stat.type)
                    {

                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Time.time - spawnTimestamp > lifetime)
        {
            OnDeath();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        EntityBehaviour entity = other.GetComponent<EntityBehaviour>();
        if (entity == null)
            return;
    }

    public void ApplyEffect(EffectType effect)
    {

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

    public void DebugLog()
    {
        Debug.Log(gameObject);
    }
}