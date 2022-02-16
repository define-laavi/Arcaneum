using Asteroids.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SimpleAsteroidSpawner : MonoBehaviour
{
    [SerializeField] private List<Asteroid> _asteroidVariants;
    [Min(0.1f), SerializeField] private float _secondsPerAsteroid;
    [Min(0.1f), SerializeField] private float _secondsToStartSpawning;

    public void Restart()
    {
        Stop();
        StartCoroutine(AwaitStart());
    }
    public void Stop()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        if (_asteroidVariants.Count == 0)
            throw new System.Exception("No asteroid variants added to the spawner!");

        Restart();
    }
    
    private IEnumerator AwaitStart()
    {
        yield return new WaitForSecondsRealtime(_secondsToStartSpawning);
        StartCoroutine(SpawnAsteroid());
    }
    private IEnumerator SpawnAsteroid()
    {
        var a = Instantiate(_asteroidVariants[Random.Range(0, _asteroidVariants.Count)]);
        a.transform.position = World.GetRandomVectorOutsideOfPlayArea();

        yield return new WaitForSecondsRealtime(_secondsPerAsteroid);
        StartCoroutine(SpawnAsteroid());
    }
}
