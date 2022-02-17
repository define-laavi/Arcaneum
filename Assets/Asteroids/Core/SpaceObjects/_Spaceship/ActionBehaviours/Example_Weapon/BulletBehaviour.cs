using UnityEngine;

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

        //We have to check whether the bullet left the player, otherwise the player would self the struct while trying to shoot.
        private void OnCollisionExit2D(Collision2D col)
        {
            _leftPlayer = true; //left players body
        }
    }
}