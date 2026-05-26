using System;
using UnityEngine;

namespace Anvil
{
    public interface IGlobalUIElement
    {
        public bool IsVisible { get; }
        public GameObject gameObject { get; }
        public string idString { get; set; }
        public int PreferedIndex { get; set; }
        public void Show(Action callback = null);
        public void Hide(Action callback = null);
        public void TryPlayAnimation(string animationName, Action callback = null);
        // public void Hide(float timeOut, Action callback = null);
        public void SetSortingOrder(int order);
    }
}
