using System.Collections;
using Managers;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{

    [SerializeField] private GameObject bacteriaSmallPrefab;
    [SerializeField] private GameObject bacteriaBigPrefab;

    [SerializeField] private float swarmBacteriaSmallInterval = 0.5f;
    [SerializeField] private float swarmBacteriaBigInterval = 2f;

    [SerializeField, Range(0f, 300f)] private float spawnDeviation = 40f;

    private Vector3 _spawnPoint;
    private readonly Vector3[] _worldCorners = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = gameObject.transform.position;
        GameManager.Instance.playableArea.GetWorldCorners(_worldCorners);
        StartCoroutine(SpawnBacterias(swarmBacteriaSmallInterval, bacteriaSmallPrefab));
        StartCoroutine(SpawnBacterias(swarmBacteriaBigInterval, bacteriaBigPrefab));
    }

    private IEnumerator SpawnBacterias(float interval, GameObject bacteria)
    {
        yield return new WaitWhile(() => GameManager.Instance.isPaused);
        yield return new WaitForSeconds(interval);
        
        // Randomness in spawn location around spawn-point
        Vector3 spawnPlacement = new Vector3(
            Mathf.Clamp(
                Random.Range(_spawnPoint.x - spawnDeviation, _spawnPoint.x + spawnDeviation), 
                _worldCorners[0].x,
                _worldCorners[2].x
            ),
            Mathf.Clamp(
                Random.Range(_spawnPoint.y - spawnDeviation, _spawnPoint.y + spawnDeviation),
                _worldCorners[0].y,
                _worldCorners[2].y
            ),
            0f
        );

        GameObject newBacteria = Instantiate(bacteria, spawnPlacement, Quaternion.identity);
        newBacteria.transform.parent = BacteriaManager.Instance.transform;
        BacteriaManager.Instance.AddBacteria(newBacteria);
        StartCoroutine(SpawnBacterias(interval, bacteria));
    }
}
