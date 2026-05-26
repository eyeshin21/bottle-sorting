using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class Pools
    {
        static Pool<List<bool>> _poolListBool = new();
        public static List<bool> GetListBool()
        {
            var list = _poolListBool.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static List<bool> GetListBool(int count, bool value = false)
        {
            var list = _poolListBool.Get();
            Assert.IsEmpty(list);
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
            return list;
        }
        public static void Return(List<bool> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListBool.Return(list);
            }
        }

        static Pool<List<int>> _poolListInt = new();
        public static List<int> GetListInt()
        {
            var list = _poolListInt.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static List<int> GetListInt(int count, int value = 0)
        {
            var list = _poolListInt.Get();
            Assert.IsEmpty(list);
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
            return list;
        }
        public static List<int> GetListIndices(int count)
        {
            var list = _poolListInt.Get();
            Assert.IsEmpty(list);
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            return list;
        }
        public static void Return(List<int> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListInt.Return(list);
            }
        }

        static Pool<List<float>> _poolListFloat = new();
        public static List<float> GetListFloat()
        {
            var list = _poolListFloat.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static void Return(List<float> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListFloat.Return(list);
            }
        }

        static Pool<List<string>> _poolListString = new();
        public static List<string> GetListString()
        {
            var list = _poolListString.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static void Return(List<string> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListString.Return(list);
            }
        }

        static Pool<List<Vector3>> _poolListVector3 = new();
        public static List<Vector3> GetListVector3()
        {
            var list = _poolListVector3.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static void Return(List<Vector3> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListVector3.Return(list);
            }
        }

        static Pool<List<GameObject>> _poolListGameObject = new();
        public static List<GameObject> GetListGameObject()
        {
            var list = _poolListGameObject.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static void Return(List<GameObject> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListGameObject.Return(list);
            }
        }

        static Pool<List<Transform>> _poolListTransform = new();
        public static List<Transform> GetListTransform()
        {
            var list = _poolListTransform.Get();
            Assert.IsEmpty(list);
            return list;
        }
        public static void Return(List<Transform> list)
        {
            if (list != null)
            {
                list.Clear();
                _poolListTransform.Return(list);
            }
        }

        static Pool<Dictionary> _poolDictionary = new();
        public static Dictionary GetDictionary()
        {
            var dict = _poolDictionary.Get();
            Assert.IsEmpty(dict);
            return dict;
        }
        public static void Return(Dictionary dict)
        {
            if (dict != null)
            {
                dict.Clear();
                _poolDictionary.Return(dict);
            }
        }
    }
}