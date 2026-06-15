using UnityEngine;
using System.Collections.Generic;
using Anvil;

namespace Anvil
{
    public abstract class BasePool<T>
    {
        protected Stack<T> _pool = new();

        public int Count => _pool.Count;

        public abstract T Get();

        public virtual void Return(T item)
        {
            if (item == null)
            {
                LegacyLog.Warning("Return null item to pool!");
                return;
            }

            //Log.Debug($"Return {item} to pool {Helper.GetClassName<T>()}: count={_pool.Count + 1}");
            _pool.Push(item);
        }

        public virtual void Clear()
        {
            _pool.Clear();
        }

#if DEBUG_MODE
        // public virtual void OnGUI(string label)
        // {
        //     GUIHelper.LayoutLeft(() =>
        //     {
        //         int count = _pool.Count;
        //         GUIHelper.Label($"{label}'s pool: count={count}");
        //         if (GUIHelper.Button("Clear", () => count > 0))
        //         {
        //             Clear();
        //         }
        //     });
        // }

        public override string ToString()
        {
            return $"[{Helper.GetClassName<T>()}] poolCount={_pool.Count}";
        }
#endif
    }
}