using System;
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

        [SerializeField] private string selectedOrganism = "null"; 

        private void Awake()
        {
            instance = this;
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
                        break;
                    case "Macrophage":
                        MacrophageManager.Instance.Spawn(new Vector2(0f,0f), worldLocation);
                        break;
                    case "Neutrophil":
                        break;
                }
            }
        }
    }
}
