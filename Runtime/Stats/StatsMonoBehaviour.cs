using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class StatsMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected Stats stats;

        protected SerializedDictionary<Stat, float> instanceStats = new SerializedDictionary<Stat, float>();

        protected virtual void Awake()
        {
            stats = Instantiate(stats);

            instanceStats = new SerializedDictionary<Stat, float>(stats.instanceStats);
            stats.OnApplyModifier += OnStatsModified;
        }

        protected virtual void OnDestroy()
        {
            stats.OnApplyModifier -= OnStatsModified;
            stats.Reset();
        }

        public void AddModfier(Modifier modifier)
        {
            if (modifier == null)
            {
                Debug.LogWarning("Attempted to add a null modifier.");
                return;
            }
            stats.UnlockUpgrade(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            if (modifier == null)
            {
                Debug.LogWarning("Attempted to remove a null modifier.");
                return;
            }

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

        protected float GetInstanceStat(Stat stat)
        {
            if (instanceStats.TryGetValue(stat, out float value))
            {
                return value;
            }
            Debug.LogWarning($"Stat {stat} not found in instance stats dictionary.");
            return 0f; // ou algum valor padrão
        }
    }
}
