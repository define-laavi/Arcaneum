using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arcadeum.Asteroids.Core
{
    public class ShipTrailBehaviour : MonoBehaviour
    {
        [SerializeField] private List<TrailRenderer> _trails;

        private bool _lastEmit;

        /// <summary>Updates the emission state based on the position of spaceship - disables if it is outside of the play area to avoid visual glitches</summary>
        public void UpdateTrails(Vector2 position)
        {
            bool emits = World.IsInPlayArea(position);
            if(!_lastEmit && emits) //Rising edge
            {
                SetEmission(true);
                _lastEmit = true;
            }
            else if(_lastEmit && !emits) //Falling edge
            {
                SetEmission(false);
                _lastEmit= false;
            }
        }
        private void SetEmission(bool emits)
        {
            TrailRenderer trail;
            for (int i = _trails.Count - 1; i >= 0; i--)
            {
                trail = _trails[i];
                trail.emitting = emits;
                if (!emits) trail.Clear();
            }
        }
    }
}
