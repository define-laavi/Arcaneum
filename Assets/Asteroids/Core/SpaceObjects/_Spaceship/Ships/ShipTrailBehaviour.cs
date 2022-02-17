using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public class ShipTrailBehaviour : MonoBehaviour
    {
        public List<TrailRenderer> trails;

        private bool _lastEmit;

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
            for (int i = trails.Count - 1; i >= 0; i--)
            {
                trail = trails[i];
                trail.emitting = emits;
                if (!emits) trail.Clear();
            }
        }
    }
}
