using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ComplementProteinManager : MonoBehaviour
    {
        public static ComplementProteinManager Instance;
        public List<GameObject> complementProteinList;
        public static event Action<List<GameObject>> OnComplementProteinListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Complement Protein");
            foreach (GameObject cell in cells)
            {
                complementProteinList.Add(cell);
            }
            OnComplementProteinListChanged?.Invoke(complementProteinList);
        }

        public void RemoveAntibodies(GameObject cell)
        {
            complementProteinList.Remove(cell);
            Destroy(cell);
            OnComplementProteinListChanged?.Invoke(complementProteinList);
        }
    }
}