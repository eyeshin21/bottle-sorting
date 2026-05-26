using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static readonly Type TypeString = typeof(string);

        public static string Clipboard
        {
            get => GUIUtility.systemCopyBuffer;
            set => GUIUtility.systemCopyBuffer = value;
        }

        public static bool IsTypeOfValueOrString<T>()
        {
            var type = typeof(T);
            return type.IsValueType || type == TypeString;
        }

        public static T GetDefault<T>()
        {
            var type = typeof(T);
            if (type.IsValueType || type == TypeString)
            {
                return default(T);
            }
            return (T)Activator.CreateInstance(type);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        public static bool IsEquals(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        public static bool IsEquals(float a, float b, float epsilon)
        {
            return Mathf.Abs(a - b) <= epsilon;
        }

        //public static bool IsEquals(Vector3 v1, Vector3 v2)
        //{
        //    return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
        //}

        public static int Min(int value1, int value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        public static int Min(int value1, int value2, int value3)
        {
            if (value1 < value2)
            {
                return value1 < value3 ? value1 : value3;
            }
            return value2 < value3 ? value2 : value3;
        }

        public static int RoundToInt(float value)
        {
            if (value > 0)
            {
                int n = (int)value;
                value -= n;
                if (value > 0.5f)
                {
                    n++;
                }
                return n;
            }
            else if (value < 0)
            {
                int n = (int)value;
                value -= n;
                if (value < -0.5f)
                {
                    n--;
                }
                return n;
            }
            return 0;
        }

        public static int RoundToInt(double value)
        {
            return RoundToInt((float)value);
        }

        public static float GetDiagonal(float width, float height)
        {
            return Mathf.Sqrt(width * width + height * height);
        }

        /// <summary>
        /// Returns angle in [0,360)
        /// </summary>
        public static float ClampAngle(float angle)
        {
            if (angle < 0)
            {
                do
                {
                    angle += 360;
                }
                while (angle < 0);
            }
            else if (angle >= 360)
            {
                do
                {
                    angle -= 360;
                }
                while (angle >= 360);
            }
            return angle;
        }

        public static int[] CreateIndices<T>(T[] a)
        {
            return CreateIndices(a.Length);
        }

        public static int[] CreateIndices(int count)
        {
            var indices = new int[count];
            for (int i = 0; i < count; i++)
            {
                indices[i] = i;
            }
            return indices;
        }

        public static float GetDistance(Vector3 pos1, Vector3 pos2)
        {
            float deltaX = pos2.x - pos1.x;
            float deltaY = pos2.y - pos1.y;
            return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static float GetDistanceSquare(Vector3 pos1, Vector3 pos2)
        {
            float deltaX = pos2.x - pos1.x;
            float deltaY = pos2.y - pos1.y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static int GetClosestPointIndex(List<Vector3> points, Vector3 pos)
        {
            int pointCount = points.Count;
            int index = 0;
            float minDistance = GetDistanceSquare(pos, points[0]);
            for (int i = 1; i < pointCount; i++)
            {
                float distance = GetDistanceSquare(pos, points[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }
            return index;
        }

        public static void UpdateSizeByHeight(ref Vector2 size, float height)
        {
            float scale = height / size.y;
            size.x *= scale;
            size.y = height;
        }

        public static int GetPOT(int n)
        {
            int pot = 2;
            while (pot < n)
            {
                pot *= 2;
            }
            return pot;
        }

        public static int GetPOT(float n)
        {
            int pot = 2;
            while (pot < n)
            {
                pot *= 2;
            }
            return pot;
        }

        /// <summary>
        /// Compare: -1 => Left, 1 => Right
        /// </summary>
        public static void TryGetBinary(Vector3 pos1, Vector3 pos2, float length, Func<Vector3, int> compareFunc)
        {
            float deltaX = pos2.x - pos1.x;
            float deltaY = pos2.y - pos1.y;
            float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
            float left = 0;
            float right = length;
            var pos = pos1;
            do
            {
                float middle = (left + right) * 0.5f;
                float ratio = middle / distance;
                pos.x = pos1.x + deltaX * ratio;
                pos.y = pos1.y + deltaY * ratio;
                int compare = compareFunc(pos);
                if (compare == 0) return;
                if (compare < 0)
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
                //#if UNITY_EDITOR
                if (Mathf.Abs(right - left) <= 0.001f)
                {
                    LegacyLog.Warning($"Break: left={left}, right={right}");
                    break;
                }
                //#endif
            }
            while (true);
        }

#if UNITY_EDITOR
        //[UnityEditor.MenuItem("Test/Round To Int")]
        public static void TestRoundToInt()
        {
            for (int i = 0; i < 100; i++)
            {
                float value = Random.Range(-100, 100);
                if (GetRandomBool())
                {
                    value += 0.5f;
                }
                else
                {
                    value += Random.value;
                }

                int value1 = Mathf.RoundToInt(value);
                int value2 = RoundToInt(value);
                if (value1 != value2)
                {
                    LegacyLog.Warning($"{value:0.000}: {value1} vs {value2}");
                }
            }
        }
#endif
    }
}