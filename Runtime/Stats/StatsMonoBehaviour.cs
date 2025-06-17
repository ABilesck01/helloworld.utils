using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helloworld.utils
{
    public class StatsMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected Stats stats;

        private Dictionary<Stat, float> instanceStats = new Dictionary<Stat, float>();

        protected virtual void Awake()
        {
            instanceStats = new Dictionary<Stat, float>(stats.instanceStats);
            stats.OnApplyModifier += OnStatsModified;
        }

        protected virtual void OnDestroy()
        {
            stats.OnApplyModifier -= OnStatsModified;
        }

        private void OnStatsModified(Stats source, Modifier modifier)
        {
            foreach (var stat in modifier.statsToModify.Keys)
            {
                float newValue = stats.GetStat(stat); // valor com upgrade
                if (instanceStats.ContainsKey(stat))
                {
                    instanceStats[stat] = newValue;
                }
                else
                {
                    instanceStats.Add(stat, newValue);
                }
            }
        }
    }
}
