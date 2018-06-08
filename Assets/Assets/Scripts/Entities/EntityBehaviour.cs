﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sandbox.RPG;

public class EntityBehaviour : MonoBehaviour
{
    [Header("Data")]
    public EntityType type;
    public GameValue[] values;

    [Header("Behaviour")]
    public float lifetime = Mathf.Infinity;
    public bool destroyOnDeath = true;

    [Header("Effects")]
    public Stat[] statChangesOnCollision;
    public EffectType effectOnCollision;

    public UnityEvent onCollision;
    public UnityEvent onDeath;

    private Dictionary<EffectType, float> buffStack; // Value = start time, [TODO] track stacks

    private float spawnTimestamp;
    private Animator animator;

    private GameValue GetGameValue(Stat stat) { return GetGameValue(stat.type); }
    private GameValue GetGameValue(StatType type)
    {
        foreach (GameValue gv in values)
            if (gv.stat.type == type)
                return gv;

        return null;
    }

    private void SetGameValue(Stat stat)
    {
        GameValue gv = GetGameValue(stat.type);
        if (gv == null)
            return;

        gv.maxValue = stat.baseValue;
        gv.Value = stat.value;
    }

    private void Awake()
    {
        spawnTimestamp = Time.time;
        animator = GetComponent<Animator>();

        if (type != null)
            foreach (Stat stat in type.baseStats)
                SetGameValue(stat);
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

        if (effectOnCollision != null)
            entity.ApplyEffect(effectOnCollision);
    }

    public void ApplyStatChange(Stat statChange)
    {
        GameValue gv = GetGameValue(statChange);
        if (gv == null)
            return;

        switch (statChange.mode)
        {
            case StatMode.Additive:
                gv.maxValue += statChange.baseValue;
                gv.Value += statChange.value;
                break;
        }
    }

    public void ApplyBuff(EffectType buff)
    {
        buffStack.Add(buff, Time.time);
        IEnumerator coroutine = Coroutine_ApplyBuff(buff);
        StartCoroutine(coroutine);
    }

    public IEnumerator Coroutine_ApplyBuff(EffectType buff)
    {
        yield return null;
    }

    public void ApplyEffect(EffectType effect)
    {
        foreach (Stat statChange in effect.statChanges)
        {
            ApplyStatChange(statChange);
        }
        foreach (EffectType buff in effect.buffs)
        {
            ApplyBuff(buff);
        }
    }

    public void OnCollision()
    {
        onCollision.Invoke();
    }

    public void OnDeath()
    {
        onDeath.Invoke();
        if (destroyOnDeath == true)
            ObjectPooler.Deallocate(transform);
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}