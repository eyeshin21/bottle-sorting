using System;
using UnityEngine;

namespace Anvil.Tools
{
    public class TransformLogger : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("transform at awake:");
            LogTransform();
        }

        private void Update()
        {
            LogTransform();
        }

        private void LogTransform()
        {
            Debug.Log($"transform {transform.position}, rotation {transform.rotation}, scale {transform.localScale}");
        }
    }
}