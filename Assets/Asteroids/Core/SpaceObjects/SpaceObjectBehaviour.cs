using Arcadeum.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    [RequireComponent(typeof(Rigidbody2D)), DisallowMultipleComponent]
    public abstract class SpaceObjectBehaviour : MonoBehaviour
    {
        public static List<SpaceObjectBehaviour> allSpaceObjects = new List<SpaceObjectBehaviour>();

        public string spaceObjectTag;

        private void Start() { OnStart(); OnEnabled(); }
        private void OnEnable() { allSpaceObjects.Add(this); OnEnabled(); }
        private void Update()
        {
            OnUpdate();
        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<SpaceObjectBehaviour>(out var spaceObjectBehaviour))
                OnHit(col, spaceObjectBehaviour);
        }
        private void OnDisable()
        {
            allSpaceObjects.Remove(this);
        }

        protected virtual void OnStart() { }
        protected virtual void OnEnabled() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnHit(Collision2D col, SpaceObjectBehaviour colBehaviour) { }
        public virtual void OnDeath() { Pool.Despawn(this.gameObject); }
    }
}
