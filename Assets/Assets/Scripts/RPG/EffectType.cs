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
            Self = (1 << 0),
            Ally = (1 << 1),
            Enemy = (1 << 2),
            Location = (1 << 3),
        }

        [CreateAssetMenu(fileName = "EffectType", menuName = "RPG/Effect Type", order = 0)]
        public class EffectType : ScriptableObject
        {
            public Sprite image;
            public string animationTrigger;
            public string displayName = "Melee Attack";
            [TextArea(1, 5)]
            public string description;

            [Header("Behaviour")]
            [EnumFlags]
            public EffectTargetType targetType;

            public Stat[] statChanges; // Total effect to be divided over all ticks
            public EffectType[] buffs;
            public bool channeled;
            public bool chargeCancelable;
            public bool castCancelable;

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