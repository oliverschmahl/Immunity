using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BacteriaManager : MonoBehaviour
    {
        public static BacteriaManager Instance;
        public List<GameObject> bacteriaList;
        public static event Action<List<GameObject>> OnBacteriaListChanged; 
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] bacteria = GameObject.FindGameObjectsWithTag("Bacteria");
            foreach (GameObject bacterium in bacteria)
            {
                bacteriaList.Add(bacterium);
            }
            OnBacteriaListChanged?.Invoke(bacteriaList);
        }
        
        public void RemoveBacteria(GameObject bacteria)
        {
            bacteriaList.Remove(bacteria);
            Destroy(bacteria);
            OnBacteriaListChanged?.Invoke(bacteriaList);
        }
    }
}
