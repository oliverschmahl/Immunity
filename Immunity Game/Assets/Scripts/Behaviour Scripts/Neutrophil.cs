using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    public class Neutrophil : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Range(10f, 300f)] private float speed = 50f;
        [SerializeField, Range(0.1f, 5f)] private float rotationSpeed = 0.5f;
        [SerializeField] private int damage = 100;
        [SerializeField, Range(10f, 200f)] private float damageRange = 50f;
        [SerializeField, Range(10f, 200f)] private float randomMovementRangeBeforeDetonation = 50f;
        [SerializeField, Range(0.1f, 20f)] private float shakeStrength = 0.5f;

        private List<GameObject> _bacteriaSmall;
        private List<GameObject> _bacteriaLarge;
        
        private GameObject[] _bacteria;
        private GameObject[] _cells;
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private bool _reachedSpawnTarget;
        private float _selfDestructTimer;
        private bool _hasExploded = false;
        
        public Vector2 spawnTarget;


        private void Awake()
        {
            CellManager.OnCellListChanged += CellListChanged;
        }

        private void OnDestroy()
        {
            CellManager.OnCellListChanged -= CellListChanged;
        }

        private void CellListChanged(List<GameObject> cellList)
        {
            _cells = cellList.ToArray();
        }

        private void BacteriaListChanged(List<GameObject> bacteriaList)
        {
            _bacteria = bacteriaList.ToArray();
        }

        private void Start()
        { 
            _bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
            _bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

            _bacteria = _bacteriaLarge.Concat(_bacteriaSmall).ToArray();
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;
            
            var _transform = transform;
            var _pos = _transform.position;

            if (!_reachedSpawnTarget)
            {
                float step = speed * Time.deltaTime;
                Vector2 _posV2 = 
                    _pos = Vector2.MoveTowards(_pos, spawnTarget, step);
                
                _transform.position = _pos;
                transform.right = spawnTarget - _posV2;
                if (Vector2.Distance(transform.position, spawnTarget) < 5f) _reachedSpawnTarget = true;
            }

            if (!_reachedSpawnTarget) return;
            bool selfDestructTimerReached = _selfDestructTimer > 5f;
            if (!selfDestructTimerReached) {
                float x = _pos.x + Random.Range(-shakeStrength, shakeStrength);
                float y = _pos.y + Random.Range(-shakeStrength, shakeStrength);
                transform.position = new Vector3(x, y, 0f);
                _selfDestructTimer += Time.deltaTime;
            }

            if (!selfDestructTimerReached) return;
            if (!_hasExploded) Explode(_pos);
        }

        private void Explode(Vector3 neutrophilPosition)
        {
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Characters/New_Defence_Organisms/Neutrophil_Explode.png");
            foreach (GameObject bacterium in _bacteria)
            {
                if (Vector2.Distance(neutrophilPosition, bacterium.transform.position) < damageRange) bacterium.GetComponent<Health>().TakeDamage(damage); print(bacterium.name);
            }

            _hasExploded = true;
        }
    }
}