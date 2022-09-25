using System.Collections;
using Managers;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{

    [SerializeField] private GameObject bacteriaSmallPrefab;
    [SerializeField] private GameObject bacteriaBigPrefab;

    [SerializeField] private float swarmBacteriaSmallInterval = 0.5f;
    [SerializeField] private float swarmBacteriaBigInterval = 2f;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBacterias(swarmBacteriaSmallInterval, bacteriaSmallPrefab));
        StartCoroutine(SpawnBacterias(swarmBacteriaBigInterval, bacteriaBigPrefab));
    }

    private IEnumerator SpawnBacterias(float interval, GameObject bacteria)
    {
        yield return new WaitForSeconds(interval);
        Vector3 spawnPlacement = new Vector3(Random.Range(-5f, 5), 2f, 0);
        GameObject newBacteria = Instantiate(bacteria, spawnPlacement, Quaternion.identity);
        newBacteria.transform.parent = BacteriaManager.Instance.transform;
        BacteriaManager.Instance.AddBacteria(newBacteria);
        StartCoroutine(SpawnBacterias(interval, bacteria));
    }
}
