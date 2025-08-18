using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    [CreateAssetMenu(menuName ="Hello World/Utils/New Stats")]
    public class Stats : ScriptableObject
    {
        public SerializedDictionary<Stat, float> stats = new SerializedDictionary<Stat, float>();
        private List<Modifier> modifiers = new List<Modifier>();

        public event Action<Stats, Modifier> OnApplyModifier;

        public float GetStat(Stat stat)
        {
            if (stats.TryGetValue(stat, out float value))
                return GetUpgradedValue(stat, value);

            Debug.LogWarning($"Stat {stat} not found in stats dictionary.");
            return 0f; // ou algum valor padrão
        }

        public float ChangeStat(Stat stat, float delta)
        {
            if (stats.TryGetValue(stat, out float currentValue))
            {
                stats[stat] += delta;
                return stats[stat];
            }


            return -1f;
        }

        public void UnlockUpgrade(Modifier modifier)
        {
            if (!modifiers.Contains(modifier))
            {
                modifiers.Add(modifier);
                OnApplyModifier?.Invoke(this, modifier);
            }
        }

        public void RemoveUpgrade(Modifier modifier)
        {
            if (!modifiers.Contains(modifier))
            {
                return;
            }

            modifiers.Remove(modifier);
        }

        private float GetUpgradedValue(Stat stat, float baseValue)
        {
            foreach (var modifier in modifiers)
            {
                if (!modifier.statsToModify.TryGetValue(stat, out float modifierValue))
                    continue;

                if (modifier.isPercentageModifier)
                    baseValue *= (modifierValue / 100f) + 1f;
                else
                    baseValue += modifierValue;
            }
            return baseValue;
        }

        public void Reset()
        {
            modifiers.Clear();
        }

        public bool HasModifier(Modifier modifier)
        {
            return modifiers.Contains(modifier);
        }
    }

    public enum Stat
    {
        Health,
        Speed,
        JumpHeight,
        Damage,
    }

}
