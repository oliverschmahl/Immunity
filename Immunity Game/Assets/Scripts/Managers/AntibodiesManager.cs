using System;
using System.Collections.Generic;
using Behaviour_Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class AntibodiesManager : MonoBehaviour
    {
        public static AntibodiesManager Instance;
        public List<GameObject> antibodiesList;
        public GameObject antibodiesPrefab;
        public static event Action<List<GameObject>> OnAntibodiesListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Antibodies");
            foreach (GameObject cell in cells)
            {
                antibodiesList.Add(cell);
            }
            OnAntibodiesListChanged?.Invoke(antibodiesList);
        }

        public void RemoveAntibodies(GameObject cell)
        {
            antibodiesList.Remove(cell);
            Destroy(cell);
            OnAntibodiesListChanged?.Invoke(antibodiesList);
        }
        
        public void AddAntibodies(GameObject cell)
        {
            antibodiesList.Add(cell);
            OnAntibodiesListChanged?.Invoke(antibodiesList);
        }
        
        public void Spawn(Vector2 location, Vector2 spawnTarget)
        {
            var spawned = Instantiate(antibodiesPrefab, location, quaternion.identity);
            spawned.transform.parent = MacrophageManager.Instance.transform;
            spawned.GetComponent<Antibodies>().spawnTarget = spawnTarget;
            AddAntibodies(spawned);
        }
    }
}