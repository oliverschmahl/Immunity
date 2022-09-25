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
            if (GameManager.Instance.IsPaused) return;
            if (killsLeft <= 0) return;
            if (_bacteria == null || _bacteria.Length <= 0) return;
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
                if (distanceToTarget < 30f)
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
