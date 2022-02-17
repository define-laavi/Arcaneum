using Arcadeum.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    [RequireComponent(typeof(Rigidbody2D)), DisallowMultipleComponent]
    public abstract class SpaceObjectBehaviour : MonoBehaviour
    {
        //Helps track all the space objects for the blast destruction. It is still better than calling FindObjectsOfType<SpaceObjectBehaviour>
        public static List<SpaceObjectBehaviour> allSpaceObjects = new List<SpaceObjectBehaviour>();

        private void Start() 
        { 
            OnStart(); 
            OnEnabled(); 
        }
        private void OnEnable() 
        { 
            allSpaceObjects.Add(this); 
            OnEnabled(); 
        }
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

        /// <summary> Handles the initialization of the object. Is invoked one time during session </summary>
        protected virtual void OnStart() { }

        /// <summary> Happens every time the object is spawned from the pool. It's best to reset it's parameters here. </summary>
        protected virtual void OnEnabled() { }

        /// <summary> Happens once every game tick. </summary>
        protected virtual void OnUpdate() { }

        /// <summary> Happens when this space object collides with another one - colliding with other object does not call this function!</summary>
        protected virtual void OnHit(Collision2D col, SpaceObjectBehaviour colBehaviour) { }

        /// <summary> Handles the general despawning of the object. </summary>
        public virtual void OnDeath() { Pool.Despawn(this.gameObject); }
    }
}
