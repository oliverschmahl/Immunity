using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class NeutrophilManager : MonoBehaviour
    {
        public static NeutrophilManager Instance;
        public List<GameObject> neutrophilList;
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
    }
}