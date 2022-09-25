using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CellManager : MonoBehaviour
    {
        public static CellManager Instance;
        public List<GameObject> cellList;
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

        public void RemoveCell(GameObject cell)
        {
            cellList.Remove(cell);
            Destroy(cell);
            OnCellListChanged?.Invoke(cellList);
        }
    }
}
