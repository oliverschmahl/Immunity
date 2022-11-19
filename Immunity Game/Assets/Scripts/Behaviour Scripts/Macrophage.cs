using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class Macrophage : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Range(10f, 300f)] private float movementSpeed = 50f;
        [SerializeField, Range(0.1f, 5f)] private float rotationSpeed = 0.5f;
        [SerializeField] private int killsLeft = 100;
        
        private List<GameObject> _bacteriaSmall;
        private List<GameObject> _bacteriaLarge;
        private GameObject[] _bacteria;
        private GameObject _target;
        
        public Vector2 spawnTarget;
        private bool _reachedSpawnTarget = false;

        private void Start()
        {
            _bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
            _bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

            _bacteria = _bacteriaLarge.Concat(_bacteriaSmall).ToArray();
        }

        void Update()
        {
            if (GameManager.instance.IsPaused) return;
            if (killsLeft <= 0) return;
            if (!_reachedSpawnTarget)
            {
                float step = movementSpeed * Time.deltaTime;
                var _transform = transform;
                var _pos = transform.position;
                Vector2 _posV2 = 
                    _pos = Vector2.MoveTowards(_pos, spawnTarget, step);
                
                _transform.position = _pos;
                transform.up = spawnTarget - _posV2;
                if (Vector2.Distance(transform.position, spawnTarget) < 5f) _reachedSpawnTarget = true;
                return;
            }
            if (_bacteria is not {Length: > 0}) return;
            FindTarget();
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
                if (distanceToTarget < 60f)
                {
                    _target.SetActive(false);
                    killsLeft -= 1;
                    FindTarget();
                }
            }
        }

        void FindTarget()
        {
            float distance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in _bacteria)
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
