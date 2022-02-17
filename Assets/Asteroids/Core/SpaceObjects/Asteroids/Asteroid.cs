using Arcadeum.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public class Asteroid : SpaceObjectBehaviour
    {
        [Header("General")]
        public SharedInt pointsOnBulletHit;

        [Header("Movement")]
        public float minSpeed;
        public float maxSpeed;
        public float minRotationSpeed, maxRotationSpeed;

        [Header("Split")]
        public List<Asteroid> asteroidPrefabsToSpawn;
        public int numberOfAsteroidsToSpawn;
        public ParticleSystem destructionParticleSystem;
        [SerializeField] private AudioClip _onDeathSound;

        private Vector2 _direction;
        private float _speed;
        private float _rotationSpeed;
        private bool _wasInGameArea = false;

        protected override void OnEnabled()
        {
            _wasInGameArea = false;
            _direction = (World.GetRandomVectorInPlayArea() - (Vector2)transform.position).normalized; //GetRandomPositionInsidePlayArea
            _speed = Random.Range(minSpeed, maxSpeed);
            _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
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
                Score.AddScore(pointsOnBulletHit.Value);
            }
        }
        private void Split()
        {
            if (asteroidPrefabsToSpawn.Count == 0 || numberOfAsteroidsToSpawn <= 0) return;

            for (var i = 0; i < numberOfAsteroidsToSpawn; i++)
            {
                var asteroid = Pool.Spawn(asteroidPrefabsToSpawn[Random.Range(0, asteroidPrefabsToSpawn.Count)].gameObject);
                asteroid.transform.position = transform.position;
            }
        }

        public override void OnDeath()
        {
            var a = Pool.Spawn(destructionParticleSystem.gameObject);
            a.transform.position = transform.position;
            Pool.Despawn(a, a.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
            Pool.Despawn(gameObject);
            SoundPool.Play(_onDeathSound, 0.5f);
        }
    }
}
