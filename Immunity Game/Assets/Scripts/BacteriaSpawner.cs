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

    private readonly Vector3[] _worldCorners = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playableArea.GetWorldCorners(_worldCorners);
        StartCoroutine(SpawnBacterias(swarmBacteriaSmallInterval, "small"));
        StartCoroutine(SpawnBacterias(swarmBacteriaBigInterval, "large"));
    }

    private IEnumerator SpawnBacterias(float interval, string type)
    {
        yield return new WaitWhile(() => GameManager.instance.isPaused);
        yield return new WaitForSeconds(interval);

        if (type.Equals("small")) BacteriaSmallManager.Instance.SpawnBacteria(transform.position);
        if (type.Equals("large")) BacteriaLargeManager.Instance.SpawnBacteria(transform.position);
        
        StartCoroutine(SpawnBacterias(interval, type));
    }
}
