using System.Globalization;
using UnityEngine;

namespace Anvil
{
    public static partial class JsonSerializer
    {
        
        public static Vector3 ParseVector3(this string str)
        {
            var parts = str.Split(ValueSeparator);
            if (parts.Length != 3)
            {
                Debug.LogError($"Cannot convert string '{str}' to Vector3. Expected format: 'x_y_z'");
                return Vector3.zero;
            }

            if (TryParseSerializedFloat(parts[0], out float x) &&
                TryParseSerializedFloat(parts[1], out float y) &&
                TryParseSerializedFloat(parts[2], out float z))
            {
                return new Vector3(x, y, z);
            }
            else
            {
                Debug.LogError($"Cannot convert string '{str}' to Vector3. Invalid number format.");
                return Vector3.zero;
            }
        }

        public static string ToSerializeSafeString(this Vector3 vector)
        {
            return $"{vector.x.ToSerializeSafeString()}{ValueSeparator}{vector.y.ToSerializeSafeString()}{ValueSeparator}{vector.z.ToSerializeSafeString()}";
        }
        static readonly int FloatToInt = 1000000;
        static readonly float IntToFloat = 0.000001f;
        private static string ToSerializeSafeString(this float value)
        {
            int intValue = Mathf.RoundToInt(value * FloatToInt);
            value = intValue * IntToFloat;
            return value.ToString(CultureInfo.InvariantCulture);
        }
        public static bool TryParseSerializedFloat(this string str, out float value)
        {
            if (float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }
            else
            {
                value = 0;
                Debug.LogError($"Cannot convert string '{str}' to float. Invalid number format.");
                return false;
            }
        }
    }
}