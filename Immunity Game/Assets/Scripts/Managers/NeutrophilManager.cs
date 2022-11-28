using System;
using System.Collections.Generic;
using Behaviour_Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class NeutrophilManager : MonoBehaviour
    {
        public static NeutrophilManager Instance;
        public List<GameObject> neutrophilList;
        public GameObject neutrophilPrefab;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Neutrophil");
            foreach (GameObject cell in cells)
            {
                neutrophilList.Add(cell);
            }
        }

        public void Remove(GameObject cell)
        {
            neutrophilList.Remove(cell);
            Destroy(cell);
        }
        
        public void Add(GameObject cell)
        {
            neutrophilList.Add(cell);
        }

        public void Spawn(Vector2 location, Vector2 spawnTarget)
        {
            var spawned = Instantiate(neutrophilPrefab, location, quaternion.identity);
            spawned.transform.parent = Instance.transform;
            spawned.GetComponent<Neutrophil>().spawnTarget = spawnTarget;
            Add(spawned);
        }
    }
}