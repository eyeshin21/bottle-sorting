using System;
using System.Collections.Generic;

namespace Anvil
{
    public class ShareController<T> where T : new()
    {
        private SharePool<T> _pool;
        private T _value;

        public T Value => _value;
        public bool Sharing { get; set; }

        public ShareController(SharePool<T> pool, T value)
        {
            _pool = pool;
            _value = value;
        }

        public void Return()
        {
            _pool.Return(this);
        }
    }

    public class SharePool<T> where T : new()
    {
        private Stack<ShareController<T>> _stack = new Stack<ShareController<T>>();
        private Action<T> _returnCallback;

        public SharePool(Action<T> returnCallback)
        {
            _returnCallback = returnCallback;
        }

        public ShareController<T> Get()
        {
            ShareController<T> controller;
            if (_stack.Count > 0)
            {
                controller = _stack.Pop();
            }
            else
            {
                controller = new ShareController<T>(this, new T());
            }
            controller.Sharing = true;

            return controller;
        }

        public void Return(ShareController<T> controller)
        {
            controller.Sharing = false;

            _returnCallback(controller.Value);
            _stack.Push(controller);
        }
    }

    public static class Shared
    {
        static SharePool<Dictionary> _dictPool;
        public static ShareController<Dictionary> Dictionary
        {
            get
            {
                if (_dictPool == null)
                {
                    _dictPool = new SharePool<Dictionary>(dict => dict.Clear());
                }

                var controller = _dictPool.Get();
                return controller;
            }
        }

        public static string Serialize(Action<Dictionary> callback)
        {
            var sharedDict = Dictionary;
            var dict = sharedDict.Value;
            callback?.Invoke(dict);
            var json = dict.Serialize();
            sharedDict.Return();
            return json;
        }

        public static void Deserialize(string json, Action<Dictionary> callback, bool isSimpleJson = false)
        {
            var sharedDict = Dictionary;
            var dict = sharedDict.Value.Deserialize(json, isSimpleJson);
            callback?.Invoke(dict);
            sharedDict.Return();
        }
    }
}