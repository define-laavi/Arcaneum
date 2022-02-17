using Arcadeum.Common;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
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
        [SerializeField] private List<SpaceshipAction> _spaceshipActions;
        [SerializeField] private AudioClip _playerDeathSound;

        private Vector2 _input;
        private Vector3 _speed;
        private float _angularSpeed;

        private Vector2 _position;
        private Quaternion _rotation;

        private ShipThrusterBehaviour _shipThrusterBehaviour;
        private ShipTrailBehaviour _shipTrailBehaviour;

        protected override void OnStart()
        {
            _shipThrusterBehaviour = GetComponent<ShipThrusterBehaviour>();
            if (_shipThrusterBehaviour == null)
                Debug.LogWarning("Spaceship has no thruster behaviour attached, is this intentional?");

            _shipTrailBehaviour = GetComponent<ShipTrailBehaviour>();
            if (_shipTrailBehaviour == null)
                Debug.LogWarning("Spaceship has no trail behaviour attached, is this intentional?");

            if (_spaceshipActions.Count == 0)
                Debug.LogWarning("Spaceship has no actions, is this intentional?");

            if(_playerDeathSound == null)
                Debug.LogWarning("Spaceship has no death sound, is this intentional?");

            foreach (var action in _spaceshipActions)
            {
                if (action.Behaviour == null)
                    throw new Exception($"Action {action.Name} has no behaviour");
                if (action.Key == KeyCode.None)
                    throw new Exception($"Action {action.Name} has no keycode selected");
            }
        }

        protected override void OnEnabled()
        {
            //Resets all things to zero state
            transform.position = _position = Vector3.zero;
            transform.rotation = _rotation = quaternion.identity;
            _speed = Vector3.zero;
            _angularSpeed = 0;
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

            ApplyLinearImpulse(acceleration);

            _position = transform.position + _speed * Time.deltaTime;
        }
        private void HandleActions()
        {
            for(int i = _spaceshipActions.Count - 1; i >= 0; i--)
            {
                var action = _spaceshipActions[i];
                if (Input.GetKeyDown(action.Key))
                {
                    action.Behaviour?.Act(this);
                }
            }
        }

        protected override void OnHit(Collision2D col, SpaceObjectBehaviour spaceObjectBehaviour)
        {
            if (spaceObjectBehaviour is BulletBehaviour)
            {
                if ((spaceObjectBehaviour as BulletBehaviour).CanHitPlayer)
                {
                    OnDeath();
                }
            }
            else if (spaceObjectBehaviour is Asteroid)
            {
                OnDeath();
            }
        }

        public override void OnDeath()
        {
            Gameplay.OnPlayerDeath(transform.position);
            if(_playerDeathSound)
                SoundPool.Play(_playerDeathSound,0.5f);
            base.OnDeath();
        }

        /// <summary> Applies linear impulse to spaceship </summary>
        /// <param name="impulse">Bear in mind that this works for one frame so the force must be big to take some effect</param>
        public void ApplyLinearImpulse(Vector2 impulse)
        {
            _speed += (Vector3)impulse / mass * Time.deltaTime;

            if (_speed.sqrMagnitude > maxLinearSpeed * maxLinearSpeed)
            {
                _speed = _speed.normalized * maxLinearSpeed;
            }
        }
    }
}