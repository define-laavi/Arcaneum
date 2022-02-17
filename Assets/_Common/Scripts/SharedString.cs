using UnityEngine;

namespace Arcadeum.Common
{
    [CreateAssetMenu(menuName = "Scriptables/SharedString", fileName = "New SharedString")]
    public class SharedString : ScriptableObject
    {
        [SerializeField] private string _value;
        public string Value => _value;
    }
}