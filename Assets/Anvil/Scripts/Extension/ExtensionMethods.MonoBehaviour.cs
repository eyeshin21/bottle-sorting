#region
using System;
using System.Collections;
using UnityEngine;
#endregion

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static void InvokeCoroutine(this MonoBehaviour mb,Action f,float delay)
        {
            mb.StartCoroutine(InvokeRoutine(f,delay));
        }

        private static IEnumerator InvokeRoutine(Action f,float delay)
        {
            yield return new WaitForSeconds(delay);
            f();
        }
    }
}