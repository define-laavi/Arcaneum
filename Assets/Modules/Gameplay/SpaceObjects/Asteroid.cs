using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class Asteroid : SpaceObject
{
    public int pointsOnBulletHit;
    public List<SpaceObject> spawnOnBulletHit;

    public float minSpeed, maxSpeed;
    public float minRotationSpeed, maxRotationSpeed;
    
    private Vector2 _direction;
    private float _speed;
    private float _rotationSpeed;
    
    public override void OnCreate()
    {
        var position = transform.position;
        
        _direction = World.GetRandomVectorInPlayArea() - (Vector2)position; //GetRandomPositionInsidePlayArea
        _speed = Random.Range(minSpeed, maxSpeed);
        _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    public override void OnTransform()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBulletHit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerHit()
    {
        throw new System.NotImplementedException();
    }
}
