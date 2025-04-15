/// <summary>
/// Provides a menu item to generate a standardized scene hierarchy.
/// For consistent layout and easier scene navigation.
/// </summary>

/// <remarks>
/// 15/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEditor;

namespace ProjectCeros
{
    public class SceneStructureGenerator : MonoBehaviour
    {
        [MenuItem("Tools/Create Default Scene Structure")]
        static void CreateStructure()
        {
            CreateEmpty("@System");
            CreateEmpty("@Debug");
            CreateEmpty("@Management");
            CreateEmpty("@UI/Layouts");
            CreateEmpty("Cameras");
            CreateEmpty("Lights/Volumes");
            CreateEmpty("Particles");
            CreateEmpty("Sound");
            CreateEmpty("World/Global");
            CreateEmpty("Gameplay/Actors");
            CreateEmpty("Gameplay/Items");
            CreateEmpty("Gameplay/Triggers");
            CreateEmpty("Gameplay/Quests");
            CreateEmpty("_Dynamic");
        }

        static void CreateEmpty(string path)
        {
            string[] levels = path.Split('/');
            Transform parent = null;
            foreach (var level in levels)
            {
                Transform existing = parent == null ?
                    GameObject.Find(level)?.transform :
                    parent.Find(level);
                if (existing != null)
                {
                    parent = existing;
                    continue;
                }

                GameObject go = new GameObject(level);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                if (parent != null) go.transform.parent = parent;
                parent = go.transform;
            }
        }
    }
}
