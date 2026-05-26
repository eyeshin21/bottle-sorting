using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void AddOrHideGameObjects<T>(List<T> components, int newCount, AddCallback<T> addCallback) where T : Component
        {
            int count = components.Count;
            if (count < newCount)
            {
                int addCount = newCount - count;
                for (int i = 0; i < addCount; i++)
                {
                    components.Add(addCallback());
                }
            }
            else if (count > newCount)
            {
                int hideCount = count - newCount;
                int index = count - 1;
                for (int i = 0; i < hideCount; i++)
                {
                    components[index--].gameObject.SetActive(false);
                }
            }
        }
    }
}