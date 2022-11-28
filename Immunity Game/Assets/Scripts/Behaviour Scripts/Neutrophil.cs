using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour_Scripts
{
    [SelectionBase]
    public class Neutrophil : MonoBehaviour
    {
        #region State
        [Space(3f), Header("Movement")] public State state = State.Normal;
        public enum State
        {
            Normal,
            Exploded,
        }
        #endregion
        
        #region Movement Variables
        [Space(3f), Header("Movement")]
        public float maxSpeed = 2f;
        public float steerStrength = 1;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _desiredDirection;
        #endregion

        #region Damage Variables
        [Space(3f), Header("Damage")]
        [Tooltip("Damage dealt to bacteria within the explosion radius")]
        public int damage = 100;
        public float damageRange = 5f;
        private CircleCollider2D _explosionCollider;
        public int guuDamage = 2;
        #endregion

        #region Goo
        [Space(3f), Header("Goo")]
        public float gooSeconds = 25f;
        private float _passedTime = 0f;
        private float _lastTime = 0f;
        private float _selfDestructTimer;
        private bool _hasExploded = false;
        private CircleCollider2D _gooCollider;
        private ParticleSystem _particleSystem;
        #endregion

        #region Visual Effects
        [Space(3f), Header("Visual Effects")]
        public float shakeStrength = 0.01f;
        private SpriteManager _spriteManager;
        #endregion

        #region Targets
        [Space(3f), Header("Targeting")]
        public Vector2 spawnTarget;
        private bool _reachedSpawnTarget;
        #endregion

        private void Awake()
        {
            _position = transform.position;
            _spriteManager = GetComponentInChildren<SpriteManager>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _gooCollider = GetComponentInChildren<CircleCollider2D>();
            _explosionCollider = GetComponentInChildren<CircleCollider2D>();
        }

        private void Start()
        {
            _spriteManager.changeSprite((int) state);
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;
            
            _spriteManager.changeSprite((int) state);

            if (!_reachedSpawnTarget)
            {
                _desiredDirection = ((Vector2)spawnTarget - _position).normalized;
                
                var desiredVelocity = _desiredDirection * maxSpeed;
                var desiredTurnSpeed = (desiredVelocity - _velocity) * steerStrength;
                var acceleration = Vector2.ClampMagnitude(desiredTurnSpeed, steerStrength) / 1;

                _velocity = Vector2.ClampMagnitude(_velocity + acceleration * Time.deltaTime, maxSpeed);
                _position += _velocity * Time.deltaTime;

                var angle = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;
                transform.SetPositionAndRotation(_position, Quaternion.Euler(0,0, angle));

                if (Vector2.Distance(_position, spawnTarget) < 1f) _reachedSpawnTarget = true;
                return;
            }
            
            bool selfDestructTimerReached = _selfDestructTimer > 5f;
            if (!selfDestructTimerReached) {
                float x = _position.x + Random.Range(-shakeStrength, shakeStrength);
                float y = _position.y + Random.Range(-shakeStrength, shakeStrength);
                ((Component)this).transform.position = new Vector3(x, y, 0f);
                _selfDestructTimer += Time.deltaTime;
            }

            if (!selfDestructTimerReached) return;
            if (!_hasExploded)
            {
                Explode(_position);
            }

            gooSeconds -= Time.deltaTime;
            if (gooSeconds < -2f) NeutrophilManager.Instance.Remove(gameObject);
            if (gooSeconds <= 0f)
            {
                _particleSystem.Stop();
                return;
            }
            
            
            _passedTime += Time.deltaTime;
            List<Collider2D> objectsInsideGuuArea = new List<Collider2D>();
            _gooCollider.OverlapCollider(new ContactFilter2D(), objectsInsideGuuArea);

            foreach (Collider2D objectInsideGuuArea in objectsInsideGuuArea)
            {
                GameObject objectInsideGameObject = objectInsideGuuArea.gameObject;
                if (objectInsideGameObject.CompareTag("Bacteria Large") ||
                    objectInsideGameObject.CompareTag("Bacteria Small") ||
                    objectInsideGameObject.CompareTag("Cell") ||
                    objectInsideGameObject.CompareTag("T Cell") ||
                    objectInsideGameObject.CompareTag("Macrophage") ||
                    objectInsideGameObject.CompareTag("Complement Protein") ||
                    objectInsideGameObject.CompareTag("Antibodies"))
                {
                    if (_passedTime > _lastTime + 1) objectInsideGuuArea.gameObject.GetComponent<Health>().TakeDamage(guuDamage);
                }
            }

            if (_passedTime > _lastTime + 1) _lastTime = _passedTime;

        }

        private void Explode(Vector3 neutrophilPosition)
        {
            _particleSystem.Play();
            state = State.Exploded;
            
            List<Collider2D> objectsInsideExplosionArea = new List<Collider2D>();
            _explosionCollider.OverlapCollider(new ContactFilter2D(), objectsInsideExplosionArea);

            foreach (Collider2D objectInsideExplosionArea in objectsInsideExplosionArea)
            {
                GameObject objectInsideGameObject = objectInsideExplosionArea.gameObject;
                if (objectInsideGameObject.CompareTag("Bacteria Large") ||
                    objectInsideGameObject.CompareTag("Bacteria Small") ||
                    objectInsideGameObject.CompareTag("Cell") ||
                    objectInsideGameObject.CompareTag("T Cell") ||
                    objectInsideGameObject.CompareTag("Macrophage") ||
                    objectInsideGameObject.CompareTag("Complement Protein") ||
                    objectInsideGameObject.CompareTag("Antibodies"))
                {
                    objectInsideExplosionArea.gameObject.GetComponent<Health>().TakeDamage(damage);
                }
            }
            _hasExploded = true;
        }
    }
}