using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class ComplementProtein : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationSpeed = 0.5f;
        [SerializeField, Range(1, 30)] private int damage = 1;
        [SerializeField, Range(1f,5f)] private float attackRange = 1f;

        private List<GameObject> _bacteriaSmall;
        private List<GameObject> _bacteriaLarge;
        private GameObject[] _bacteria;
        public GameObject _target;
        
        public Vector2 spawnTarget;
        private bool _reachedSpawnTarget = false;

        private void Start()
        {
            
        }

        void Update()
        {
            if (GameManager.instance.IsPaused) return;

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
            
            FindTarget();
            if (_target)
            {
                var complementProteinTransform = transform;
                var complementProteinPosition = complementProteinTransform.position;
                var complementProteinUp = complementProteinTransform.up;
                
                var targetPosition = _target.transform.position;
                
                // Move forward
                complementProteinPosition += complementProteinUp * (Time.deltaTime * movementSpeed);
                complementProteinTransform.position = complementProteinPosition;
                // Rotate towards target
                Vector3 targetDirection = (targetPosition - complementProteinPosition).normalized;
                Vector2 newDirection = Vector2.Lerp(complementProteinUp, targetDirection, rotationSpeed * Time.deltaTime);
                transform.up = newDirection;

                float distanceToTarget = Vector2.Distance(complementProteinPosition, targetPosition);
                if (distanceToTarget < attackRange)
                {
                    _target.GetComponent<Health>().TakeDamage(damage);
                }
            }
        }
        
        void FindTarget()
        {
            _bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
            _bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

            _bacteria = _bacteriaLarge.Concat(_bacteriaSmall).ToArray();
            
            float distance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in _bacteria)
            {
                if(!enemy.activeInHierarchy) continue;
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