using UnityEngine;
using UnityEngine.Serialization;

public class BacteriaMovement : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)] private float movementSpeed = 1f;
    [SerializeField, Range(1f, 5f)] private float rotationSpeed = 3f;

    private Vector3 _target;

    void Start()
    {
        LookForCell(transform);
    }

    void Update()
    {
        // Movement code
        transform.position += transform.up * (Time.deltaTime * movementSpeed);

        // Rotation code
        // if there is no target gameObject create a random temporary waypoint
        if (_target.Equals(new Vector3(0f, 0f, 0f)))
        {
            _target = CreateRandomTarget(transform.position);
        }

        // if target is within distance search for new
        if (Vector3.Distance(transform.position, _target) < 0.3f)
        {
            
            _target = CreateRandomTarget(transform.position);
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
            if (distance < 1 && distance < smallestDistance)
            {
                smallestDistance = distance;
                closestCell = cell;
            }
        }

        _target = closestCell.transform.position;
    }

    private Vector3 CreateRandomTarget(Vector3 bacteriaPosition)
    {
        return new Vector3(
            Random.Range(bacteriaPosition.x - 1f, bacteriaPosition.x + 1f),
            Random.Range(bacteriaPosition.y - 1f, bacteriaPosition.y + 1f),
            0f
        );
    }
}