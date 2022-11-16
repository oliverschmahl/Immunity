using System;
using System.Collections.Generic;
using Behaviour_Scripts;
using UnityEngine;

namespace Managers
{
    public class TcellManager : MonoBehaviour
    {
        public static TcellManager Instance;

        public GameObject tcellPrefab;
        public List<GameObject> tcellList;
        public static event Action<List<GameObject>> OnTcellListChanged;

        private void Awake()
        {
            Instance = this;
        }
        public void RemoveTcell(GameObject cell)
        {
            tcellList.Remove(cell);
            Destroy(cell);
            OnTcellListChanged?.Invoke(tcellList);
        }

        public void AddTcell(GameObject cell)
        {
            tcellList.Add(cell);
            OnTcellListChanged?.Invoke(tcellList);
        }

        public void Spawn(Vector2 location, Vector2 targetLocation)
        {
            var spawned = Instantiate(tcellPrefab, location, Quaternion.identity);
            spawned.transform.parent = Instance.transform;
            spawned.GetComponent<Tcell>().spawnTarget = targetLocation; 
            AddTcell(spawned);
        }
    }
}