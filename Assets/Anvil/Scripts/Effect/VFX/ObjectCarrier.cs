using System;
using System.Numerics;
using Anvil.Legacy;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Anvil
{
    public class ObjectCarrier : MonoBehaviour
    {
        [SerializeField]protected Transform _parentTF;
        public Transform Parent => _parentTF;
    }
}
