using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private float movementSpeed_Angry = 3f;
        [SerializeField] private float rotationSpeed_Angry = 2f;
        
        [SerializeField] private State state = State.Normal;
        [SerializeField] private int damage = 100;
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

            if (!target)
            {
                FindTarget();
            }
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
                if (distanceToTarget < 2f)
                {
                    target.GetComponent<Health>().TakeDamage(damage); // Gives damage
                    if (killsLeft > 0 ) {
                        killsLeft -= 1;
                    }

                    target = null;
                }
            }
        }

        void FindTarget()
        {
            List<GameObject> bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
            List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

            GameObject[] bacteria = bacteriaLarge.Concat(bacteriaSmall).ToArray();
            int maxBacteria = 10;
            
            Array.Sort(bacteria, delegate (GameObject a, GameObject b) {
                return Vector3.Distance(a.transform.position, transform.position)
                    .CompareTo(Vector3.Distance(b.transform.position, transform.position));
            });
            
            List<GameObject> closestBacteria = new List<GameObject>();
            
            int count = 0;
            for (int i = 0; i < bacteria.Length; i++)
            {
                GameObject closestObject = bacteria[i];
                if (closestObject.activeInHierarchy)
                {
                    closestBacteria.Add(closestObject);
                    count++;
                    if (count >= maxBacteria)
                    {
                        break;
                    }
                }
            }
            
            GameObject closestEnemy = null;
            if (closestBacteria.Count > 0)
            {
                var nextTarget = Random.Range(0, closestBacteria.Count - 1);
                var foundCell = closestBacteria[nextTarget];
                closestEnemy = foundCell;
            }
            

            if (closestEnemy)
            {
                target = closestEnemy;
            }
            
        }

        public State getState() {
            return state;
        }

        public void boost() {
            killsLeft = 200;
        }
    }
}
