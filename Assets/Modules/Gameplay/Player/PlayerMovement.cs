using System.Collections;
using System.Collections.Generic;
using Asteroids.Modules.Gameplay;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;
    
    void Update()
    {
        var position = transform.position;
        position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * speed;
        position = World.PlaceInPlayArea(position);
        transform.position = position;
    }
}
