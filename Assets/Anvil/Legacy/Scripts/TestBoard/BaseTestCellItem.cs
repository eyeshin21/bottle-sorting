using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseTestCellItem<TBoard, TCell, TItem> : BaseTestCell<TBoard, TCell>
        where TBoard : ITestBoard<TBoard, TCell>
        where TCell : ITestCell<TBoard, TCell>
        where TItem : class, ITestItem<TCell>
    {
        protected TItem[] _items;
        protected TItem[] Items => _items ??= NewItems();

        protected abstract TCell _cell { get; }
        protected abstract TItem[] NewItems();

        public TItem TopItem
        {
            get
            {
                if (_items != null)
                {
                    for (int i = _items.Length - 1; i >= 0; i--)
                    {
                        var item = _items[i];
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
                return null;
            }
        }

        public bool Empty => !HasItem();

        public int ItemCount
        {
            get
            {
                if (_items != null)
                {
                    int count = 0;
                    for (int i = _items.Length - 1; i >= 0; i--)
                    {
                        if (_items[i] != null)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                return 0;
            }
        }

        #region Helper
        protected bool HasItemFromLayer(int layerIndex)
        {
            if (_items == null) return false;

            int layerCount = _items.Length;
            if (layerIndex < 0 || layerIndex >= layerCount)
            {
                LegacyLog.Warning($"Index out of [0,{layerCount - 1}]");
                return false;
            }

            for (int i = layerIndex; i < layerCount; i++)
            {
                if (_items[i] != null)
                {
                    return true;
                }
            }

            return false;
        }

        protected TItem GetItem(int index)
        {
            if (_items == null) return null;

            if (index < 0 || index >= _items.Length)
            {
                LegacyLog.Warning($"Index out of [0,{_items.Length - 1}]");
                return null;
            }

            return _items[index];
        }

        public TItem GetItemTopDown(AcceptFunc<TItem> acceptFunc)
        {
            if (_items != null)
            {
                for (int i = _items.Length - 1; i >= 0; i--)
                {
                    var item = _items[i];
                    if (item != null && acceptFunc(item))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        protected bool CanAddItem(int index)
        {
            if (_items == null) return true;

            if (index < 0 || index >= _items.Length)
            {
                LegacyLog.Warning($"Index out of [0,{_items.Length - 1}]");
                return false;
            }

            return _items[index] == null;
        }

        protected bool AddItem(TItem item, int index)
        {
            Assert.IsNull(item.Cell);

            var items = Items;
            if (items == null)
            {
                LegacyLog.Warning("Items is null!");
                return false;
            }

            if (index < 0 || index >= items.Length)
            {
                LegacyLog.Warning($"{this}: Can't add item {item} (Invalid index {index})!");
                return false;
            }

            var currentItem = items[index];
            if (currentItem != null)
            {
                LegacyLog.Warning($"{this}: Can't add item {item} (Overlap item {currentItem})!");
                return false;
            }

            item.Cell = _cell;
            items[index] = item;
            return true;
        }

        protected void RemoveItem(TItem item, int index)
        {
            Assert.IsEquals(item.Cell, _cell);
            if (_items != null)
            {
                if (index >= 0 && index < _items.Length)
                {
                    var currentItem = _items[index];
                    if (currentItem == null || currentItem == item)
                    {
                        item.Cell = default;
                        _items[index] = null;
                    }
                    else
                    {
                        LegacyLog.Warning($"{this}: Can't remove item {item} (currentItem={currentItem})!");
                    }
                }
                else
                {
                    LegacyLog.Warning($"{this}: Can't remove item {item} (Invalid index {index})!");
                }
            }
            else
            {
                LegacyLog.Warning($"{this}: Items is null!");
            }
        }

        public bool HasItem()
        {
            if (_items != null)
            {
                for (int i = _items.Length - 1; i >= 0; i--)
                {
                    if (_items[i] != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool HasItem(int index)
        {
            if (_items != null)
            {
                if (index >= 0 && index < _items.Length)
                {
                    return _items[index] != null;
                }
            }
            return false;
        }

        public void ForEachItem(Callback<TItem> callback)
        {
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    var item = _items[i];
                    if (item != null)
                    {
                        callback?.Invoke(item);
                    }
                }
            }
        }

        public void Clear()
        {
            if (_items != null)
            {
                for (int i = _items.Length - 1; i >= 0; i--)
                {
                    var item = _items[i];
                    if (item != null)
                    {
                        item.Cell = default;
                        _items[i] = null;
                    }
                }
            }
        }

        public void Clear(Callback<TItem> callback)
        {
            if (_items != null)
            {
                for (int i = _items.Length - 1; i >= 0; i--)
                {
                    var item = _items[i];
                    if (item != null)
                    {
                        item.Cell = default;
                        _items[i] = null;
                        callback?.Invoke(item);
                    }
                }
            }
        }
        #endregion

#if DEBUG_MODE
        public void LogItems()
        {
            int count = _items.GetCount();
            for (int i = 0; i < count; i++)
            {
                var item = _items[i];
                if (item != null)
                {
                    var s = Helper.CreateString(sb =>
                    {
                        sb.Append($"{this}: {item}");
                        for (int j = i + 1; j < count; j++)
                        {
                            sb.Append($", {_items[j]}");
                        }
                    });
                   LegacyLog.Debug(s);
                    return;
                }
            }
           LegacyLog.Debug(this);
        }
#endif
    }
}