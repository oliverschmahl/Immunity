using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Internal;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public bool isPaused;
        public RectTransform playableArea;
        [SerializeField, Range(0f, 100f)] public float playableAreaPadding = 2f;
        [SerializeField] private string selectedOrganism = "null";
        
        public Vector2 macrophageSpawnPos;
        public Vector2 neutrophilSpawnPos;
        public Vector2 complementProteinSpawnPos;
        public Vector2 tcellSpawnPos;
        public Vector2 antibodiesSpawnPos;
        
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private void Awake()
        {
            instance = this;
            instance.playableArea.GetWorldCorners(_worldCorners);
        
            macrophageSpawnPos = new Vector2(-40,-86);
            neutrophilSpawnPos = new Vector2(-20,-83);
            complementProteinSpawnPos = new Vector2(0, -82);
            tcellSpawnPos = new Vector2(20, -82);
            antibodiesSpawnPos = new Vector2(40, -84);
        }

        public Vector3[] GetWorldCorners()
        {
            return _worldCorners;
        }

        public bool IsPaused
        {
            get => isPaused;
            set => isPaused = value;
        }

        public void SetSelectedOrganism(string organism)
        {
            selectedOrganism = organism;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedOrganism.Equals("null")) return;
                Vector3 worldLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldLocation.z = 10;
                switch (selectedOrganism)
                {
                    case "Macrophage":
                        MacrophageManager.Instance.Spawn(macrophageSpawnPos, worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Neutrophil":
                        NeutrophilManager.Instance.Spawn(neutrophilSpawnPos, worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Complement Protein":
                        ComplementProteinManager.Instance.SpawnComplementProtein(complementProteinSpawnPos, worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "T Cell":
                        TcellManager.Instance.Spawn(tcellSpawnPos ,worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Antibodies":
                        AntibodiesManager.Instance.Spawn(antibodiesSpawnPos, worldLocation);
                        selectedOrganism = "null";
                        break;
                }
            }
        }
    }
}
