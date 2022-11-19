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
        [SerializeField] private float speed = 2f;
        [SerializeField] private int damage = 100;
        [SerializeField] private float damageRange = 5f;
        [SerializeField] private float shakeStrength = 0.01f;

        private List<GameObject> _bacteriaSmall;
        private List<GameObject> _bacteriaLarge;

        private SpriteManager _spriteManager;
        private ParticleSystem _particleSystem;

        private bool _reachedSpawnTarget;
        private float _selfDestructTimer;
        private bool _hasExploded = false;
        
        public Vector2 spawnTarget;

        private enum State
        {
            Normal=0,
            Exploded=1,
        }
        
        private void Awake()
        {
            _spriteManager = GetComponentInChildren<SpriteManager>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (GameManager.instance.IsPaused) return;
            
            var transform1 = ((Component)this).transform;
            var pos = transform1.position;

            if (!_reachedSpawnTarget)
            {
                float step = speed * Time.deltaTime;
                Vector2 posV2 = 
                    pos = Vector2.MoveTowards(pos, spawnTarget, step);
                
                transform1.position = pos;
                ((Component)this).transform.right = spawnTarget - posV2;
                if (Vector2.Distance(((Component)this).transform.position, spawnTarget) < 5f) _reachedSpawnTarget = true;
            }

            if (!_reachedSpawnTarget) return;
            bool selfDestructTimerReached = _selfDestructTimer > 5f;
            if (!selfDestructTimerReached) {
                float x = pos.x + Random.Range(-shakeStrength, shakeStrength);
                float y = pos.y + Random.Range(-shakeStrength, shakeStrength);
                ((Component)this).transform.position = new Vector3(x, y, 0f);
                _selfDestructTimer += Time.deltaTime;
            }

            if (!selfDestructTimerReached) return;
            if (!_hasExploded) Explode(pos);
        }

        private void Explode(Vector3 neutrophilPosition)
        {
            _particleSystem.Play();
            _spriteManager.changeSprite((int)State.Exploded);
            foreach (GameObject bacterium in BacteriaSmallManager.Instance.pooledBacterias)
            {
                if (!bacterium.activeInHierarchy) continue;
                if (Vector2.Distance(neutrophilPosition, bacterium.transform.position) < damageRange) bacterium.GetComponent<Health>().TakeDamage(damage);
            }

            _hasExploded = true;
        }
    }
}