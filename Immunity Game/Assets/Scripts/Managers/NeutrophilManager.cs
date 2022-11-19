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
        public static event Action<List<GameObject>> OnNeutrophilListChanged;

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
            OnNeutrophilListChanged?.Invoke(neutrophilList);
        }

        public void RemoveAntibodies(GameObject cell)
        {
            neutrophilList.Remove(cell);
            Destroy(cell);
            OnNeutrophilListChanged?.Invoke(neutrophilList);
        }
        
        public void AddMacrophage(GameObject cell)
        {
            neutrophilList.Add(cell);
            OnNeutrophilListChanged?.Invoke(neutrophilList);
        }

        public void Spawn(Vector2 location, Vector2 spawnTarget)
        {
            var spawned = Instantiate(neutrophilPrefab, location, quaternion.identity);
            spawned.transform.parent = MacrophageManager.Instance.transform;
            spawned.GetComponent<Neutrophil>().spawnTarget = spawnTarget;
            AddMacrophage(spawned);
        }
    }
}