using System.Collections.Generic;
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
        
        private GameObject[] _bacteria;
        private GameObject _target;
        
        public Vector2 spawnTarget;
        private bool reachedSpawnTarget = false;

        private void Awake()
        {
            BacteriaManager.OnBacteriaListChanged += BacteriaListChanged;
        }

        private void OnDestroy()
        {
            BacteriaManager.OnBacteriaListChanged -= BacteriaListChanged;
        }

        private void BacteriaListChanged(List<GameObject> bacteriaList)
        {
            _bacteria = bacteriaList.ToArray();
        }


        void Start()
        {
            FindTarget();
        }

        void Update()
        {
            if (GameManager.instance.IsPaused) return;
            if (killsLeft <= 0) return;
            if (!reachedSpawnTarget)
            {
                float step = movementSpeed * Time.deltaTime;
                var _transform = transform;
                var _pos = transform.position;
                Vector2 _posV2 = 
                    _pos = Vector2.MoveTowards(_pos, spawnTarget, step);
                
                _transform.position = _pos;
                transform.up = spawnTarget - _posV2;
                if (Vector2.Distance(transform.position, spawnTarget) < 5f) reachedSpawnTarget = true;
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
                    BacteriaManager.Instance.RemoveBacteria(_target);
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
