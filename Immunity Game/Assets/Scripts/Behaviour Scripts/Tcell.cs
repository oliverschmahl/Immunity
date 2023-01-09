using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class Tcell : MonoBehaviour
    {
        public Vector2 spawnTarget;
        private bool reachedSpawnTarget = false;
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationSpeed = 0.8f;

        private List<GameObject> macrophages;

        private GameObject target;

        void Start()
        {
            macrophages = MacrophageManager.Instance.macrophageList;
        }

        void Update()
        {
            // If game is paused stop behaviour
            if (GameManager.instance.isPaused) return;

            if (!reachedSpawnTarget)
            {
                float step = movementSpeed * Time.deltaTime;
                var _transform = transform;
                var _pos = transform.position;
                Vector2 _posV2 = 
                _pos = Vector2.MoveTowards(_pos, spawnTarget, step);
                
                _transform.position = _pos;
                transform.right = spawnTarget - _posV2;
                if (Vector2.Distance(transform.position, spawnTarget) < 5f) reachedSpawnTarget = true;
                return;
            }
            
            macrophages = MacrophageManager.Instance.macrophageList; // Update macrophage list
            bool areThereMacrophages = macrophages.Count > 0; // Sets true if any macrophages exists
            if (!areThereMacrophages) return; // If there are no macrophages, then do nothing

            FindTarget();

            if (target) {
                var tCellTransform = transform;
                var tCellPosition = tCellTransform.position;
                var tCellUp = tCellTransform.up;

                var targetPosition = target.transform.position;

                // Move forward
                tCellPosition += tCellUp * (Time.deltaTime * movementSpeed);
                tCellTransform.position = tCellPosition;
                // Rotate towards target
                Vector3 targetDirection = (targetPosition - tCellPosition).normalized;
                Vector2 newDirection = Vector2.Lerp(tCellUp, targetDirection, rotationSpeed * Time.deltaTime);
                transform.up = newDirection;

                float distanceToTarget = Vector2.Distance(tCellPosition, targetPosition);

                if (distanceToTarget < 5f)
                {
                    target.GetComponent<Macrophage>().boost();
                    TcellManager.Instance.RemoveTcell(transform.gameObject);
                }
            }
        }

        void FindTarget()
        {
            float distance = Mathf.Infinity;
            GameObject closestMacrophage = null;
            foreach (GameObject macrophage in macrophages)
            {
                if (macrophage.GetComponent<Macrophage>().getState() == Macrophage.State.Disabled) {
                    float distanceToMacrophage = Vector2.Distance(transform.position, macrophage.transform.position);
                    if (distanceToMacrophage < distance)
                    {
                        closestMacrophage = macrophage;
                        distance = distanceToMacrophage;
                    }
                }
            }

            if (closestMacrophage) 
            {
                target = closestMacrophage;
            }
        }
    }
}