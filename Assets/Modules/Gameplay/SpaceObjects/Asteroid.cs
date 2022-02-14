using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "SpaceObjects/Asteroid")]
public class Asteroid : SpaceObject
{
    public int pointsOnBulletHit;
    public List<SpaceObject> spawnOnBulletHit;

    public float minSpeed, maxSpeed;
    public float minRotationSpeed, maxRotationSpeed;
    
    private Vector2 _direction;
    private float _speed;
    private float _rotationSpeed;
    
    public override void OnCreate(Transform t)
    {
        var position = t.position;
        
        _direction = Random.insideUnitCircle.normalized; //GetRandomPositionInsidePlayArea
        _speed = Random.Range(minSpeed, maxSpeed);
        _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    public override void OnTransform(Transform t)
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
