using System;
using System.Collections;
using UnityEngine;

public abstract class SpaceshipBulletBehaviour : MonoBehaviour
{
    private bool _canHit = false;

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
        _canHit = true; //left players body
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(_canHit) OnHit(col);
    }

    protected virtual void OnCreate(){}
    protected virtual void Tick(){}
    protected abstract void Move();
    protected abstract void OnHit(Collision2D col);
}
