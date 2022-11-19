using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class BacteriaSmallManager : MonoBehaviour
    {
        public static BacteriaSmallManager Instance;
        
        public List<GameObject> pooledBacterias;
        public GameObject bacteriaSmallPrefab;
        public int pooledAmount;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            pooledBacterias = new List<GameObject>();
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject pooledBacteria = Instantiate(bacteriaSmallPrefab, transform);
                pooledBacteria.SetActive(false);
                pooledBacterias.Add(pooledBacteria);
            }
            
            GameObject[] placedManually = GameObject.FindGameObjectsWithTag("Bacteria Small");
            foreach (GameObject bacteria in placedManually)
            {
                pooledBacterias.Add(bacteria);
            }
        }

        public void SpawnBacteria(Vector2 spawnPosition)
        {
            foreach (GameObject bacteria in pooledBacterias)
            {
                if (!bacteria.activeInHierarchy)
                {
                    bacteria.transform.position = spawnPosition;
                    bacteria.transform.rotation = quaternion.identity;
                    bacteria.SetActive(true);
                    return;
                }
            }
        }
    }
}
