using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic movement parameters")]
    public float mass;
    public float thrusterForce;
    public float angularForce;
    public float backwardForce;

    [Header("Clamps")]
    public float speedStopTime; //Space has no friction, but the ship needs to come to a stop at some point.
    public float angularStopTime; //Space has no friction, but the ship needs to come to a stop at some point.
    public float maxSpeed;
    public float maxAngularSpeed;

    [Header("Thrusters Visuals")] 
    public List<Transform> forwardThrusters;
    public Vector2 forwardThrustersMaxScale;
    
    public List<Transform> clockwiseAngularThrusters;
    public Vector2 clockwiseAngularThrustersMaxScale;

    public List<Transform> counterClockwiseAngularThrusters;
    public Vector2 counterClockwiseAngularThrustersMaxScale;

    public List<Transform> backwardThrusters;
    public Vector2 backwardThrusterMaxScale;


    private Vector2 _input;
    private Vector3 _speed;
    private float _angularSpeed;

    private float maxSpeedSq => maxSpeed * maxSpeed;
    
    private void OnValidate()
    {
        if (mass <= 0) mass = 0.01f;
        if (thrusterForce <= 0) thrusterForce = 0.01f;
        if (angularForce <= 0) angularForce = 0.01f;
        if (backwardForce <= 0) backwardForce = 0.01f;
        if (speedStopTime <= 0) speedStopTime = 0.01f;
        if (angularStopTime <= 0) angularStopTime = 0.01f;
    }

    void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        UpdateAngular();
        UpdateSpeed();
        
        var position = transform.position;
        position += _speed;
        position = World.PlaceInPlayArea(position);
        transform.position = position;
        transform.rotation *= Quaternion.Euler(0,0,-_angularSpeed);

        UpdateVisuals();
    }

    void UpdateSpeed()
    {
        var inputSpeed = _input.y * (_input.y > 0 ? thrusterForce : backwardForce) / mass;
        var acceleration = transform.up * inputSpeed * Time.deltaTime;
        
        _speed += acceleration;

        if (_speed.sqrMagnitude > maxSpeedSq)
            _speed = _speed.normalized * maxSpeed;

        _speed = Vector2.Lerp(_speed, Vector2.zero, Time.deltaTime / speedStopTime);
    }

    void UpdateAngular()
    {
        var inputSpeed = _input.x * angularForce / mass;

        _angularSpeed += inputSpeed * Time.deltaTime;

        _angularSpeed = Mathf.Clamp(_angularSpeed, -maxAngularSpeed, maxAngularSpeed);
       
        _angularSpeed = Mathf.Lerp(_angularSpeed, 0, Time.deltaTime/ angularStopTime);
    }

    void UpdateVisuals()
    {
        var forwardInput = Mathf.Clamp01(_input.y);
        var backwardInput = Mathf.Clamp01(-_input.y);
        var leftInput = Mathf.Clamp01(_input.x);
        var rightInput = Mathf.Clamp01(-_input.x);
        
        foreach (var forwardThruster in forwardThrusters)
        {
            forwardThruster.localScale =
                new Vector3(forwardThrustersMaxScale.x, forwardThrustersMaxScale.y, 1) * forwardInput;
        }
        
        foreach (var backwardThruster in backwardThrusters)
        {
            backwardThruster.localScale =
                new Vector3(backwardThrusterMaxScale.x, backwardThrusterMaxScale.y, 1) * backwardInput;
        }
        
        foreach (var clockwiseThruster in clockwiseAngularThrusters)
        {
            clockwiseThruster.localScale =
                new Vector3(clockwiseAngularThrustersMaxScale.x, clockwiseAngularThrustersMaxScale.y, 1) * leftInput;
        }
        
        foreach (var counterClockwiseThruster in counterClockwiseAngularThrusters)
        {
            counterClockwiseThruster.localScale =
                new Vector3(counterClockwiseAngularThrustersMaxScale.x, counterClockwiseAngularThrustersMaxScale.y, 1) * rightInput;
        }
    }
}
