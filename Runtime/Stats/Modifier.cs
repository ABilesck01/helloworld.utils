using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class Modifier : ScriptableObject
    {
        public Sprite icon;
        public string upgradeName;
        public string description;
        public int cost = 0;
        [Space]
        public List<Stats> unitsToUpgrade = new List<Stats>();
        public Dictionary<Stat, float> statsToModify = new Dictionary<Stat, float>();
        public bool isPercentageModifier = false;

        public void DoUpgrade()
        {
            foreach (var item in unitsToUpgrade)
            {
                item.UnlockUpgrade(this);
            }
        }

    }
}
