using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : SpaceObjectBehaviour
{
    [Header("General")]
    public int pointsOnBulletHit;

    [Header("Movement")]
    public float minSpeed;
    public float maxSpeed;
    public float minRotationSpeed, maxRotationSpeed;

    [Header("Split")]
    public List<Asteroid> asteroidPrefabsToSpawn;
    public int numberOfAsteroidsToSpawn;
    
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
        
        if (World.IsInPlayArea(position))
            _wasInGameArea = true;

        position += (Vector3) _direction * _speed * Time.deltaTime;

        if (_wasInGameArea)
            position = World.LoopInPlayArea(position);

        transform.position = position;
        transform.rotation *= quaternion.Euler(0,0,_rotationSpeed*Time.deltaTime);
    }

    protected override void OnHit(Collision2D col, SpaceObjectBehaviour spaceObjectBehaviour)
    {
        if (spaceObjectBehaviour.GetType().IsSubclassOf(typeof(SpaceshipBulletBehaviour))) //if we hit any type of bullet
        {
            Split();
            OnDestroy();
        }
    }
    private void Split()
    {
        if (asteroidPrefabsToSpawn.Count == 0 || numberOfAsteroidsToSpawn <= 0)
            return;
        
        for (var i = 0; i < numberOfAsteroidsToSpawn; i++)
        {
            var asteroid = Instantiate(asteroidPrefabsToSpawn[Random.Range(0, asteroidPrefabsToSpawn.Count)].gameObject);
            asteroid.transform.position = transform.position;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
