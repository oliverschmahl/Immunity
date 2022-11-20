using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour_Scripts
{
    public class Macrophage : MonoBehaviour
    {
        public enum State
        {
            Normal,
            Disabled,
            Angry
        }

        // Serialized fields
        [SerializeField] private float movementSpeed_Normal = 1.2f;
        [SerializeField] private float rotationSpeed_Normal = 0.7f;
        [SerializeField] private float movementSpeed_Angry = 1.5f;
        [SerializeField] private float rotationSpeed_Angry = 1.2f;

        [SerializeField] private State state = State.Normal;
        
        [SerializeField] private int killsLeft = 100;

        private float _movementSpeed;
        private float _rotationSpeed;

        public GameObject target;
        
        public Vector2 spawnTarget;
        private bool _reachedSpawnTarget = false;

        private SpriteManager _spriteManager; 

        private void Start()
        {
            if (state == State.Normal) _movementSpeed = movementSpeed_Normal; _rotationSpeed = rotationSpeed_Normal;
            if (state == State.Angry) _movementSpeed = movementSpeed_Angry; _rotationSpeed = movementSpeed_Angry;
            _spriteManager = GetComponentInChildren<SpriteManager>();
        }

        void Update()
        {
            if (GameManager.instance.IsPaused) return;

            state = killsLeft switch
            {
                > 100 => State.Angry,
                <= 100 and > 0 => State.Normal,
                _ => State.Disabled
            };

            _spriteManager.changeSprite((int) state);
            switch (state)
            {
                case State.Normal:
                    _movementSpeed = movementSpeed_Normal;
                    _rotationSpeed = rotationSpeed_Normal;
                    break;
                case State.Angry:
                    _movementSpeed = movementSpeed_Angry;
                    _rotationSpeed = rotationSpeed_Angry;
                    break;
                case State.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (state == State.Disabled) return;


            if (!_reachedSpawnTarget)
            {
                float step = _movementSpeed * Time.deltaTime;
                var macrophageTransform = transform;
                var macrophagePosition = macrophageTransform.position;
                Vector2 newPosition = macrophagePosition = Vector2.MoveTowards(macrophagePosition, spawnTarget, step);
                
                transform.position = macrophagePosition;
                transform.up = spawnTarget - newPosition;
                if (Vector2.Distance(transform.position, spawnTarget) < 3f) _reachedSpawnTarget = true;
                return;
            }
            
            FindTarget();
            if (target)
            {
                var macrophageTransform = transform;
                var macrophagePosition = macrophageTransform.position;
                var macrophageUp = macrophageTransform.up;
                
                var targetPosition = target.transform.position;
                
                // Move forward
                macrophagePosition += macrophageUp * (Time.deltaTime * _movementSpeed);
                transform.position = macrophagePosition;
                // Rotate towards target
                Vector3 targetDirection = (targetPosition - macrophagePosition).normalized;
                Vector2 newDirection = Vector2.Lerp(macrophageUp, targetDirection, _rotationSpeed * Time.deltaTime);
                transform.up = newDirection;

                float distanceToTarget = Vector2.Distance(macrophagePosition, targetPosition);
                if (distanceToTarget < 1f)
                {
                    target.SetActive(false);
                    if (killsLeft > 0 ) {
                        killsLeft -= 1;
                    }
                }
            }
        }

        void FindTarget()
        {
            List<GameObject> bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
            List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

            GameObject[] bacteria = bacteriaLarge.Concat(bacteriaSmall).ToArray();
            
            float distance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in bacteria)
            {
                if(!enemy.activeInHierarchy) continue;
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            if (closestEnemy) target = closestEnemy;
        }

        public State getState() {
            return state;
        }

        public void boost() {
            killsLeft = 200;
        }
    }
}
