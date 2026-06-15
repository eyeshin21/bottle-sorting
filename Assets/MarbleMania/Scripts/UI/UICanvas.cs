using System;
using Anvil;
using UnityEngine;

namespace MatchThree
{
    public class UICanvas : MonoBehaviour
    {
        private void Awake()
        {
            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null) return;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Context.MainCamera;
            }
        }
    }
}