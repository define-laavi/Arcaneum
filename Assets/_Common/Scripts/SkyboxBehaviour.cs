using UnityEngine;

namespace Arcadeum.Common
{
    public class SkyboxBehaviour : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private float _speed;

        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        private float currentRot = 0;

        void Update()
        {
            currentRot = (currentRot + _speed * Time.deltaTime) % 360;
            _material?.SetFloat(Rotation, currentRot);
        }
    }
}