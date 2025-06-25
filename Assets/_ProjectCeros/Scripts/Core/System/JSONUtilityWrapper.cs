/// <summary>
/// Provides utility methods for working with JSON arrays in Unity.
/// Unity's JsonUtility does not natively support deserializing top-level arrays.
/// This wrapper fixes that limitation by wrapping arrays into an object temporarily.
/// </summary>

/// <remarks>
/// 29/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public static class JsonUtilityWrapper
    {
        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }

        public static List<T> FromJsonArray<T>(string json)
        {
            string newJson = "{\"Items\":" + json + "}";
            return JsonUtility.FromJson<Wrapper<T>>(newJson).Items;
        }
    }
}