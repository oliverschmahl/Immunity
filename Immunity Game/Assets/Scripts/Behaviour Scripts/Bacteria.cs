using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    public class Bacteria : MonoBehaviour
    {
        public enum State
        {
            active=0,
            stunned=1,
            damage=2
        }

        public float maxSpeed = 1;
        public float steerStrength = 1;
        public float wanderStrength = 0.1f;
        public Vector3? targetCell = null;
        public GameObject visitedCell = null;
        public int damage = 1;
        public float damageRange = 0.5f;

        public bool beingTargeted = false;
        
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 desiredDirection;

        private SpriteManager spriteManager; 

        private State state = State.active;

        private void Start()
        {
            position = transform.position;
            spriteManager = GetComponentInChildren<SpriteManager>();
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;

            if (state == State.stunned) return;

            LookForTargetCell();
            
            if (!WorldBounds.InsideWorld(position))
            {
                targetCell = new Vector3(Random.Range(WorldBounds.GetMinX(), WorldBounds.GetMaxX()), Random.Range(WorldBounds.GetMinY(), WorldBounds.GetMaxY()), 0f);
            }
            
            if (targetCell == null) desiredDirection = (desiredDirection + Random.insideUnitCircle * wanderStrength).normalized;
            if (targetCell != null) desiredDirection = ((Vector2)targetCell - position).normalized;

            var desiredVelocity = desiredDirection * maxSpeed;
            var desiredTurnSpeed = (desiredVelocity - velocity) * steerStrength;
            var acceleration = Vector2.ClampMagnitude(desiredTurnSpeed, steerStrength) / 1;

            velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
            position += velocity * Time.deltaTime;

            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.SetPositionAndRotation(position, Quaternion.Euler(0,0, angle));
        }

        private void LookForTargetCell()
        {
            if (targetCell == null)
            {
                int maxCells = 10;
                var cells = CellManager.Instance.cellList;
                GameObject[] cellsArray = new GameObject[cells.Count];
                cells.CopyTo(cellsArray);
                
                Array.Sort(cellsArray, delegate (GameObject a, GameObject b) {
                    return Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position));
                });

                List<GameObject> closeCells = new List<GameObject>();
                
                int count = 0;
                for (int i = 0; i < cellsArray.Length; i++)
                {
                    GameObject closestObject = cellsArray[i];
                    if (closestObject.activeInHierarchy)
                    {
                        closeCells.Add(closestObject);
                        count++;
                        if (count >= maxCells)
                        {
                            break;
                        }
                    }
                }
                
                if (closeCells.Count > 0)
                {
                    var nextTarget = Random.Range(0, closeCells.Count - 1);
                    var foundCell = closeCells[nextTarget];
                    targetCell = foundCell.transform.position;
                    visitedCell = foundCell;
                }
            }

            if (targetCell != null)
            {
                if (Vector2.Distance(position, (Vector2)targetCell) < damageRange)
                {
                    targetCell = null;
                    if (visitedCell != null)
                    {
                        visitedCell.GetComponent<Health>().TakeDamage(damage);
                    }
                }
            }
        }

        public void Stun() {
            state = State.stunned;
            spriteManager.changeSprite((int) State.stunned);
        }

        public void WakeUp() {
            state = State.active;
            spriteManager.changeSprite((int) State.active);
        }

        public void DamageFlash()
        {
            StartCoroutine(DamageFlashSprite());
        }

        IEnumerator DamageFlashSprite()
        {
            spriteManager.changeSprite((int) State.damage);
            yield return new WaitForSeconds(0.10f);
            spriteManager.changeSprite((int) state);
        }

        public State GetState() {
            return state;
        }
    }
}