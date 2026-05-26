using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsLastItem<T>(this List<T> list, T item)
        {
            return list != null && list.Count > 0 && list[list.Count - 1].Equals(item);
        }

        public static int GetCount<T>(this List<T> list)
        {
            return list != null ? list.Count : 0;
        }

        public static T GetFirst<T>(this List<T> list)
        {
            return list != null && list.Count > 0 ? list[0] : default;
        }

        public static T GetLast<T>(this List<T> list)
        {
            return list != null && list.Count > 0 ? list[list.Count - 1] : default;
        }

        public static T GetRandom<T>(this List<T> list)
        {
            return list != null && list.Count > 0 ? list[Helper.GetRandomIndex(list.Count)] : default;
        }

        /// <summary>
        /// Returns default value if out of range.
        /// </summary>
        public static T TryGet<T>(this List<T> list, int index)
        {
            if (list == null || index < 0) return default;

            int count = list.Count;
            return index < count ? list[index] : default;
        }

        public static T ForceGet<T>(this List<T> list, int index)
        {
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return index <= 0 ? list[0] : list[Mathf.Min(index, count - 1)];
                }
            }
            return default;
        }

        public static T RemoveFirst<T>(this List<T> list)
        {
            int count = GetCount(list);
            if (count > 0)
            {
                T item = list[0];
                list.RemoveAt(0);
                return item;
            }
            return default;
        }

        public static T RemoveLast<T>(this List<T> list)
        {
            int count = GetCount(list);
            if (count > 0)
            {
                T item = list[count - 1];
                list.RemoveAt(count - 1);
                return item;
            }
            return default;
        }

        public static bool RemoveFirst<T>(this List<T> list, out T item)
        {
            int count = GetCount(list);
            if (count > 0)
            {
                item = list[0];
                list.RemoveAt(0);
                return true;
            }
            item = default;
            return false;
        }

        public static bool RemoveLast<T>(this List<T> list, out T item)
        {
            int count = GetCount(list);
            if (count > 0)
            {
                item = list[count - 1];
                list.RemoveAt(count - 1);
                return true;
            }
            item = default;
            return false;
        }

        public static T Get<T>(this List<T> list, Func<T, bool> acceptFunc)
        {
            if (list != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = list[i];
                    if (acceptFunc(item))
                    {
                        return item;
                    }
                }
            }
            return default;
        }

        public static TValue GetFirstValue<T, TValue>(this List<T> list, Func<T, TValue> func)
        {
            return list != null && list.Count > 0 ? func(list[0]) : default;
        }

        public static TValue GetLastValue<T, TValue>(this List<T> list, Func<T, TValue> func)
        {
            return list != null && list.Count > 0 ? func(list[list.Count - 1]) : default;
        }

        public static List<T> GetCopy<T>(this List<T> list)
        {
            return list != null ? new List<T>(list) : default;
        }

        public static void AddIfNotContains<T>(this List<T> list, T item)
        {
            if (list != null && !list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void AddRandom<T>(this List<T> list, T item)
        {
            if (list != null)
            {
                int count = list.Count;
                if (count == 0)
                {
                    list.Add(item);
                }
                else
                {
                    int index = Helper.GetRandomIndex(count + 1);
                    if (index < count)
                    {
                        list.Insert(index, item);
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Adds if not contain the specified item.
        /// </summary>
        public static void CheckAdd<T>(this List<T> list, T item)
        {
            if (list != null)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }

        public static void CheckAdd<T>(this List<T> list, T item1, T item2)
        {
            if (list != null)
            {
                if (!list.Contains(item1))
                {
                    list.Add(item1);
                }
                if (!list.Contains(item2))
                {
                    list.Add(item2);
                }
            }
        }

        public static void AddItems<T, U>(this List<T> list, List<U> list2, Func<U, bool> acceptFunc, Func<U, T> convertFunc)
        {
            if (list2 != null)
            {
                int count = list2.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = list2[i];
                    if (acceptFunc(item))
                    {
                        list.Add(convertFunc(item));
                    }
                }
            }
        }

        public static void CheckRemove<T>(this List<T> list, T item)
        {
            if (list != null)
            {
                list.Remove(item);
            }
        }

        public static void Remove<T>(this List<T> list, List<T> subList)
        {
            if (list != null && subList != null)
            {
                int count = subList.Count;
                for (int i = 0; i < count; i++)
                {
                    list.Remove(subList[i]);
                }
            }
        }

        public static void AddChildren(this List<Transform> list, Transform transform)
        {
            if (transform != null)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    list.Add(transform.GetChild(i));
                }
            }
        }

        public static void AddChildren(this List<Transform> list, GameObject gameObject)
        {
            if (gameObject != null)
            {
                var transform = gameObject.transform;
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    list.Add(transform.GetChild(i));
                }
            }
        }

        public static void AddChildren(this List<GameObject> list, Transform transform)
        {
            if (transform != null)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    list.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        public static void AddChildren(this List<GameObject> list, GameObject gameObject)
        {
            if (gameObject != null)
            {
                var transform = gameObject.transform;
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    list.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        public static void Set<T, U>(this List<T> list, U[] array, Callback<T, U> callback)
        {
            if (list != null)
            {
                int newCount = array.GetLength();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount);
                    for (int i = 0; i < newCount; i++)
                    {
                        callback(list[i], array[i]);
                    }
                }
            }
        }

        public static void Set<T>(this List<T> list, List<T> list2)
        {
            if (list != null)
            {
                int newCount = list2.GetCount();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount, null);
                    for (int i = 0; i < newCount; i++)
                    {
                        list[i] = list2[i];
                    }
                }
            }
        }

        public static void Set<T>(this List<T> list, List<T> list2, Callback<T, T> callback)
        {
            if (list != null)
            {
                int newCount = list2.GetCount();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount);
                    for (int i = 0; i < newCount; i++)
                    {
                        callback(list[i], list2[i]);
                    }
                }
            }
        }

        public static void Set<T, U>(this List<T> list, List<U> list2, Callback<T, U> callback)
        {
            if (list != null)
            {
                int newCount = list2.GetCount();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount);
                    for (int i = 0; i < newCount; i++)
                    {
                        callback(list[i], list2[i]);
                    }
                }
            }
        }

        public static void Set<T, U>(this List<T> list, List<U> list2, Func<T> newFunc, Callback<T, U> callback)
        {
            if (list != null)
            {
                int newCount = list2.GetCount();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount, newFunc);
                    for (int i = 0; i < newCount; i++)
                    {
                        callback(list[i], list2[i]);
                    }
                }
            }
        }

        public static void Set<T, U>(this List<T> list, List<U> list2, Func<U, T> parseFunc)
        {
            if (list != null)
            {
                int newCount = list2.GetCount();
                if (newCount == 0)
                {
                    list.Clear();
                }
                else
                {
                    Helper.SetCount(list, newCount, null);
                    for (int i = 0; i < newCount; i++)
                    {
                        list[i] = parseFunc(list2[i]);
                    }
                }
            }
        }

        public static void Swap<T>(this List<T> list, int count = -1)
        {
            if (count < 0) count = list.GetCount();
            if (count < 2) return;

            T tmp;
            if (count == 2)
            {
                if (Helper.GetRandomBool())
                {
                    tmp = list[0];
                    list[0] = list[1];
                    list[1] = tmp;
                }
                return;
            }

            for (int i = 0; i < count; i++)
            {
                int j = Random.Range(0, count);
                if (i != j)
                {
                    tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;
                }
            }
        }

        public static void SwapDiff(this List<int> list)
        {
            int count = list.GetCount();
            if (count < 2) return;

            if (count == 2)
            {
                int tmp = list[0];
                list[0] = list[1];
                list[1] = tmp;
                return;
            }

            if (count == 3)
            {
                int tmp = list[0];
                if (Helper.GetRandomBool())
                {
                    list[0] = list[1];
                    list[1] = list[2];
                    list[2] = tmp;
                }
                else
                {
                    list[0] = list[2];
                    list[2] = list[1];
                    list[1] = tmp;
                }
                return;
            }

            var visited = Pools.GetListBool(count);
            var list2 = Pools.GetListInt(count);
            var indices = Pools.GetListIndices(count);
            indices.Swap();

            for (int i = 0; i < count; i++)
            {
                int index = indices[i];
                int item = list[index];
                bool found = false;
                for (int j = 0; j < count; j++)
                {
                    if (j != index && !visited[j])
                    {
                        list2[j] = item;
                        visited[j] = true;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (visited[j])
                        {
                            Assert.IsNotEquals(j, index);
                            list2[index] = list2[j];
                            list2[j] = item;
                            visited[index] = true;
                            found = true;
                            break;
                        }
                    }
                    Assert.IsTrue(found);
                }
            }

            //#if UNITY_EDITOR
            //            for (int i = 0; i < count; i++)
            //            {
            //                if (list[i] == list2[i])
            //                {
            //                   LegacyLog.Warning(list.ToString2());
            //                   LegacyLog.Warning(list2.ToString2());
            //                    break;
            //                }
            //            }
            //#endif

            // Copy
            for (int i = 0; i < count; i++)
            {
                Assert.IsNotEquals(list[i], list2[i]);
                list[i] = list2[i];
            }

            Pools.Return(visited);
            Pools.Return(list2);
            Pools.Return(indices);
        }

        public static void SetMinCount<T>(this List<T> list, int minCount)
        {
            int addCount = minCount - list.Count;
            if (addCount > 0)
            {
                for (int i = 0; i < addCount; i++)
                {
                    list.Add(default);
                }
            }
        }

        public static void SetIndices(this List<int> list, int count)
        {
            int addCount = count - list.Count;
            if (addCount > 0)
            {
                for (int i = 0; i < addCount; i++)
                {
                    list.Add(0);
                }
            }

            for (int i = 0; i < count; i++)
            {
                list[i] = i;
            }
        }

        public static void SetRandomIndices(this List<int> list, int count)
        {
            SetIndices(list, count);
            list.Swap(count);
        }

        /// <summary>
        /// Null is not equals empty.
        /// </summary>
        public static bool IsEquals<T>(this List<T> list1, List<T> list2)
        {
            if (list1 == null) return list2 == null;
            if (list2 == null) return false;

            int count1 = list1.Count;
            int count2 = list2.Count;
            if (count1 != count2) return false;

            for (int i = 0; i < count1; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Null is not equals empty.
        /// </summary>
        public static bool IsEquals<T>(this List<T> list1, List<T> list2, Func<T, T, bool> compareFunc)
        {
            if (list1 == null) return list2 == null;
            if (list2 == null) return false;

            int count1 = list1.Count;
            int count2 = list2.Count;
            if (count1 != count2) return false;

            for (int i = 0; i < count1; i++)
            {
                if (!compareFunc(list1[i], list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains<T>(this List<T> list, Func<T, bool> containFunc)
        {
            if (list != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (containFunc(list[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// compareFunc(item, list[i])
        /// </summary>
        public static void CheckInsert<T>(this List<T> list, T item, Func<T, T, int> compareFunc)
        {
            if (compareFunc != null)
            {
                Insert(list, item, compareFunc);
            }
            else
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// compareFunc(item, list[i])
        /// </summary>
        public static void Insert<T>(this List<T> list, T item, Func<T, T, int> compareFunc)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(item);
            }
            else if (compareFunc(item, list[0]) < 0)
            {
                list.Insert(0, item);
            }
            else if (compareFunc(item, list[count - 1]) >= 0)
            {
                list.Add(item);
            }
            else
            {
                for (int i = 1; i < count; i++)
                {
                    if (compareFunc(item, list[i]) < 0)
                    {
                        list.Insert(i, item);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insert 1 to {0,1,1,2} => {0,1,1,[1],2}
        /// </summary>
        public static void InsertAsc(this List<int> list, int value)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(value);
            }
            else if (value < list[0])
            {
                list.Insert(0, value);
            }
            else if (value >= list[count - 1])
            {
                list.Add(value);
            }
            else
            {
                for (int i = 1; i < count; i++)
                {
                    if (value < list[i])
                    {
                        list.Insert(i, value);
                        break;
                    }
                }
            }
        }

        public static void InsertAsc(this List<float> list, float value)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(value);
            }
            else if (value < list[0])
            {
                list.Insert(0, value);
            }
            else if (value >= list[count - 1])
            {
                list.Add(value);
            }
            else
            {
                for (int i = 1; i < count; i++)
                {
                    if (value < list[i])
                    {
                        list.Insert(i, value);
                        break;
                    }
                }
            }
        }

        public static void InsertAsc(this List<string> list, string value)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(value);
            }
            else if (value.CompareTo(list[0]) < 0)
            {
                list.Insert(0, value);
            }
            else if (value.CompareTo(list[count - 1]) >= 0)
            {
                list.Add(value);
            }
            else
            {
                for (int i = 1; i < count; i++)
                {
                    if (value.CompareTo(list[i]) < 0)
                    {
                        list.Insert(i, value);
                        break;
                    }
                }
            }
        }

        public static void InsertAsc<T>(this List<T> list, T item, Func<T, int> valueFunc)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(item);
            }
            else
            {
                int value = valueFunc(item);
                if (value < valueFunc(list[0]))
                {
                    list.Insert(0, item);
                }
                else if (value >= valueFunc(list[count - 1]))
                {
                    list.Add(item);
                }
                else
                {
                    for (int i = 1; i < count; i++)
                    {
                        if (value < valueFunc(list[i]))
                        {
                            list.Insert(i, item);
                            break;
                        }
                    }
                }
            }
        }

        public static void InsertDesc<T>(this List<T> list, T item, Func<T, int> valueFunc)
        {
            int count = list.Count;
            if (count == 0)
            {
                list.Add(item);
            }
            else
            {
                int value = valueFunc(item);
                if (value > valueFunc(list[0]))
                {
                    list.Insert(0, item);
                }
                else if (value <= valueFunc(list[count - 1]))
                {
                    list.Add(item);
                }
                else
                {
                    for (int i = 1; i < count; i++)
                    {
                        if (value > valueFunc(list[i]))
                        {
                            list.Insert(i, item);
                            break;
                        }
                    }
                }
            }
        }

        public static void SetStartIndex(this List<Vector3> list, int startIndex)
        {
            int count = list.GetCount();
            if (count == 0) return;

            var tmp = new Vector3[startIndex];
            for (int i = 0; i < startIndex; i++)
            {
                tmp[i] = list[i];
            }

            int index = 0;
            for (int i = startIndex; i < count; i++)
            {
                list[index++] = list[i];
            }

            for (int i = 0; i < startIndex; i++)
            {
                list[index++] = tmp[i];
            }
        }

        static readonly float AdjustEpsilon = 0.01f;
        public static void AdjustByMinDistance(this List<Vector3> points, float minDistance)
        {
            if (minDistance <= 0) return;

            int count = points.GetCount();
            if (count < 3) return;

            float minSquare = minDistance * minDistance;
            int i = 1;
            do
            {
                var currentPoint = points[i];
                var prevPoint = points[i - 1];
                var nextPoint = points[i + 1];
                float d1Square = prevPoint.GetDistanceSquare(currentPoint);
                float d2Square = currentPoint.GetDistanceSquare(nextPoint);
                if (d1Square < minSquare)
                {
                    if (d2Square < minSquare)
                    {
                        // Remove current point
                        //Log.Warning($"Remove at {i}");
                        points.RemoveAt(i);
                        count--;
                        i--;
                    }
                    else if (d2Square > minSquare)
                    {
                        // Fix current point
                        float fixing = minDistance - Mathf.Sqrt(d1Square);
                        Helper.TryGetBinary(currentPoint, nextPoint, fixing, pos =>
                        {
                            float delta = prevPoint.GetDistance(pos) - minDistance;
                            if (delta < -AdjustEpsilon) return 1;
                            if (delta > AdjustEpsilon) return -1;
                            points[i] = pos;
                            return 0;
                        });
                    }
                }
                else
                {
                    if (d2Square < minSquare)
                    {
                        if (i < count - 2)
                        {
                            // Fix on next point
                        }
                        else
                        {
                            // Fix current point
                            float fixing = minDistance - Mathf.Sqrt(d2Square);
                            Helper.TryGetBinary(currentPoint, prevPoint, fixing, pos =>
                            {
                                float delta = nextPoint.GetDistance(pos) - minDistance;
                                if (delta < -AdjustEpsilon) return 1;
                                if (delta > AdjustEpsilon) return -1;
                                points[i] = pos;
                                return 0;
                            });
                        }
                    }
                }
                i++;
            }
            while (i < count - 1);
        }

        //public static void AdjustByMaxDistance(this List<Vector3> points, float maxDistance, bool closed = false)
        //{
        //    if (maxDistance <= 0) return;

        //    int count = points.GetCount();
        //    if (count < 2) return;

        //    float maxSquare = maxDistance * maxDistance;
        //    var point = points[0];
        //    list.Add(point);
        //    float prevX = point.x;
        //    float prevY = point.y;
        //    int max = closed ? count : count - 1;
        //    for (int i = 1; i <= max; i++)
        //    {
        //        if (i < count)
        //        {
        //            point = points[i];
        //        }
        //        else
        //        {
        //            point = points[0];
        //        }
        //        float deltaX = point.x - prevX;
        //        float deltaY = point.y - prevY;
        //        float distanceSquare = deltaX * deltaX + deltaY * deltaY;
        //        if (distanceSquare > maxSquare)
        //        {
        //            float distance = Mathf.Sqrt(distanceSquare);
        //            float angle = Mathf.Atan2(deltaY, deltaX);
        //            int segmentCount = Mathf.CeilToInt(distance / maxDistance);
        //            float step = distance / segmentCount;
        //            float stepX = step * Mathf.Cos(angle);
        //            float stepY = step * Mathf.Sin(angle);
        //            var midPoint = new Vector3(prevX, prevY);
        //            for (int j = 1; j < segmentCount; j++)
        //            {
        //                midPoint.x += stepX;
        //                midPoint.y += stepY;
        //                list.Add(midPoint);
        //            }
        //        }
        //        if (i < count)
        //        {
        //            list.Add(point);
        //            prevX = point.x;
        //            prevY = point.y;
        //        }
        //    }

        //    return list;
        //}

        /// <summary>
        /// Check null list.
        /// </summary>
        public static void ForEach2<T>(this List<T> list, Callback<T> callback)
        {
            if (list != null && callback != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    callback(list[i]);
                }
            }
        }

        /// <summary>
        /// callback(index, item)
        /// </summary>
        public static void ForEach<T>(this List<T> list, Callback<int, T> callback)
        {
            if (list != null && callback != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    callback(i, list[i]);
                }
            }
        }

        public static List<TResult> ToList<T, TResult>(this List<T> list, Func<T, TResult> convertFunc)
        {
            if (list != null)
            {
                int count = list.Count;
                var list2 = new List<TResult>(count);
                for (int i = 0; i < count; i++)
                {
                    list2.Add(convertFunc(list[i]));
                }
                return list2;
            }
            return null;
        }

        public static void ToList<T, TResult>(this List<T> list, ref List<TResult> list2, Func<T, TResult> convertFunc)
        {
            if (list != null)
            {
                int count = list.Count;
                if (list2 != null)
                {
                    list2.Clear();
                }
                else
                {
                    list2 = new List<TResult>(count);
                }
                for (int i = 0; i < count; i++)
                {
                    list2.Add(convertFunc(list[i]));
                }
            }
            else
            {
                list2?.Clear();
            }
        }

        public static List<Vector3> ToListVector3(this List<Vector2> list)
        {
            if (list != null)
            {
                int count = list.Count;
                var newList = new List<Vector3>(count);
                for (int i = 0; i < count; i++)
                {
                    newList.Add(list[i]);
                }
                return newList;
            }
            return null;
        }

        public static Vector2[] ToArrayVector2(this List<Vector3> list)
        {
            if (list != null)
            {
                int count = list.Count;
                var array = new Vector2[count];
                for (int i = 0; i < count; i++)
                {
                    array[i] = list[i];
                }
                return array;
            }
            return null;
        }

        public static List<Vector2> ToListVector2(this List<Vector3> list)
        {
            if (list != null)
            {
                int count = list.Count;
                var newList = new List<Vector2>(count);
                for (int i = 0; i < count; i++)
                {
                    newList.Add(list[i]);
                }
                return newList;
            }
            return null;
        }

        public static AABB GetAABB(this List<Vector2> list)
        {
            GetAABB(list, out float left, out float top, out float right, out float bottom);
            return new AABB(left, top, right, bottom);
        }

        public static AABB GetAABB(this List<Vector3> list)
        {
            GetAABB(list, out float left, out float top, out float right, out float bottom);
            return new AABB(left, top, right, bottom);
        }

        public static AABB GetAABB(this List<List<Vector2>> list)
        {
            GetAABB(list, out float left, out float top, out float right, out float bottom);
            return new AABB(left, top, right, bottom);
        }

        public static AABB GetAABB(this List<List<Vector3>> list)
        {
            GetAABB(list, out float left, out float top, out float right, out float bottom);
            return new AABB(left, top, right, bottom);
        }

        public static void GetAABB(this List<Vector2> list, out float left, out float top, out float right, out float bottom)
        {
            int count = list.GetCount();
            if (count == 0)
            {
                left = top = right = bottom = 0;
                return;
            }

            var pos = list[0];
            left = pos.x;
            right = left;
            bottom = pos.y;
            top = bottom;
            for (int i = 1; i < count; i++)
            {
                pos = list[i];
                left = Mathf.Min(left, pos.x);
                right = Mathf.Max(right, pos.x);
                bottom = Mathf.Min(bottom, pos.y);
                top = Mathf.Max(top, pos.y);
            }
        }

        public static void GetAABB(this List<Vector3> list, out float left, out float top, out float right, out float bottom)
        {
            int count = list.GetCount();
            if (count == 0)
            {
                left = top = right = bottom = 0;
                return;
            }

            var pos = list[0];
            left = pos.x;
            right = left;
            bottom = pos.y;
            top = bottom;
            for (int i = 1; i < count; i++)
            {
                pos = list[i];
                left = Mathf.Min(left, pos.x);
                right = Mathf.Max(right, pos.x);
                bottom = Mathf.Min(bottom, pos.y);
                top = Mathf.Max(top, pos.y);
            }
        }

        public static void GetAABB(this List<List<Vector2>> list, out float left, out float top, out float right, out float bottom)
        {
            int count = list.GetCount();
            if (count == 0)
            {
                left = top = right = bottom = 0;
                return;
            }

            GetAABB(list[0], out left, out top, out right, out bottom);
            for (int i = 1; i < count; i++)
            {
                GetAABB(list[i], out float left2, out float top2, out float right2, out float bottom2);
                left = Mathf.Min(left, left2);
                right = Mathf.Max(right, right2);
                bottom = Mathf.Min(bottom, bottom2);
                top = Mathf.Max(top, top2);
            }
        }

        public static void GetAABB(this List<List<Vector3>> list, out float left, out float top, out float right, out float bottom)
        {
            int count = list.GetCount();
            if (count == 0)
            {
                left = top = right = bottom = 0;
                return;
            }

            GetAABB(list[0], out left, out top, out right, out bottom);
            for (int i = 1; i < count; i++)
            {
                GetAABB(list[i], out float left2, out float top2, out float right2, out float bottom2);
                left = Mathf.Min(left, left2);
                right = Mathf.Max(right, right2);
                bottom = Mathf.Min(bottom, bottom2);
                top = Mathf.Max(top, top2);
            }
        }

        public static void SetPivotToCenter(this List<Vector3> list)
        {
            int count = list.GetCount();
            if (count == 0) return;

            var pos = list[0];
            float left = pos.x;
            float right = left;
            float bottom = pos.y;
            float top = bottom;
            for (int i = 1; i < count; i++)
            {
                pos = list[i];
                left = Mathf.Min(left, pos.x);
                right = Mathf.Max(right, pos.x);
                bottom = Mathf.Min(bottom, pos.y);
                top = Mathf.Max(top, pos.y);
            }
            float width = right - left;
            float height = top - bottom;
            float deltaX = -width * 0.5f - left;
            float deltaY = -height * 0.5f - bottom;
            for (int i = 0; i < count; i++)
            {
                pos = list[i];
                pos.x += deltaX;
                pos.y += deltaY;
                list[i] = pos;
            }
        }

        public static void CheckRemoveMiddlePoints(this List<Vector3> points, float minDistance)
        {
            if (minDistance <= 0) return;

            int count = points.GetCount();
            if (count < 3) return;

            float minDistanceSquare = minDistance * minDistance;
            int i = 1;
            do
            {
                var point1 = points[i - 1];
                var point2 = points[i];
                float deltaX = point2.x - point1.x;
                float deltaY = point2.y - point1.y;
                float distanceSquare = deltaX * deltaX + deltaY * deltaY;
                if (distanceSquare < minDistanceSquare)
                {
                    points.RemoveAt(i);
                    count--;
                }
                else
                {
                    i++;
                }
            }
            while (i < count - 1);
        }

        public static void CheckAddMiddlePoints(this List<Vector3> points, float maxDistance)
        {
            if (maxDistance <= 0) return;

            int count = points.GetCount();
            if (count < 2) return;

            float maxDistanceSquare = maxDistance * maxDistance;
            for (int i = count - 2; i >= 0; i--)
            {
                var point1 = points[i];
                var point2 = points[i + 1];
                float deltaX = point2.x - point1.x;
                float deltaY = point2.y - point1.y;
                float distanceSquare = deltaX * deltaX + deltaY * deltaY;
                if (distanceSquare > maxDistanceSquare)
                {
                    float distance = Mathf.Sqrt(distanceSquare);
                    float angle = Mathf.Atan2(deltaY, deltaX);
                    int segmentCount = Mathf.CeilToInt(distance / maxDistance);
                    float step = distance / segmentCount;
                    float stepX = step * Mathf.Cos(angle);
                    float stepY = step * Mathf.Sin(angle);
                    for (int j = 1; j < segmentCount; j++)
                    {
                        point1.x += stepX;
                        point1.y += stepY;
                        points.Insert(i + j, point1);
                    }
                }
            }
        }

        public static string ToLineString<T>(this List<T> list, int count = -1)
        {
            if (list == null) return StringNull;
            if (count < 0) count = list.Count;
            if (count == 0) return StringListEmpty;
            if (count == 1) return list[0].ToString();

            return StringHelper.GetString(sb =>
            {
                sb.Append(list[0].ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"\n{list[i]}");
                }
            });
        }

        public static List<U> GetUniqueList<T, U>(this List<T> list, Func<T, U> func) where U : new()
        {
            if (list != null)
            {
                int count = list.Count;
                var list2 = new List<U>(count);
                for (int i = 0; i < count; i++)
                {
                    var item = func(list[i]);
                    if (item != null && !list2.Contains(item))
                    {
                        list2.Add(item);
                    }
                }
                return list2;
            }
            return default;
        }

        /// <summary>
        /// Returns (null) or [] or [item1, item2, ...]
        /// </summary>
        public static string ToString2<T>(this List<T> list, int count = -1)
        {
            if (list == null) return StringNull;
            if (count < 0) count = list.Count;
            if (count == 0) return StringListEmpty;
            if (count == 1) return $"[{list[0]}]";

            return StringHelper.GetString(sb =>
            {
                sb.Append($"[{list[0]}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($", {list[i]}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// Returns (null) or [] or [item1?item2?...]
        /// </summary>
        public static string ToString<T>(this List<T> list, string separator, int count = -1)
        {
            if (list == null) return StringNull;
            if (count < 0) count = list.Count;
            if (count == 0) return StringListEmpty;
            if (count == 1) return $"[{list[0]}]";

            return StringHelper.GetString(sb =>
            {
                sb.Append($"[{list[0]}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{list[i]}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// Returns (null) or [] or [item1, item2, ...]
        /// </summary>
        public static string ToString<T>(this List<T> list, Func<T, object> func, int count = -1)
        {
            if (list == null) return StringNull;
            if (count < 0) count = list.Count;
            if (count == 0) return StringListEmpty;
            if (count == 1) return $"[{func(list[0])}]";

            return StringHelper.GetString(sb =>
            {
                sb.Append($"[{func(list[0])}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($", {func(list[i])}");
                }
                sb.Append("]");
            });
        }
        
        private static System.Random rng = new System.Random();  

        //Fisher-Yates shuffle
        public static void Shuffle<T>(this List<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (list[k],list[n]) = (list[n],list[k]);
            }  
        }
    }
}