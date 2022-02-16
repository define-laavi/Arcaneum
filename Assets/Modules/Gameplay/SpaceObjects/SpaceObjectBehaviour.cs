using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceObjectBehaviour : MonoBehaviour
{
    private void Start()
    {
        OnCreate();
    }
    private void Update()
    {
        Tick();
        Move();
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
