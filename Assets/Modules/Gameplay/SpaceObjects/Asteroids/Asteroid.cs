using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : SpaceObjectBehaviour
{
    public int pointsOnBulletHit;
    public List<Asteroid> asteroidPrefabsToSpawn;
    public int numberOfAsteroidsToSpawn;
    
    public float minSpeed, maxSpeed;
    public float minRotationSpeed, maxRotationSpeed;
    
    private Vector2 _direction;
    private float _speed;
    private float _rotationSpeed;

    private bool _wasInGameArea = false;
    
    protected override void OnCreate()
    {
        _wasInGameArea = false;
        _direction = World.GetRandomVectorInPlayArea() - (Vector2)transform.position; //GetRandomPositionInsidePlayArea
        _speed = Random.Range(minSpeed, maxSpeed);
        _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    protected override void Move()
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

    protected override void OnHit(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Split();
            Destroy();
        }
    }

    void Split()
    {
        if (asteroidPrefabsToSpawn.Count == 0 || numberOfAsteroidsToSpawn <= 0)
            return;
        
        for (var i = 0; i < numberOfAsteroidsToSpawn; i++)
        {
            var asteroid = Instantiate(asteroidPrefabsToSpawn[Random.Range(0, asteroidPrefabsToSpawn.Count)].gameObject);
            asteroid.transform.position = transform.position;
        }
    }

    protected override void Destroy()
    {
        base.Destroy();
    }
}
