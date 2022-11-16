using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class AntibodiesManager : MonoBehaviour
    {
        public static AntibodiesManager Instance;
        public List<GameObject> antibodiesList;
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
    }
}