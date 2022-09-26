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
        [SerializeField, Range(100f, 1000f)] private float randomTargetDistance = 500f;
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
            transform.position += transform.up * (Time.deltaTime * movementSpeed);

            // Rotation code
            // if there is no target gameObject create a random temporary waypoint
            if (_target.magnitude == 0)
            {
                CreateRandomTarget(transform.position);
            }

            // if target is within distance search for new 
            if (Vector3.Distance(transform.position, _target) < visionDistance)
            {
                CreateRandomTarget(transform.position);
                LookForCell(transform);
            }

            Vector3 targetDirection = (_target - transform.position).normalized;
            Vector2 newDirection = Vector2.Lerp(transform.up, targetDirection, rotationSpeed * Time.deltaTime);
        
            transform.up = newDirection;
        }

        private void LookForCell(Transform bacteriaTransform)
        {
            if (_cellList == null) return;
            // find the closest cell to the bacteria.
            float smallestDistance = Mathf.Infinity;
            GameObject closestCell = null;
            foreach (GameObject cell in _cellList)
            {
                Vector3 difference = cell.transform.position - bacteriaTransform.position;
                float distance = difference.sqrMagnitude;
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
                    _worldCorners[0].x,
                    _worldCorners[2].x
                ),
                Mathf.Clamp(
                    Random.Range(bacteriaPosition.y - randomTargetDistance, bacteriaPosition.y + randomTargetDistance),
                    _worldCorners[0].y,
                    _worldCorners[2].y
                ),
                0f
            );
        }
        
        private void OnTriggerEnter2D(Collider2D col) 
        {
            Collider2D collidingInstance = col;

            if (!collidingInstance.CompareTag("Cell")) return;
            if (collidingInstance.GetComponent<Health>() == null) return;
            collidingInstance.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(collidingInstance + " Is attacking");
        }
    }
}