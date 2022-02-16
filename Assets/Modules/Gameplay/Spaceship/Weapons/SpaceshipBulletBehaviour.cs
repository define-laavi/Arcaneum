﻿using UnityEngine;

public abstract class SpaceshipBulletBehaviour : MonoBehaviour
{
    protected bool _exittedPlayer = false;

    private void OnEnable()
    {
        OnCreate();
    }

    private void Update()
    {
        Tick();
        Move();
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        _exittedPlayer = true; //left players body
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnHit(col);
    }

    protected virtual void OnCreate(){}
    protected virtual void Tick(){}
    protected virtual void Move(){}
    protected virtual void OnHit(Collision2D col){}
    protected virtual void Destroy(){Destroy(this.gameObject);}
}
