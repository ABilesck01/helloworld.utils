using AYellowpaper.SerializedCollections;
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
        public SerializedDictionary<Stat, float> statsToModify = new SerializedDictionary<Stat, float>();
        public bool isPercentageModifier = false;


    }
}
