using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    public class Neutrophil : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Range(10f, 300f)] private float movementSpeed = 50f;
        [SerializeField, Range(0.1f, 5f)] private float rotationSpeed = 0.5f;
        [SerializeField] private int damage = 100;
        [SerializeField, Range(10f, 200f)] private float damageRange = 50f;
        [SerializeField, Range(10f, 200f)] private float randomMovementRangeBeforeDetonation = 50f;
        [SerializeField, Range(0.1f, 20f)] private float shakeStrength = 0.5f;

        private GameObject[] _bacteria;
        private GameObject[] _cells;
        private Vector3 _target;
        private readonly Vector3[] _worldCorners = new Vector3[4];

        private bool _reachedTarget;
        private float _selfDestructTimer;
        private bool _hasExploded = false;


        private void Awake()
        {
            BacteriaManager.OnBacteriaListChanged += BacteriaListChanged;
            CellManager.OnCellListChanged += CellListChanged;
        }

        private void OnDestroy()
        {
            BacteriaManager.OnBacteriaListChanged -= BacteriaListChanged;
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
            GameManager.instance.playableArea.GetWorldCorners(_worldCorners);
            Vector3 neutrophilPosition = transform.position;
            _target = new Vector3(
                Mathf.Clamp(
                    Random.Range(neutrophilPosition.x - randomMovementRangeBeforeDetonation,
                        neutrophilPosition.x + randomMovementRangeBeforeDetonation),
                    _worldCorners[0].x,
                    _worldCorners[2].x
                ),
                Mathf.Clamp(
                    Random.Range(neutrophilPosition.y - randomMovementRangeBeforeDetonation,
                        neutrophilPosition.y + randomMovementRangeBeforeDetonation),
                    _worldCorners[0].y,
                    _worldCorners[2].y
                ),
                0f
            );
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;
            var neutrophilTransform = transform;
            var neutrophilPosition = neutrophilTransform.position;
            var neutrophilUp = neutrophilTransform.up;
            
            if (!_reachedTarget)
            {
                // Move forward
                neutrophilPosition += neutrophilUp * (Time.deltaTime * movementSpeed);
                neutrophilTransform.position = neutrophilPosition;
                // Rotate towards target
                Vector3 targetDirection = (_target - neutrophilPosition).normalized;
                Vector2 newDirection = Vector2.Lerp(neutrophilUp, targetDirection, rotationSpeed * Time.deltaTime);
                transform.up = newDirection;

                if (Vector2.Distance(neutrophilPosition, _target) < 2f) _reachedTarget = true;
            }

            if (!_reachedTarget) return;
            bool selfDestructTimerReached = _selfDestructTimer > 5f;
            if (!selfDestructTimerReached) {
                float x = neutrophilPosition.x + Random.Range(-shakeStrength, shakeStrength);
                float y = neutrophilPosition.y + Random.Range(-shakeStrength, shakeStrength);
                transform.position = new Vector3(x, y, 0f);
                _selfDestructTimer += Time.deltaTime;
            }

            if (!selfDestructTimerReached) return;
            if (!_hasExploded) Explode(neutrophilPosition);
        }

        private void Explode(Vector3 neutrophilPosition)
        {
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Characters/Defense Organisms/Neutrophil_Explode");
            foreach (GameObject bacterium in _bacteria)
            {
                if (Vector2.Distance(neutrophilPosition, bacterium.transform.position) < damageRange) bacterium.GetComponent<Health>().TakeDamage(damage);
            }

            _hasExploded = true;
        }
    }
}