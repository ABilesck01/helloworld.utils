using System;
using UnityEngine;

namespace HelloWorld.Utils
{
    public static class JsonUtilityWrapper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] FromJson<T>(string json)
        {
            // Adiciona um objeto fictício em volta do array para funcionar com JsonUtility
            string wrappedJson = $"{{\"array\":{json}}}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
            return wrapper.array;
        }
    }
}
