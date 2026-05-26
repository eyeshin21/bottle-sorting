using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
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
            Assert.NotContains(_pool, item);
            _pool.Push(item);
        }

        public virtual void Return(T[,] a)
        {
            if (a == null) return;

            a.GetSize(out int rowCount, out int columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    Return(a[row, column]);
                }
            }
        }

        /// <summary>
        /// Returns items to pool and clear list.
        /// </summary>
        public virtual void Return(List<T> items)
        {
            int count = items.GetCount();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Return(items[i]);
                }
                items.Clear();
            }
        }

        public virtual void Clear()
        {
            _pool.Clear();
        }

#if DEBUG_MODE
        public virtual void OnGUI(string label)
        {
            GUIHelper.LayoutLeft(() =>
            {
                int count = _pool.Count;
                GUIHelper.Label($"{label}'s pool: count={count}");
                if (GUIHelper.Button("Clear", () => count > 0))
                {
                    Clear();
                }
            });
        }

        public override string ToString()
        {
            return $"[{Helper.GetClassName<T>()}] poolCount={_pool.Count}";
        }
#endif
    }
}