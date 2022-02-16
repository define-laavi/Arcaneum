using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class ShipTrailBehaviour : MonoBehaviour
{
    public List<TrailRenderer> trails;

    public void UpdateTrails(Vector2 position)
    {
        SetEmission(World.IsInPlayArea(position));
    }

    private void SetEmission(bool emits)
    {
        foreach (var trail in trails)
        {
            if(!emits) trail.Clear();
            trail.emitting = emits;
        }
    }
}
