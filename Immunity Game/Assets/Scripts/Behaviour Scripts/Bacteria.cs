using System;
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
            stunned=1
        }

        public float maxSpeed = 1;
        public float steerStrength = 1;
        public float wanderStrength = 0.1f;
        public Vector3? targetCell = null;
        public GameObject visitedCell = null;
        public float viewRadius = 5f;
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
                List<GameObject> closeCells = new List<GameObject>();

                
                foreach (GameObject cell in CellManager.Instance.cellList)
                {
                    float distance = Vector2.Distance(position, cell.transform.position);
                    if (distance < viewRadius && cell != visitedCell)
                    {
                        closeCells.Add(cell);
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

        public void stun() {
            state = State.stunned;
            spriteManager.changeSprite((int) State.stunned);
        }

        public void wakeUp() {
            state = State.active;
            spriteManager.changeSprite((int) State.active);
        }

        public State getState() {
            return state;
        }
    }
}