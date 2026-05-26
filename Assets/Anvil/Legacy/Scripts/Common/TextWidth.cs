using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    /// <summary>
    /// Text + Width
    /// </summary>
    public class TextWidth
    {
        string _text;
        float _width;

        public string Text => _text;
        public float Width => _width;

        public TextWidth()
        {

        }

        public TextWidth(string text, float width)
        {
            _text = text;
            _width = width;
        }

        public void Construct(string text, float width)
        {
            _text = text;
            _width = width;
        }

        #region Pool
        static Pool<TextWidth> _pool;
        static Pool<TextWidth> Pool => _pool ??= new();

        static Pool<List<TextWidth>> _poolList;
        static Pool<List<TextWidth>> PoolList => _poolList ??= new();

        public void ReturnPool()
        {
            Pool.Return(this);
        }

        public static TextWidth Get()
        {
            return Pool.Get();
        }

        public static TextWidth Get(string text, float width)
        {
            var textWidth = Pool.Get();
            textWidth.Construct(text, width);
            return textWidth;
        }

        public static List<TextWidth> GetList()
        {
            return PoolList.Get();
        }

        public static void Return(List<TextWidth> list)
        {
            if (list != null)
            {
                Pool.Return(list);
                PoolList.Return(list);
            }
        }
        #endregion
    }
}