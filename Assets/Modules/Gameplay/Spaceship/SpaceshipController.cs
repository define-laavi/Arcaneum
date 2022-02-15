using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Asteroids.Modules.Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class SpaceshipController : MonoBehaviour
{
    public float mass;
    
    [Header("Linear Movement")]
    public float thrusterForce;
    public float backwardForce;
    public float maxLinearSpeed;
    public float linearStopTime; //Space has no friction, but the ship needs to come to a stop at some point.

    [Header("Angular Movement")]
    public float angularForce;
    public float maxAngularSpeed;
    public float angularStopTime; //Space has no friction, but the ship needs to come to a stop at some point.

    [Header("Other")]
    public SpaceshipGunBehaviour spaceshipGunBehaviour;

    private Vector2 _input;
    private Vector3 _speed;
    private float _angularSpeed;

    private Vector3 _position = Vector3.zero;
    private Quaternion _orientation = quaternion.identity;

    private ShipThrusterBehaviour _shipThrusterBehaviour;
    private ShipTrailBehaviour _shipTrailBehaviour;
    
    private void OnValidate()
    {
        if (mass <= 0) mass = 0.01f;
        if (thrusterForce <= 0) thrusterForce = 0.01f;
        if (angularForce <= 0) angularForce = 0.01f;
        if (backwardForce <= 0) backwardForce = 0.01f;
        if (linearStopTime <= 0) linearStopTime = 0.01f;
        if (angularStopTime <= 0) angularStopTime = 0.01f;
    }

    private void Start()
    {
        _shipThrusterBehaviour = GetComponent<ShipThrusterBehaviour>();
        _shipTrailBehaviour = GetComponent<ShipTrailBehaviour>();
        
        if(spaceshipGunBehaviour == null)
            Debug.LogWarning("Spaceship has no gun controller, is this intentional?");
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        //Dampen the speed
        _speed = Vector2.Lerp(_speed, Vector2.zero, Time.deltaTime / linearStopTime);
        _angularSpeed = Mathf.Lerp(_angularSpeed, 0, Time.deltaTime/ angularStopTime);
        
        UpdateLinear();
        UpdateAngular();
        
        _shipThrusterBehaviour?.UpdateThrust(_input);
        _shipTrailBehaviour?.UpdateTrails(_position);
        
        transform.position = _position = World.LoopInPlayArea(_position);
        transform.rotation = _orientation;

        HandleGun();
    }
   
    private void UpdateLinear()
    {
        var inputSpeed = _input.x * angularForce / mass;

        _angularSpeed += inputSpeed * Time.deltaTime;
        _angularSpeed = Mathf.Clamp(_angularSpeed, -maxAngularSpeed, maxAngularSpeed);
        
        _orientation *= Quaternion.Euler(0,0,-_angularSpeed * Time.deltaTime);
    }
    private void UpdateAngular()
    {
        var inputSpeed = _input.y * (_input.y > 0 ? thrusterForce : backwardForce) / mass;
        var acceleration = transform.up * inputSpeed;
        
        ApplyLinearForce(acceleration);

        _position = transform.position + _speed * Time.deltaTime;
    }

    public void ApplyLinearForce(Vector2 force)
    {
        _speed += (Vector3) force / mass * Time.deltaTime;
        
        if (_speed.sqrMagnitude > maxLinearSpeed * maxLinearSpeed)
            _speed = _speed.normalized * maxLinearSpeed;
    }

    private void HandleGun()
    {
        if (spaceshipGunBehaviour)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spaceshipGunBehaviour.Shoot(this);
            }
        }
    }
}
