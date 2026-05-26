using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        #region Vector2
        public static void AddOffset(this List<Vector2> list, float offsetX, float offsetY)
        {
            if (list != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    item.x += offsetX;
                    item.y += offsetY;
                    list[i] = item;
                }
            }
        }

        public static List<Vector2> GetList(this List<Vector2> list, float offsetX, float offsetY)
        {
            if (list != null)
            {
                int count = list.Count;
                var list2 = new List<Vector2>(count);
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    item.x += offsetX;
                    item.y += offsetY;
                    list2.Add(item);
                }
                return list2;
            }
            return null;
        }

        public static List<Vector2> GetList(this List<Vector2> list, float offsetX, float offsetY, float scale)
        {
            if (list != null)
            {
                int count = list.Count;
                var list2 = new List<Vector2>(count);
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    item.x = (item.x + offsetX) * scale;
                    item.y = (item.y + offsetY) * scale;
                    list2.Add(item);
                }
                return list2;
            }
            return null;
        }
        #endregion

        #region Vector3
        public static void AddOffset(this List<Vector3> list, float offsetX, float offsetY)
        {
            if (list != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    item.x += offsetX;
                    item.y += offsetY;
                    list[i] = item;
                }
            }
        }

        public static List<Vector3> GetList(this List<Vector3> list, float offsetX, float offsetY)
        {
            if (list != null)
            {
                int count = list.Count;
                var list2 = new List<Vector3>(count);
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    item.x += offsetX;
                    item.y += offsetY;
                    list2.Add(item);
                }
                return list2;
            }
            return null;
        }
        #endregion
    }
}