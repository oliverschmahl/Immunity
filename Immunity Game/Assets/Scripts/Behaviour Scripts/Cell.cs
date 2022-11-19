using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    public class Cell : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 200f)] private float movementSpeed = 0.1f;
        [SerializeField, Range(100f, 2000f)] private float randomTargetDistance = 100f;
        
        private bool _capturedStopBehaviour = false;
        
        private Vector3 _target;
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private void Start()
        {
            GameManager.instance.playableArea.GetWorldCorners(_worldCorners);
            CreateRandomTarget(currentCellPosition: transform.position);
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;
            if (_capturedStopBehaviour) return;
            MoveCell();
        }

        private void MoveCell()
        {
            var cellTransform = transform;
            var cellPosition = cellTransform.position;
            var cellUp = cellTransform.up;
           
            var targetIsReached = _target == cellPosition;

            if (targetIsReached)
            {
                 CreateRandomTarget(cellPosition);
            }
            
            var distance = Vector2.Distance(cellPosition, _target);
            Vector2 targetDirection = _target - cellPosition.normalized;
            var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            
            transform.position = Vector2.MoveTowards(transform.position, _target, movementSpeed * Time.deltaTime/10);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            RemoveIfOutOfBounds(cellPosition);
        }

        private void RemoveIfOutOfBounds(Vector3 cellPosition)
        {
            if (cellPosition.x > _worldCorners[2].x + 100f) gameObject.SetActive(false);
            if (cellPosition.x < _worldCorners[0].x + -100f) gameObject.SetActive(false);
            if (cellPosition.y > _worldCorners[2].y + 100f) gameObject.SetActive(false);
            if (cellPosition.y < _worldCorners[0].y + -100f) gameObject.SetActive(false);
        }

        private void CreateRandomTarget(Vector3 currentCellPosition)
        {
            _target = new Vector3(
                Mathf.Clamp(
                    Random.Range(currentCellPosition.x - randomTargetDistance, currentCellPosition.x + randomTargetDistance), 
                    _worldCorners[0].x + 200f,
                    _worldCorners[2].x - 200f
                ),
                Mathf.Clamp(
                    Random.Range(currentCellPosition.y - randomTargetDistance, currentCellPosition.y + randomTargetDistance),
                    _worldCorners[0].y + 200f,
                    _worldCorners[2].y - 200f
                ),
                0f
            );
        }
    }
}