using Arcadeum.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    [DisallowMultipleComponent]
    public class Gameplay : MonoBehaviour
    {
        private static Gameplay _instance;

        [Header("General")]
        [SerializeField, Min(1)] private int _maxLives;
        [SerializeField] private Spaceship _spaceshipPrefab;

        [Header("Life Display")]
        [SerializeField] private Transform _lifeHolder;
        [SerializeField] private GameObject _lifeIndicator, _lostIndicator;

        [Header("Destruction")]
        [SerializeField] private ParticleSystem _playerDestructionBlast;
        [SerializeField] private float _timeOfBlast;
        [SerializeField] private float _timeAfterBlast;
        [SerializeField, Range(0, 1)] private float _imageFillPercentage;

        private int _lives;

        void Start()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this;

            if (_playerDestructionBlast == null)
                throw new Exception("No player destruction blast effect set!");

            _lives = _maxLives;
            UpdateLives();

            SpawnShip();
        }

        private void UpdateLives()
        {
            foreach (Transform t in _lifeHolder)
            {
                Destroy(t.gameObject);
            }

            for (var i = 0; i < _maxLives; i++)
            {
                if (i < _lives)
                {
                    Instantiate(_lifeIndicator, _lifeHolder);
                }
                else
                {
                    Instantiate(_lostIndicator, _lifeHolder);
                }
            }
        }

        public static void OnPlayerDeath(Vector2 lastPlayerPosition)
        {
            if (_instance == null)
                throw new Exception("There is no gameplay script instance");

            _instance._lives -= 1;
            _instance.UpdateLives();

            if (_instance._lives > 0)
            {
                _instance.StartCoroutine(RestartGame(lastPlayerPosition));
            }
            else
            {
                _instance.StartCoroutine(GameOver(lastPlayerPosition));
            }
        }

        private static IEnumerator GameOver(Vector2 lastPlayerPosition)
        {
            //Disable spawning
            SimpleAsteroidSpawner.Stop();

            yield return Blast(lastPlayerPosition);
        }
        private static IEnumerator RestartGame(Vector2 lastPlayerPosition)
        {
            //Disable spawning
            SimpleAsteroidSpawner.Stop();

            //Blast space objects
            yield return Blast(lastPlayerPosition);

            yield return new WaitForSecondsRealtime(_instance._timeAfterBlast);

            _instance.SpawnShip();

            //Enable spawning again
            SimpleAsteroidSpawner.Run();
        }

        private static IEnumerator Blast(Vector2 lastPlayerPosition)
        {
            const float radiOfBlast = 50f; // | For the best effect values on the player destruction particle system should be the same (startLifetime and startSize)

            _instance._playerDestructionBlast.startSize = radiOfBlast; //Even tho this method is deprecated its still the best method to change particle parameters realtime
            _instance._playerDestructionBlast.startLifetime = _instance._timeOfBlast; //We can't use main.startSize here because "main" is a getter.

            const int ticks = 60;

            //Create the blast
            var particle = Pool.Spawn(_instance._playerDestructionBlast.gameObject);
            particle.transform.position = lastPlayerPosition;
            Pool.Despawn(particle, 4);

            var objects = new List<SpaceObjectBehaviour>(SpaceObjectBehaviour.allSpaceObjects);

            //Remove all the bullets
            for (int j = objects.Count - 1; j >= 0; j--)
            {
                if (objects[j] is BulletBehaviour)
                {
                    objects[j].OnDeath();
                    objects.RemoveAt(j);
                }
                else if (objects[j] is Spaceship)
                {
                    objects.RemoveAt(j);
                }


            }

            //Remove all the other space objects as the blast radius enlarges
            for (var i = 0; i < ticks; i++)
            {
                var radi = radiOfBlast * (float)i / ticks * _instance._imageFillPercentage;
                var radiSq = radi * radi;

                for (int j = objects.Count - 1; j >= 0; j--)
                {
                    if (objects[j] == null)
                    {
                        objects.RemoveAt(j);
                        continue;
                    }

                    if ((objects[j].transform.position - (Vector3)lastPlayerPosition).sqrMagnitude < radiSq)
                    {
                        objects[j].OnDeath();
                        objects.RemoveAt(j);
                    }
                }

                yield return new WaitForSecondsRealtime(_instance._timeOfBlast / ticks);
            }
        }

        private void SpawnShip()
        {
            var spaceShip = Pool.Spawn(_instance._spaceshipPrefab.gameObject);
            spaceShip.transform.position = Vector3.zero;
            spaceShip.transform.rotation = Quaternion.identity;
        }
    }
}