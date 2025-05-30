/// <summary>
/// Instantiates a specified prefab and ensures it persists across scene loads.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class PrefabInstantiator : MonoBehaviour
    {
        [Tooltip("The prefab to instantiate.")]
        [SerializeField] private GameObject prefab;

        public void InstantiatePrefab()
        {
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            DontDestroyOnLoad(instance);
        }
    }
}
