using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxBehaviour : MonoBehaviour
{
    public Material material;

    public float speed;

    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private float currentRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRot += speed * Time.deltaTime;
        currentRot %= 360;
        material.SetFloat(Rotation, currentRot);
    }
}
