using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    namespace RPG
    {
        [System.Flags]
        public enum EffectTargetType
        {
            Self,
            Ally,
            Enemy,
            Location,
        }

        public enum StatEffectType
        {
            Base,
            Additive,
            Multiply,
        }

        [CreateAssetMenu(fileName = "EffectType", menuName = "RPG/Effect Type", order = 0)]
        public class EffectType : ScriptableObject
        {
            public Sprite image;
            public string animationTrigger;
            public string effectName = "Melee Attack";
            [TextArea(1, 5)]
            public string description;

            [Header("Behaviour")]
            public StatEffectType effectType;
            public Stat[] baseEffects; // Total effect to be divided over all ticks
            [EnumFlags]
            public EffectTargetType targetType;

            public EffectType[] buffs;
            public bool channeled;

            [Header("Stats")]
            public float falloff = 0; // D.va bomb, Mccree, Golden sun, etc.
            public float baseAccuracy = 1;
            public float minRange = 1;
            public float maxRange = 1;
            //public float critChance = 0.1f;

            [Header("Time")]
            public float chargeTime = 0; // Time before cast
            public float overTime = 0.1f; // Seconds between first tick and last tick
            public int ticks = 2; // Ticks over the time range overTime
            public float cooldown = 1; // Time between current cast begin and next charge begin

        }
    }
}