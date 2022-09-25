using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(spawnBacterias(swarmBacteriaSmallInterval, bacteriaSmallPrefab));
        StartCoroutine(spawnBacterias(swarmBacteriaBigInterval, bacteriaBigPrefab));
    }

    private IEnumerator spawnBacterias(float interval, GameObject bacteria)
    {
        yield return new WaitForSeconds(interval);
        Vector3 spawnPlacement = new Vector3(Random.Range(-5f, 5), 2f, 0);
        GameObject newBacteria = Instantiate(bacteria, spawnPlacement, Quaternion.identity);
        StartCoroutine(spawnBacterias(interval, bacteria));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
