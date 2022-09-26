using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour_Scripts
{
    public class Bacteria : MonoBehaviour
    {
        [SerializeField, Range(1f, 200f)] private float movementSpeed = 75f;
        [SerializeField, Range(1f, 5f)] private float rotationSpeed = 2f;
        [SerializeField, Range(100f, 2000f)] private float randomTargetDistance = 500f;
        [SerializeField, Range(10f, 1000f)] private float visionDistance = 200f;
        [SerializeField] private int damage = 5;

        // internal variables
        private GameObject[] _cellList;

        private Vector3 _target;
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private void Awake()
        {
            CellManager.OnCellListChanged += CellListChanged;
        }

        private void CellListChanged(List<GameObject> cellList)
        {
            _cellList = cellList.ToArray();
        }

        private void OnDestroy()
        {
            CellManager.OnCellListChanged -= CellListChanged;
        }
        
        private void Start()
        {
            LookForCell(transform);
            GameManager.Instance.playableArea.GetWorldCorners(_worldCorners);
        }

        private void Update()
        {
            if (GameManager.Instance.IsPaused) return;
            // Movement code
            var bacteriaTransform = transform;
            var bacteriaPosition = bacteriaTransform.position;
            var bacteriaUp = bacteriaTransform.up;
            
            transform.position += bacteriaUp * (Time.deltaTime * movementSpeed);

            // Rotation code
            // if there is no target gameObject create a random temporary waypoint
            if (_target.magnitude == 0)
            {
                CreateRandomTarget(bacteriaPosition);
            }

            // if target is within distance search for new 
            if (Vector3.Distance(bacteriaPosition, _target) < visionDistance)
            {
                CreateRandomTarget(bacteriaPosition);
                LookForCell(bacteriaTransform);
            }
            
            var targetDirection = (_target - bacteriaPosition).normalized;
            var newDirection = Vector2.Lerp(bacteriaUp, targetDirection, rotationSpeed * Time.deltaTime);
            
            if (bacteriaPosition.x > _worldCorners[2].x + 100f) BacteriaManager.Instance.RemoveBacteria(gameObject);
            if (bacteriaPosition.x < _worldCorners[0].x + -100f) BacteriaManager.Instance.RemoveBacteria(gameObject);
            if (bacteriaPosition.y > _worldCorners[2].y + 100f) BacteriaManager.Instance.RemoveBacteria(gameObject);
            if (bacteriaPosition.y < _worldCorners[0].y + -100f) BacteriaManager.Instance.RemoveBacteria(gameObject);
            
            transform.up = newDirection;
        }

        private void LookForCell(Transform bacteriaTransform)
        {
            if (_cellList == null) return;
            // find the closest cell to the bacteria.
            var smallestDistance = Mathf.Infinity;
            GameObject closestCell = null;
            foreach (var cell in _cellList)
            {
                var difference = cell.transform.position - bacteriaTransform.position;
                var distance = difference.sqrMagnitude;
                if (distance < visionDistance && distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestCell = cell;
                }
            }

            if (closestCell)
            {
                _target = closestCell.transform.position;
            }
        }

        private void CreateRandomTarget(Vector3 bacteriaPosition)
        {
            _target = new Vector3(
                Mathf.Clamp(
                    Random.Range(bacteriaPosition.x - randomTargetDistance, bacteriaPosition.x + randomTargetDistance), 
                    _worldCorners[0].x + 200f,
                    _worldCorners[2].x - 200f
                ),
                Mathf.Clamp(
                    Random.Range(bacteriaPosition.y - randomTargetDistance, bacteriaPosition.y + randomTargetDistance),
                    _worldCorners[0].y + 200f,
                    _worldCorners[2].y - 200f
                ),
                0f
            );
        }
        
        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (!col.CompareTag("Cell")) return;
            if (col.GetComponent<Health>() == null) return;
            col.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(col + " Is attacking");
        }
    }
}