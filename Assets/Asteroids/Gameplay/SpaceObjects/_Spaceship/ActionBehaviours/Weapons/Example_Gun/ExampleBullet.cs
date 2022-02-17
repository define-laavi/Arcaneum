using Arcadeum.Common;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public class ExampleBullet : BulletBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private GameObject _onHitParticle;

        protected override void OnUpdate()
        {
            var position = transform.position;
            position += _speed * transform.up * Time.deltaTime;
            position = World.LoopInPlayArea(position);
            transform.position = position;
        }
        protected override void OnHit(Collision2D col, SpaceObjectBehaviour spaceObjectBehaviour)
        {
            //We want bullets to destroy on player only after it went outside of the barrel and on anything else right after the shot.
            if (spaceObjectBehaviour.GetType() == typeof(Spaceship))
            {
                if (CanHitPlayer)
                {
                    OnDeath();
                }
            }
            else
            {
                OnDeath();
            }
        }
        public override void OnDeath()
        {
            var p = Pool.Spawn(_onHitParticle);
            p.transform.position = transform.position;
            Pool.Despawn(p, 0.7f);
            Pool.Despawn(this.gameObject);
        }
    }
}