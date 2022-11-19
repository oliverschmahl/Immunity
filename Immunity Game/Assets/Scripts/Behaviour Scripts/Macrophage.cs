using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class Macrophage : MonoBehaviour
    {
        public enum State
        {
            normal=0,
            disabled=1,
            angry=2
        }

        // Serialized fields
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationSpeed = 0.5f;
        [SerializeField] private int killsLeft = 100;
        
        private List<GameObject> _bacteriaSmall;
        private List<GameObject> _bacteriaLarge;
        private GameObject[] _bacteria;
        public GameObject _target;
        
        public Vector2 spawnTarget;
        private bool _reachedSpawnTarget = false;

        public GameObject sprite;
        private SpriteManager spriteManager; 

        private State state = State.normal;

        private void Start()
        {
            spriteManager = sprite.GetComponent<SpriteManager>();
        }

        void Update()
        {
            if (GameManager.instance.IsPaused) return;

            // update state and sprite based on kill count
            if (killsLeft > 100) {
                if (state != State.angry) {
                    state = State.angry;
                    spriteManager.changeSprite((int) State.angry);
                    movementSpeed = 300f;
                    rotationSpeed = 3f;
                }
            } else if (killsLeft <= 100 && killsLeft > 0) {
                if (state != State.normal) {
                    state = State.normal;
                    spriteManager.changeSprite((int) State.normal);
                    movementSpeed = 50f;
                    rotationSpeed = 0.5f;
                }
            } else {
                if (state != State.disabled) {
                    state = State.disabled;
                    spriteManager.changeSprite((int) State.disabled);
                } 
            }

            // check if the macrophage is disabled before proceeding
            if (state == State.disabled) {
                return;
            }


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
                if (distanceToTarget < 1f)
                {
                    _target.SetActive(false);
                    if (killsLeft > 0 ) {
                        killsLeft -= 1;
                    }
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

        public State getState() {
            return state;
        }

        public void boost() {
            killsLeft = 200;
        }
    }
}
