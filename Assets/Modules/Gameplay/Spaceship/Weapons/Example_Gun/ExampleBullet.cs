using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class ExampleBullet : SpaceshipBulletBehaviour
{
    public float speed;
    public GameObject onHitParticle;
    
    protected override void Move()
    {
        var position = transform.position;
        position += speed * transform.up * Time.deltaTime;
        position = World.LoopInPlayArea(position);
        transform.position = position;
    }

    protected override void OnHit(Collision2D col)
    {
        Effects.FreezeFrame();
        var p = Instantiate(onHitParticle);
        p.transform.position = transform.position;
        Destroy(p, 0.7f);
        Destroy(this.gameObject);
    }
}
