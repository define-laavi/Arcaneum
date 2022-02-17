using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusterBehaviour : MonoBehaviour
{
    [Header("Movement Thrusters")]
    public List<Transform> forwardThrusters;
    public Vector2 forwardThrustersMaxScale;
    public List<Transform> backwardThrusters;
    public Vector2 backwardThrusterMaxScale;
    
    [Header("Rotation Thrusters")]
    public List<Transform> clockwiseThrusters;
    public Vector2 clockwiseThrustersMaxScale;
    public List<Transform> counterClockwiseThrusters;
    public Vector2 counterClockwiseThrustersMaxScale;

    public void UpdateThrust(Vector2 playerInput)
    {
        var forwardInput = Mathf.Clamp01(playerInput.y);
        var backwardInput = Mathf.Clamp01(-playerInput.y);
        var leftInput = Mathf.Clamp01(playerInput.x);
        var rightInput = Mathf.Clamp01(-playerInput.x);
        
        foreach (var forwardThruster in forwardThrusters)
        {
            forwardThruster.localScale = new Vector3(forwardThrustersMaxScale.x, forwardThrustersMaxScale.y, 1) * forwardInput;
        }
        
        foreach (var backwardThruster in backwardThrusters)
        {
            backwardThruster.localScale = new Vector3(backwardThrusterMaxScale.x, backwardThrusterMaxScale.y, 1) * backwardInput;
        }
        
        foreach (var clockwiseThruster in clockwiseThrusters)
        {
            clockwiseThruster.localScale = new Vector3(clockwiseThrustersMaxScale.x, clockwiseThrustersMaxScale.y, 1) * leftInput;
        }
        
        foreach (var counterClockwiseThruster in counterClockwiseThrusters)
        {
            counterClockwiseThruster.localScale = new Vector3(counterClockwiseThrustersMaxScale.x, counterClockwiseThrustersMaxScale.y, 1) * rightInput;
        }
    }
}
