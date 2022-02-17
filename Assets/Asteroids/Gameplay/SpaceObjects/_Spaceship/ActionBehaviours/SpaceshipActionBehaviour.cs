using UnityEngine;

namespace Arcadeum.Asteroids.Core
{
    [DisallowMultipleComponent]
    public abstract class SpaceshipActionBehaviour : MonoBehaviour
    {
        public abstract void Act(Spaceship spaceship);
    }

    [System.Serializable]
    public class SpaceshipAction
    {
        [SerializeField] private string _name;
        [SerializeField] private SpaceshipActionBehaviour _behaviour;
        [SerializeField] private KeyCode _key;

        public string Name => _name;
        public SpaceshipActionBehaviour Behaviour => _behaviour;
        public KeyCode Key => _key;
    }
}