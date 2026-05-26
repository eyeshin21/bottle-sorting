using UnityEngine;
using System.Collections;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return Manager.Instance.StartCoroutine(routine);
        }
    }
}