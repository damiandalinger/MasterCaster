/// <summary>
/// Loops the clouds in the background.
/// </summary>

/// <remarks>
/// 31/01/2026 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class CloudLoop : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _clouds;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Transform _startingPosition;
        private float _spriteLength = 19.2f;

        private void Start()
        {
            StartPositionClouds();
        }

        private void Update()
        {
            MoveClouds();
        }

        private void StartPositionClouds()
        {
            for (int i = 0; i < _clouds.Count; i++)
            {
                Vector3 spawnPos = new Vector3(_startingPosition.position.x + (i * _spriteLength), 0, 0);
                _clouds[i].transform.position = spawnPos;
            }
        }

        private void MoveClouds()
        {
            Vector3 dir = Vector3.right * (Time.deltaTime * _speed);

            for (int i = 0; i < _clouds.Count; i++)
            {
                _clouds[i].transform.Translate(dir);
            }

            if (_clouds[_clouds.Count - 1].transform.position.x > _spriteLength)
            {
                RecycleCloud();
            }
        }

        private void RecycleCloud()
        {
            GameObject cloud = _clouds[_clouds.Count - 1];
            float newX = cloud.transform.position.x - (_clouds.Count * _spriteLength);

            _clouds.RemoveAt(_clouds.Count - 1);
            cloud.transform.position = new Vector3(newX, 0f, 0f);
            _clouds.Insert(0, cloud);
        }
    }
}