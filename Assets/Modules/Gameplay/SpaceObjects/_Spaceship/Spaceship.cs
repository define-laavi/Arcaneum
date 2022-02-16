using System;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using Unity.Mathematics;
using UnityEngine;

public class Spaceship : SpaceObjectBehaviour
{
    [Min(0.01f)] public float mass;
    
    [Header("Linear Movement")]
    [Min(0.01f)] public float thrusterForce;
    [Min(0.01f)] public float backwardForce;
    [Min(0.01f)] public float maxLinearSpeed;
    [Min(0.01f)] public float linearStopTime; //Space has no friction, but the ship needs to come to a stop at some point.

    [Header("Angular Movement")]
    [Min(0.01f)] public float angularForce;
    [Min(0.01f)] public float maxAngularSpeed;
    [Min(0.01f)] public float angularStopTime; //Space has no friction, but the ship needs to come to a stop at some point.

    [Header("Other")]
    public List<SpaceshipAction> spaceshipActions;
    public ParticleSystem playerDeathParticle;

    private Vector2 _input;
    private Vector3 _speed;
    private float _angularSpeed;

    private Vector2 _position;
    private Quaternion _rotation;

    private ShipThrusterBehaviour _shipThrusterBehaviour;
    private ShipTrailBehaviour _shipTrailBehaviour;

    protected override void OnEnabled()
    {
        transform.position = _position = Vector3.zero;
        transform.rotation = _rotation = quaternion.identity;

        _shipThrusterBehaviour = GetComponent<ShipThrusterBehaviour>();
        if (_shipThrusterBehaviour == null)
            Debug.LogWarning("Spaceship has no thruster behaviour attached, is this intentional?");

        _shipTrailBehaviour = GetComponent<ShipTrailBehaviour>();
        if (_shipTrailBehaviour == null)
            Debug.LogWarning("Spaceship has no trail behaviour attached, is this intentional?");

        if (spaceshipActions.Count == 0)
            Debug.LogWarning("Spaceship has no actions, is this intentional?");

        foreach(var action in spaceshipActions)
        {
            if (action.behaviour == null)
                throw new Exception($"Action {action.name} has no behaviour");
            if (action.key == KeyCode.None)
                throw new Exception($"Action {action.name} has no keycode selected");
        }
    }

    protected override void OnUpdate()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Dampen the speed
        _speed = Vector2.Lerp(_speed, Vector2.zero, Time.deltaTime / linearStopTime);
        _angularSpeed = Mathf.Lerp(_angularSpeed, 0, Time.deltaTime / angularStopTime);

        UpdateAngular();
        UpdateLinear();

        _shipThrusterBehaviour?.UpdateThrust(_input);
        _shipTrailBehaviour?.UpdateTrails(_position);

        transform.position = _position = World.LoopInPlayArea(_position);
        transform.rotation = _rotation;

        HandleActions();
    }
    private void UpdateAngular()
    {
        var inputSpeed = _input.x * angularForce / mass;

        _angularSpeed += inputSpeed * Time.deltaTime;
        _angularSpeed = Mathf.Clamp(_angularSpeed, -maxAngularSpeed, maxAngularSpeed);

        _rotation *= Quaternion.Euler(0, 0, -_angularSpeed * Time.deltaTime);
    }
    private void UpdateLinear()
    {
        var inputSpeed = _input.y * (_input.y > 0 ? thrusterForce : backwardForce) / mass;
        var acceleration = transform.up * inputSpeed;

        ApplyLinearForce(acceleration);

        _position = transform.position + _speed * Time.deltaTime;
    }
    private void HandleActions()
    {
        foreach(var action in spaceshipActions)
        {
            if (Input.GetKeyDown(action.key))
                action.behaviour?.Act(this);
        }
    }

    protected override void OnHit(Collision2D col, SpaceObjectBehaviour spaceObjectBehaviour)
    {
        if (spaceObjectBehaviour is BulletBehaviour)
        {

            if ((spaceObjectBehaviour as BulletBehaviour).CanHitPlayer)
                OnDeath();
        }
        else if(spaceObjectBehaviour is Asteroid)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        Gameplay.OnPlayerDeath(transform.position);
        base.OnDeath();
    }

    public void ApplyLinearForce(Vector2 force)
    {
        _speed += (Vector3) force / mass * Time.deltaTime;
        
        if (_speed.sqrMagnitude > maxLinearSpeed * maxLinearSpeed)
            _speed = _speed.normalized * maxLinearSpeed;
    }
}
