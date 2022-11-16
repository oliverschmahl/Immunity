using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class BacteriaLargeManager : MonoBehaviour
    {
        public static BacteriaLargeManager Instance;
        
        public List<GameObject> pooledBacterias;
        public GameObject bacteriaLargePrefab;
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
                GameObject pooledBacteria = Instantiate(bacteriaLargePrefab, transform);
                pooledBacteria.SetActive(false);
                pooledBacterias.Add(pooledBacteria);
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