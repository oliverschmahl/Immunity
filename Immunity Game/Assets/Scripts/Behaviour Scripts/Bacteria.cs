using System;
using System.Collections.Generic;
using Managers;
using Unity.Mathematics;
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
        public Vector3? targetCell = null;
        public GameObject visitedCell = null;
        public float viewRadius = 5f;
        public int damage = 1;
        public float damageRange = 0.5f;
        
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 desiredDirection;

        private void Start()
        {
            position = transform.position;
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;

            LookForTargetCell();

            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            Vector3 bottomLeftCorner = worldCorners[0];
            Vector3 topLeftCorner = worldCorners[1];
            Vector3 topRightCorner = worldCorners[2];
            bool outsideRightBorder = position.x > topRightCorner.x - GameManager.instance.playableAreaPadding;
            bool outsideLeftBorder = position.x < topLeftCorner.x + GameManager.instance.playableAreaPadding;
            bool outsideBottomBorder = position.y < bottomLeftCorner.y - GameManager.instance.playableAreaPadding;
            bool outsideTopBorder = position.y > topRightCorner.y + GameManager.instance.playableAreaPadding;

            if (outsideRightBorder || outsideLeftBorder || outsideTopBorder || outsideBottomBorder)
            {
                targetCell = new Vector3(Random.Range(topLeftCorner.x, topRightCorner.x), Random.Range(bottomLeftCorner.y, topLeftCorner.y), 0f);
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
                float smallestDistance = Mathf.Infinity;
                GameObject foundCell = null;
                foreach (GameObject cell in CellManager.Instance.cellList)
                {
                    float distance = Vector2.Distance(position, cell.transform.position);
                    if (distance < viewRadius && distance < smallestDistance && cell != visitedCell)
                    {
                        targetCell = cell.transform.position;
                        foundCell = cell;
                        smallestDistance = distance;
                    }
                }

                if (foundCell) visitedCell = foundCell;
            }

            if (targetCell != null)
            {
                if (Vector2.Distance(position, (Vector2)targetCell) < damageRange)
                {
                    targetCell = null;
                    visitedCell.GetComponent<Health>().TakeDamage(damage);
                }
            }
        }
    }
}