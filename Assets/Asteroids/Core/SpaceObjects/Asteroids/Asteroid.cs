using Arcadeum.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public class Asteroid : SpaceObjectBehaviour
    {
        [Header("General")]
        [SerializeField] private SharedInt _pointsOnBulletHit;

        [Header("Movement")]
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minRotationSpeed;
        [SerializeField] private float _maxRotationSpeed;

        [Header("Split")]
        [SerializeField] private List<Asteroid> _asteroidPrefabsToSpawn;
        [SerializeField] private int _numberOfAsteroidsToSpawn;
        [SerializeField] private ParticleSystem _destructionParticleSystem;
        [SerializeField] private AudioClip _onDeathSound;

        private Vector2 _direction;
        private float _speed;
        private float _rotationSpeed;
        private bool _wasInGameArea = false;

        protected override void OnEnabled()
        {
            _wasInGameArea = false;
            _direction = (World.GetRandomVectorInPlayArea() - (Vector2)transform.position).normalized; //GetRandomPositionInsidePlayArea
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);
        }

        protected override void OnUpdate()
        {
            var position = transform.position;

            if (!_wasInGameArea) _wasInGameArea = World.IsInPlayArea(position); //Test if we entered the game area

            position += (Vector3)_direction * _speed * Time.deltaTime;

            if (_wasInGameArea) position = World.LoopInPlayArea(position); //Loop our position

            transform.position = position;
            transform.rotation *= Quaternion.Euler(0, 0, _rotationSpeed * Time.deltaTime);
        }

        protected override void OnHit(Collision2D col, SpaceObjectBehaviour spaceObjectBehaviour)
        {
            if (spaceObjectBehaviour is BulletBehaviour) //if we hit any type of bullet
            {
                Split();
                OnDeath();
                Score.AddScore(_pointsOnBulletHit.Value);
            }
        }
        private void Split()
        {
            if (_asteroidPrefabsToSpawn.Count == 0 || _numberOfAsteroidsToSpawn <= 0) return;

            for (var i = 0; i < _numberOfAsteroidsToSpawn; i++)
            {
                var asteroid = Pool.Spawn(_asteroidPrefabsToSpawn[Random.Range(0, _asteroidPrefabsToSpawn.Count)].gameObject);
                asteroid.transform.position = transform.position;
            }
        }

        public override void OnDeath()
        {
            if (_destructionParticleSystem != null)
            {
                var a = Pool.Spawn(_destructionParticleSystem.gameObject);
                a.transform.position = transform.position;
                Pool.Despawn(a, a.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
            }
            if (_onDeathSound != null)
            {
                SoundPool.Play(_onDeathSound, 0.5f);
            }
            Pool.Despawn(gameObject);

        }
    }
}
