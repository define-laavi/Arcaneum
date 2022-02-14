using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceObject : ScriptableObject
{
    public List<GameObject> prefabVariants;
    public Vector2 size;

    public abstract void OnCreate(Transform t);
    public abstract void OnTransform(Transform t);
    public abstract void OnBulletHit();
    public abstract void OnPlayerHit();
}
