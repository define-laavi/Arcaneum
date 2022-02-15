using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceObject : MonoBehaviour
{
    public Vector2 size;

    private void Start()
    {
        OnCreate();
    }

    private void Update()
    {
        OnTransform();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Bullet"))
            OnBulletHit();
        else if (col.gameObject.CompareTag("Player"))
            OnPlayerHit();
    }

    public abstract void OnCreate();
    public abstract void OnTransform();
    public abstract void OnBulletHit();
    public abstract void OnPlayerHit();
}
