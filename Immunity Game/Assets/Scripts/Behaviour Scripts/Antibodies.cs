using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Managers;

namespace Behaviour_Scripts
{
    public class Antibodies : MonoBehaviour
    {
        public enum State
        {
            searching=0,
            stunning=1
        }

        public Vector2 spawnTarget;
        private bool reachedSpawnTarget = false;

        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationSpeed = 1f;

        private List<GameObject> smallBacteria;
        private List<GameObject> largeBacteria;
        private GameObject[] bacteria;
        public GameObject target;

        private SpriteManager spriteManager; 

        private State state = State.searching;

        void Start() {
            spriteManager = GetComponentInChildren<SpriteManager>();
        }

        void Update() {

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

            // Check if the stunned target has been killed
            if (state == State.stunning) {
                if (target == null) {
                    state = State.searching;
                    spriteManager.changeSprite(1);
                }
                
                return;
            }

            // If searching for a target; find a target, move towards it, stun it!
            if (state == State.searching) {
                FindTarget();

                if (target) {
                    var antibodyTransform = transform;
                    var antibodyPosition = antibodyTransform.position;
                    var antibodyUp = antibodyTransform.up;

                    var targetPosition = target.transform.position;

                    // Move forward
                    antibodyPosition += antibodyUp * (Time.deltaTime * movementSpeed);
                    antibodyTransform.position = antibodyPosition;
                    // Rotate towards target
                    Vector3 targetDirection = (targetPosition - antibodyPosition).normalized;
                    Vector2 newDirection = Vector2.Lerp(antibodyUp, targetDirection, rotationSpeed * Time.deltaTime);
                    transform.up = newDirection;

                    float distanceToTarget = Vector2.Distance(antibodyPosition, targetPosition);

                    if (distanceToTarget < 5f)
                    {
                        target.GetComponent<Bacteria>().stun();
                        state = State.stunning;
                        spriteManager.changeSprite((int) State.stunning);
                    }
                }
                
                return;
            }
        }

        void FindTarget()
        {
            smallBacteria = BacteriaSmallManager.Instance.pooledBacterias;
            largeBacteria = BacteriaLargeManager.Instance.pooledBacterias;

            bacteria = largeBacteria.Concat(smallBacteria).ToArray();
            
            float distance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in bacteria)
            {
                if(!enemy.activeInHierarchy) continue;
                if(enemy.GetComponent<Bacteria>().getState() == Bacteria.State.stunned) continue;

                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            if (closestEnemy) target = closestEnemy;
        }
    }
}