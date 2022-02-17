using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Common
{
    [CreateAssetMenu(menuName = "Scriptables/SharedInt", fileName = "New SharedInt")]
    public class SharedInt : ScriptableObject
    {
        [SerializeField] private int _value;

        public int Value => _value;
    }
}