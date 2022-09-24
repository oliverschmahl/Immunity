using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class BacteriaManager : MonoBehaviour
    {
        public static BacteriaManager Instance;
        public List<GameObject> bacteriaList;
        public static event Action<List<GameObject>> OnBacteriaChanged; 

        

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            GameObject[] bacteria = GameObject.FindGameObjectsWithTag("Bacteria");
            foreach (GameObject bacterium in bacteria)
            {
                bacteriaList.Add(bacterium);
            }
            OnBacteriaChanged?.Invoke(bacteriaList);
        }

        public void RemoveBacteria(GameObject bacteria)
        {
            bacteriaList.Remove(bacteria);
            Destroy(bacteria);
            OnBacteriaChanged?.Invoke(bacteriaList);
        }

        public void AddBacteria(GameObject bacteria)
        {
            // NEEDS IMPLEM
        }

        public GameObject[] GetBacteria()
        {
            return bacteriaList.ToArray();
        }
    }
}
