using UnityEngine;

namespace Behaviour_Scripts
{
    public class Bacteria : MonoBehaviour
    {
        [SerializeField, Range(1f, 200f)] private float movementSpeed = 75f;
        [SerializeField, Range(1f, 5f)] private float rotationSpeed = 2f;
        [SerializeField, Range(100f, 1000f)] private float randomTargetDistance = 500f;
        [SerializeField, Range(10f, 1000f)] private float visionDistance = 200f;

        [SerializeField] private GameObject gameCanvas;
    
        private Vector3 _target = new(0f, 0f, 0f);
        private readonly Vector3[] _worldCorners = new Vector3[4];

        void Start()
        {
            LookForCell(transform);
            gameCanvas.GetComponent<RectTransform>().GetWorldCorners(_worldCorners);
        }

        void Update()
        {
            // Movement code
            transform.position += transform.up * (Time.deltaTime * movementSpeed);

            // Rotation code
            // if there is no target gameObject create a random temporary waypoint
            if (_target.Equals(new Vector3(0f, 0f, 0f)))
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
            // find the closest cell to the bacteria.
            GameObject[] cells = GameObject.FindGameObjectsWithTag("cell");
            float smallestDistance = Mathf.Infinity;
            GameObject closestCell = null;
            foreach (GameObject cell in cells)
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
    }
}