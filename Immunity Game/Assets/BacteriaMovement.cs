using UnityEngine;
using UnityEngine.Serialization;

public class BacteriaMovement : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)] private float movementSpeed = 1f;
    [SerializeField, Range(1f, 5f)] private float rotationSpeed = 2f;
    private static Vector3 _target;
        
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
        if (_target.Equals(new Vector3(0f,0f,0f)))
        {
            _target = new Vector3(
                Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f),
                Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f),
                0f
            );
            
        }
        
        // if target is within distance reset it
        
        Vector3 targetDirection = (_target - transform.position).normalized;
        Vector3 newDirection = Vector3.Slerp(transform.up, targetDirection, rotationSpeed * Time.deltaTime);
        transform.up = newDirection;
    }

    private static void LookForCell(Transform bacteria)
    {
        // find the closest cell to the bacteria.
        GameObject[] cells = GameObject.FindGameObjectsWithTag("cell");
        float smallestDistance = Mathf.Infinity;
        foreach (GameObject cell in cells)
        {
            Vector3 difference = cell.transform.position - bacteria.position;
            float distance = difference.sqrMagnitude;
            if (distance < 1 && distance < smallestDistance)
            {
                smallestDistance = distance;
                _target = cell.transform.position;
            }
        }
    }
}
