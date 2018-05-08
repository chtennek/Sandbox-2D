using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    namespace RPG
    {
        public enum StatMode
        {
            Base,
            Additive,
            Multiply,
            AdditiveProportion,
        }

        [System.Serializable]
        public class Stat
        {
            public StatMode mode = StatMode.Additive;
            public StatType type;

            public float baseValue;
            public float value;
        }

        [CreateAssetMenu(fileName = "StatType", menuName = "RPG/Stat Type", order = 0)]
        public class StatType : ScriptableObject
        {
            public string statName = "HP"; // Attack, Speed, Hitstun, Hitlag
            public string description;
        }

        //public class StatTypeGroup : ScriptableObject
        //{
        //    public StatType[] statTypes;
        //}
    }
}