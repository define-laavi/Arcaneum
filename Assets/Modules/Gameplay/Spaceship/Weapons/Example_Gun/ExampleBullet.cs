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


        if (col.gameObject.CompareTag("Asteroid"))
        {
            Destroy();
            Effects.FreezeFrame();
        }
        else if (_exittedPlayer && col.gameObject.CompareTag("Player"))
        {
            Destroy();
            Effects.FreezeFrame();
        }
    }

    protected override void Destroy()
    {
        var p = Instantiate(onHitParticle);
        p.transform.position = transform.position;
        Destroy(p, 0.7f);
        Destroy(this.gameObject);
    }
}
