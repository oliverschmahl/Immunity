using System.Collections;
using Managers;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{
    [SerializeField] private float swarmBacteriaSmallInterval = 0.5f;
    [SerializeField] private float swarmBacteriaBigInterval = 2f;
    [SerializeField] public int numberOfSmallBacteriaToSpawn = 10;
    [SerializeField] public int numberOfLargeBacteriaToSpawn = 10;

    [SerializeField, Range(0f, 300f)] private float spawnDeviation = 40f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBacterias(swarmBacteriaSmallInterval, "small"));
        StartCoroutine(SpawnBacterias(swarmBacteriaBigInterval, "large"));
    }

    private IEnumerator SpawnBacterias(float interval, string type)
    {
        yield return new WaitWhile(() => GameManager.instance.isPaused);
        yield return new WaitForSeconds(interval);

        if (type.Equals("small") && numberOfSmallBacteriaToSpawn > 0)
        {
            BacteriaSmallManager.Instance.SpawnBacteria(transform.position);
            numberOfSmallBacteriaToSpawn -= 1;
        }
        
        if (type.Equals("large") && numberOfLargeBacteriaToSpawn > 0)
        {
            BacteriaLargeManager.Instance.SpawnBacteria(transform.position);
            numberOfLargeBacteriaToSpawn -= 1;
        }
        
        StartCoroutine(SpawnBacterias(interval, type));
    }
}
