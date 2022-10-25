using System;
using Managers;
using UnityEngine;

namespace Behaviour_Scripts
{
    public class Tcell : MonoBehaviour
    {
        public Vector2 spawnTarget;
        private bool reachedSpawnTarget = false;
        [SerializeField, Range(1,100)] private int speed = 5;

        private void Update()
        {
            // If game is paused stop behaviour
            if (GameManager.instance.isPaused) return;

            if (!reachedSpawnTarget)
            {
                float step = speed * Time.deltaTime;
                var _transform = transform;
                var _pos = transform.position;
                Vector2 _posV2 = 
                _pos = Vector2.MoveTowards(_pos, spawnTarget, step);
                
                _transform.position = _pos;
                transform.right = spawnTarget - _posV2;
                if (Vector2.Distance(transform.position, spawnTarget) < 5f) reachedSpawnTarget = true;
            }
        }
    }
}