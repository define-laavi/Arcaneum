using System.Collections;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    public abstract class BulletBehaviour : SpaceObjectBehaviour
    {
        public bool CanHitPlayer => _leftPlayer;
        protected bool _leftPlayer = false;

        protected override void OnEnabled()
        {
            _leftPlayer = false;
            StartCoroutine(WaitToHit());
        }

        protected IEnumerator WaitToHit()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            _leftPlayer = true;
        }

    }
}