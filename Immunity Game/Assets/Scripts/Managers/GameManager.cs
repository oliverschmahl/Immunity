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
        
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private void Awake()
        {
            instance = this;
            instance.playableArea.GetWorldCorners(_worldCorners);
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
                    case "T Cell":
                        TcellManager.Instance.Spawn(new Vector2(0f, 0f) ,worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Macrophage":
                        MacrophageManager.Instance.Spawn(new Vector2(0f,0f), worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Neutrophil":
                        NeutrophilManager.Instance.Spawn(new Vector2(0f,0f), worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Complement Protein":
                        ComplementProteinManager.Instance.SpawnComplementProtein(new Vector2(0f,0f), worldLocation);
                        selectedOrganism = "null";
                        break;
                    case "Antibodies":
                        AntibodiesManager.Instance.Spawn(new Vector2(0f,0f), worldLocation);
                        selectedOrganism = "null";
                        break;
                }
            }
        }
    }
}
