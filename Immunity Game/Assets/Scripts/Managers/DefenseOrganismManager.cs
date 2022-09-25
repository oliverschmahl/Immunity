using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DefenseOrganismManager : MonoBehaviour
    {
        public static DefenseOrganismManager Instance;
        public List<GameObject> defenseOrganismList;
        public static event Action<List<GameObject>> OnDefenseOrganismListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
            foreach (GameObject cell in cells)
            {
                defenseOrganismList.Add(cell);
            }
            OnDefenseOrganismListChanged?.Invoke(defenseOrganismList);
        }

        public void RemoveDefenseOrganism(GameObject cell)
        {
            defenseOrganismList.Remove(cell);
            Destroy(cell);
            OnDefenseOrganismListChanged?.Invoke(defenseOrganismList);
        }
    }
}