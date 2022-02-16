using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class ExampleBullet : BulletBehaviour
{
    public float speed;
    public GameObject onHitParticle;
    
    protected override void OnUpdate()
    {
        var position = transform.position;
        position += speed * transform.up * Time.deltaTime;
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
        var p = Pool.Spawn(onHitParticle);
        p.transform.position = transform.position;
        Pool.Despawn(p, 0.7f);
        Pool.Despawn(this.gameObject);
    }
}
