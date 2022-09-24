using System;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class Macrophage : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Range(10f, 300f)] private float movementSpeed = 50f;
        [SerializeField, Range(0.1f, 5f)] private float rotationSpeed = 0.5f;

        
        private GameObject[] _enemies;
        private GameObject _target;

        private void Awake()
        {
            BacteriaManager.OnBacteriaChanged += BacteriaChanged;
        }

        private void OnDestroy()
        {
            BacteriaManager.OnBacteriaChanged -= BacteriaChanged;
        }

        private void BacteriaChanged(List<GameObject> bacteriaList)
        {
            _enemies = bacteriaList.ToArray();
        }


        void Start()
        {
            FindTarget();
        }

        void Update()
        {
            if (_target)
            {
                var macrophageTransform = transform;
                var macrophagePosition = macrophageTransform.position;
                var macrophageUp = macrophageTransform.up;
                
                var targetPosition = _target.transform.position;
                
                // Move forward
                macrophagePosition += macrophageUp * (Time.deltaTime * movementSpeed);
                macrophageTransform.position = macrophagePosition;
                // Rotate towards target
                Vector3 targetDirection = (targetPosition - macrophagePosition).normalized;
                Vector2 newDirection = Vector2.Lerp(macrophageUp, targetDirection, rotationSpeed * Time.deltaTime);
                transform.up = newDirection;

                float distanceToTarget = Vector2.Distance(macrophagePosition, targetPosition);
                if (distanceToTarget < 30f)
                {
                    BacteriaManager.Instance.RemoveBacteria(_target);
                    FindTarget();
                }
            }
        }

        void FindTarget()
        {
            float distance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in _enemies)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            if (closestEnemy) _target = closestEnemy;
        }
    }
}
