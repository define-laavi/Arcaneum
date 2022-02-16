using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class SpaceshipActionBehaviour : MonoBehaviour
{
    public abstract void Act(Spaceship spaceship);
}

[System.Serializable]
public class SpaceshipAction
{
    public string name;
    public SpaceshipActionBehaviour behaviour;
    public KeyCode key;
}