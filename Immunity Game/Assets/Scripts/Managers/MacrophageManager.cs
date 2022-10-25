using System;
using System.Collections.Generic;
using Behaviour_Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class MacrophageManager : MonoBehaviour
    {
        public static MacrophageManager Instance;
        public List<GameObject> macrophageList;
        public GameObject macrophagePrefab;
        public static event Action<List<GameObject>> OnMacrophageListChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject[] cells = GameObject.FindGameObjectsWithTag("Macrophage");
            foreach (GameObject cell in cells)
            {
                macrophageList.Add(cell);
            }
            OnMacrophageListChanged?.Invoke(macrophageList);
        }

        public void RemoveMacrophage(GameObject cell)
        {
            macrophageList.Remove(cell);
            Destroy(cell);
            OnMacrophageListChanged?.Invoke(macrophageList);
        }

        public void AddMacrophage(GameObject cell)
        {
            macrophageList.Add(cell);
            OnMacrophageListChanged?.Invoke(macrophageList);
        }

        public void Spawn(Vector2 location, Vector2 spawnTarget)
        {
            var spawned = Instantiate(macrophagePrefab, location, quaternion.identity);
            spawned.transform.parent = MacrophageManager.Instance.transform;
            spawned.GetComponent<Macrophage>().spawnTarget = spawnTarget;
            AddMacrophage(spawned);
        }
    }
}