using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public bool isPaused;
        public RectTransform playableArea;
        public GameObject macrophage;
        public GameObject neutrophil;
        public GameObject bacteriaSmall;
        public GameObject bacteriaLarge;

        private void Awake()
        {
            Instance = this;
        }

        public bool IsPaused
        {
            get => isPaused;
            set => isPaused = value;
        }

        public RectTransform PlayableArea => playableArea;

        public GameObject Macrophage => macrophage;

        public GameObject Neutrophil => neutrophil;

        public GameObject BacteriaSmall => bacteriaSmall;

        public GameObject BacteriaLarge => bacteriaLarge;
    }
}
