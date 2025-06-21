using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class StatsMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected Stats stats;

        protected virtual void Awake()
        {
            stats = Instantiate(stats);
        }

        protected virtual void OnDestroy()
        {
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

        protected float GetInstanceStat(Stat stat)
        {
            return stats.GetStat(stat);
        }
    }
}
