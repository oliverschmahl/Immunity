using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public bool isPaused;
        public RectTransform playableArea;
        
        [SerializeField] private CellManager cellManager;
        [SerializeField] private GameObject cellPrefab;

        [SerializeField] private MacrophageManager macrophageManager;
        [SerializeField] private GameObject macrophagePrefab;
        
        [SerializeField] private BacteriaManager bacteriaManager;
        [SerializeField] private GameObject bacteriaSmallPrefab;
        [SerializeField] private GameObject bacteriaLargePrefab;
        
        [SerializeField] private AntibodiesManager antibodiesManager;
        [SerializeField] private GameObject antibodiesPrefab;
        
        [SerializeField] private NeutrophilManager neutrophilManager;
        [SerializeField] private GameObject neutrophilPrefab;
        
        [SerializeField] private ComplementProteinManager complementProteinManager;
        [SerializeField] private GameObject complementProteinPrefab;

        private void Awake()
        {
            Instance = this;
        }

        public bool IsPaused
        {
            get => isPaused;
            set => isPaused = value;
        }
    }
}
