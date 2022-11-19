using System;
using System.Collections.Generic;
using Managers;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    public class Bacteria : MonoBehaviour
    {
        public float maxSpeed = 1;
        public float steerStrength = 1;
        public float wanderStrength = 0.1f;
        public GameObject targetCell = null;
        public GameObject visitedCell = null;
        public float viewRadius = 5f;
        public int damage = 1;
        public float damageRange = 0.05f;
        
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 desiredDirection;
        
        private Rigidbody2D body;
        private Collider2D collider2D;

        private void Start()
        {
            position = transform.position;
            body = GetComponent<Rigidbody2D>();
            
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;

            LookForTargetCell();
            if (!targetCell) desiredDirection = (desiredDirection + Random.insideUnitCircle * wanderStrength).normalized;
            if (targetCell) desiredDirection = ((Vector2)targetCell.transform.position - position).normalized;

            var desiredVelocity = desiredDirection * maxSpeed;
            var desiredTurnSpeed = (desiredVelocity - velocity) * steerStrength;
            var acceleration = Vector2.ClampMagnitude(desiredTurnSpeed, steerStrength) / 1;

            velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
            position += velocity * Time.deltaTime;

            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            //transform.SetPositionAndRotation(position, Quaternion.Euler(0,0, angle));
            body.position = position; 
            body.SetRotation(Quaternion.Euler(0,0, angle));
            

            
            
        }

        private void LookForTargetCell()
        {
            if (!targetCell)
            {
                float smallestDistance = Mathf.Infinity;
                foreach (GameObject cell in CellManager.Instance.cellList)
                {
                    float distance = Vector2.Distance(position, cell.transform.position);
                    if (distance < viewRadius && distance < smallestDistance && cell != visitedCell)
                    {
                        targetCell = cell;
                        smallestDistance = distance;
                    }
                }
            }

            if (targetCell)
            {
                if (Vector2.Distance(position, targetCell.transform.position) < damageRange)
                {
                    visitedCell = targetCell;
                    targetCell = null;
                }
            }
        }
    }
}