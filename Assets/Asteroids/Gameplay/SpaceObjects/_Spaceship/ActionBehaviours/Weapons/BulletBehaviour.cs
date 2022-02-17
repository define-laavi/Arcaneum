﻿using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public abstract class BulletBehaviour : SpaceObjectBehaviour
    {
        public bool CanHitPlayer => _leftPlayer;
        protected bool _leftPlayer = false;

        protected override void OnEnabled()
        {
            _leftPlayer = false;
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            _leftPlayer = true; //left players body
        }
    }
}