using System;
using System.Collections.Generic;
using Behaviour_Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class ComplementProteinManager : MonoBehaviour
    {
        public static ComplementProteinManager Instance;
        public List<GameObject> complementProteinList;
        public GameObject complementProteinPrefab;
        public static event Action<List<GameObject>> OnComplementProteinListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] complementProteins = GameObject.FindGameObjectsWithTag("Complement Protein");
            foreach (GameObject complementProtein in complementProteins)
            {
                complementProteinList.Add(complementProtein);
            }
            OnComplementProteinListChanged?.Invoke(complementProteinList);
        }
        
        public void Remove(GameObject complementProtein)
        {
            complementProteinList.Remove(complementProtein);
            Destroy(complementProtein);
            OnComplementProteinListChanged?.Invoke(complementProteinList);
        }

        public void Spawn(Vector2 location, Vector2 spawnTarget)
        {
            var spawnedObject = Instantiate(complementProteinPrefab, location, quaternion.identity);
            spawnedObject.transform.parent = Instance.transform;
            spawnedObject.GetComponent<ComplementProtein>().spawnTarget = spawnTarget;
            AddComplementProteinToList(spawnedObject);
        }

        public void AddComplementProteinToList(GameObject complementProtein)
        {
            complementProteinList.Add(complementProtein);
            OnComplementProteinListChanged?.Invoke((complementProteinList));
        }
    }
}