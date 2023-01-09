using System.Collections;
using Managers;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{
    [SerializeField] private float swarmBacteriaSmallInterval = 0.05f;
    [SerializeField] private float swarmBacteriaBigInterval = 0.1f;
    [SerializeField] public int numberOfSmallBacteriaToSpawn = 10;
    [SerializeField] public int numberOfLargeBacteriaToSpawn = 10;

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
