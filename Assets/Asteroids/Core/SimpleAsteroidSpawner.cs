using Arcadeum.Common;
using Arcadeum.Asteroids.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    [DisallowMultipleComponent]
    public class SimpleAsteroidSpawner : MonoBehaviour
    {
        private static SimpleAsteroidSpawner _instance;

        [Min(0.1f), SerializeField] private float _secondsPerAsteroid;
        [Min(0.1f), SerializeField] private float _secondsToStartSpawning;
        [SerializeField] private List<Asteroid> _asteroidVariants;

        public static void Run()
        {
            if (_instance == null)
                throw new System.Exception("There is no simple asteroid spawner instance!");

            _instance.StartCoroutine(_instance.AwaitStart());
        }
        public static void Stop()
        {
            if (_instance == null)
                throw new System.Exception("There is no simple asteroid spawner instance!");

            _instance.StopAllCoroutines();
        }

        private void Start()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this;

            if (_asteroidVariants.Count == 0)
                throw new System.Exception("No asteroid variants added to the spawner!");

            Run();
        }

        private IEnumerator AwaitStart()
        {
            yield return new WaitForSecondsRealtime(_secondsToStartSpawning);
            StartCoroutine(SpawnAsteroid());
        }
        private IEnumerator SpawnAsteroid()
        {
            var a = Pool.Spawn(_asteroidVariants[Random.Range(0, _asteroidVariants.Count)].gameObject);
            a.transform.position = World.GetRandomVectorOutsideOfPlayArea();

            yield return new WaitForSecondsRealtime(_secondsPerAsteroid);
            StartCoroutine(SpawnAsteroid());
        }
    }
}