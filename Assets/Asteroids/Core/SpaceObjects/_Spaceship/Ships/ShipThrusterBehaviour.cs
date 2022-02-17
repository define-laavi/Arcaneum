using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipThrusterBehaviour : MonoBehaviour
{
    [Header("Movement Thrusters")]
    [SerializeField] private List<Transform> _forwardThrusters;
    [SerializeField] private Vector2 _forwardThrustersMaxScale;
    [SerializeField] private List<Transform> _backwardThrusters;
    [SerializeField] private Vector2 _backwardThrusterMaxScale;
    
    [Header("Rotation Thrusters")]
    [SerializeField] private List<Transform> _clockwiseThrusters;
    [SerializeField] private Vector2 _clockwiseThrustersMaxScale;
    [SerializeField] private List<Transform> _counterClockwiseThrusters;
    [SerializeField] private Vector2 _counterClockwiseThrustersMaxScale;

    /// <summary>Updates the thrusters based on user input</summary>
    public void UpdateThrust(Vector2 playerInput)
    {
        var forwardInput = Mathf.Clamp01(playerInput.y);
        var backwardInput = Mathf.Clamp01(-playerInput.y);
        var leftInput = Mathf.Clamp01(playerInput.x);
        var rightInput = Mathf.Clamp01(-playerInput.x);
        
        foreach (var forwardThruster in _forwardThrusters)
        {
            forwardThruster.localScale = new Vector3(_forwardThrustersMaxScale.x, _forwardThrustersMaxScale.y, 1) * forwardInput;
        }
        
        foreach (var backwardThruster in _backwardThrusters)
        {
            backwardThruster.localScale = new Vector3(_backwardThrusterMaxScale.x, _backwardThrusterMaxScale.y, 1) * backwardInput;
        }
        
        foreach (var clockwiseThruster in _clockwiseThrusters)
        {
            clockwiseThruster.localScale = new Vector3(_clockwiseThrustersMaxScale.x, _clockwiseThrustersMaxScale.y, 1) * leftInput;
        }
        
        foreach (var counterClockwiseThruster in _counterClockwiseThrusters)
        {
            counterClockwiseThruster.localScale = new Vector3(_counterClockwiseThrustersMaxScale.x, _counterClockwiseThrustersMaxScale.y, 1) * rightInput;
        }
    }
}
