using UnityEngine;
using ProjectCeros;

namespace ProjectCeros
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyRuntimeSet _runtimeSet;

        private void OnEnable()
        {
            _runtimeSet.Add(this);
        }

        private void OnDisable()
        {
            _runtimeSet.Remove(this);
        }
    }
}