using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class CellManager : MonoBehaviour
    {
        public static CellManager Instance;
        [FormerlySerializedAs("CellList")] public List<GameObject> cellList;
        public static event Action<List<GameObject>> OnCellListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
            foreach (GameObject cell in cells)
            {
                cellList.Add(cell);
            }
            OnCellListChanged?.Invoke(cellList);
        }
    }
}
