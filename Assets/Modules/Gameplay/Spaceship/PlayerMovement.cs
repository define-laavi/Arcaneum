using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public float mass;
    
    [Header("Movement")]
    public float thrusterForce;
    public float backwardForce;
    public float maxLinearSpeed;
    public float linearStopTime; //Space has no friction, but the ship needs to come to a stop at some point.
    
    [Header("Movement Thrusters")]
    public List<Transform> forwardThrusters;
    public Vector2 forwardThrustersMaxScale;
    public List<Transform> backwardThrusters;
    public Vector2 backwardThrusterMaxScale;
    
    [Header("Rotation")]
    public float angularForce;
    public float maxAngularSpeed;
    public float angularStopTime; //Space has no friction, but the ship needs to come to a stop at some point.

    [Header("Rotation Thrusters")]
    public List<Transform> clockwiseThrusters;
    public Vector2 clockwiseThrustersMaxScale;
    public List<Transform> counterClockwiseThrusters;
    public Vector2 counterClockwiseThrustersMaxScale;
    
    
    private Vector2 _input;
    private Vector3 _speed;
    private float _angularSpeed;

    private void OnValidate()
    {
        if (mass <= 0) mass = 0.01f;
        if (thrusterForce <= 0) thrusterForce = 0.01f;
        if (angularForce <= 0) angularForce = 0.01f;
        if (backwardForce <= 0) backwardForce = 0.01f;
        if (linearStopTime <= 0) linearStopTime = 0.01f;
        if (angularStopTime <= 0) angularStopTime = 0.01f;
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        //Dampen the speed
        _speed = Vector2.Lerp(_speed, Vector2.zero, Time.deltaTime / linearStopTime);
        _angularSpeed = Mathf.Lerp(_angularSpeed, 0, Time.deltaTime/ angularStopTime);
        
        UpdateRotation();
        UpdateMovement();

        UpdateThrusterVisuals();
    }
   
    private void UpdateRotation()
    {
        var inputSpeed = _input.x * angularForce / mass;

        _angularSpeed += inputSpeed * Time.deltaTime;
        _angularSpeed = Mathf.Clamp(_angularSpeed, -maxAngularSpeed, maxAngularSpeed);
        
        transform.rotation *= Quaternion.Euler(0,0,-_angularSpeed * Time.deltaTime);
    }
    private void UpdateMovement()
    {
        var inputSpeed = _input.y * (_input.y > 0 ? thrusterForce : backwardForce) / mass;
        var acceleration = transform.up * inputSpeed * Time.deltaTime;
        
        _speed += acceleration;

        if (_speed.sqrMagnitude > maxLinearSpeed * maxLinearSpeed)
            _speed = _speed.normalized * maxLinearSpeed;

        var position = transform.position + _speed * Time.deltaTime;
        transform.position = World.LoopInPlayArea(position);
    }
    private void UpdateThrusterVisuals()
    {
        var forwardInput = Mathf.Clamp01(_input.y);
        var backwardInput = Mathf.Clamp01(-_input.y);
        var leftInput = Mathf.Clamp01(_input.x);
        var rightInput = Mathf.Clamp01(-_input.x);
        
        foreach (var forwardThruster in forwardThrusters)
        {
            forwardThruster.localScale = new Vector3(forwardThrustersMaxScale.x, forwardThrustersMaxScale.y, 1) * forwardInput;
        }
        
        foreach (var backwardThruster in backwardThrusters)
        {
            backwardThruster.localScale = new Vector3(backwardThrusterMaxScale.x, backwardThrusterMaxScale.y, 1) * backwardInput;
        }
        
        foreach (var clockwiseThruster in clockwiseThrusters)
        {
            clockwiseThruster.localScale = new Vector3(clockwiseThrustersMaxScale.x, clockwiseThrustersMaxScale.y, 1) * leftInput;
        }
        
        foreach (var counterClockwiseThruster in counterClockwiseThrusters)
        {
            counterClockwiseThruster.localScale = new Vector3(counterClockwiseThrustersMaxScale.x, counterClockwiseThrustersMaxScale.y, 1) * rightInput;
        }
    }
}
