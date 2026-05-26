using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static class FlagHelper
    {
        const int MaxBit = 31;
        const int LongMaxBit = 63;

        static int[] _bitFlags = GetBitFlags();
        static long[] _longBitFlags = GetLongBitFlags();

        static int[] GetBitFlags()
        {
            int length = MaxBit;
            int[] bitFlags = new int[length];
            int flag = 1;
            for (int i = 0; i < length; i++)
            {
                bitFlags[i] = flag;
                flag <<= 1;
            }
            return bitFlags;
        }

        static long[] GetLongBitFlags()
        {
            int length = LongMaxBit;
            long[] bitFlags = new long[length];
            long flag = 1;
            for (int i = 0; i < length; i++)
            {
                bitFlags[i] = flag;
                flag <<= 1;
            }
            return bitFlags;
        }

        /// <summary>
        /// Gets flag of the specified bit.
        /// </summary>
        /// <param name="bit">0-30</param>
        public static int GetFlag(int bit)
        {
            Assert.IsInRange(bit, 0, _bitFlags.Length - 1);
            return bit < 0 ? 0 : _bitFlags[bit];
        }

        /// <summary>
        /// Gets long flag of the specified bit.
        /// </summary>
        /// <param name="bit">0-62</param>
        public static long GetLongFlag(int bit)
        {
            Assert.IsInRange(bit, 0, _longBitFlags.Length - 1);
            return bit < 0 ? 0 : _longBitFlags[bit];
        }

        public static int GetAllBits<T>() where T : struct
        {
            return GetAllBits(Helper.GetEnumCount<T>());
        }

        public static int GetAllBits(int bitCount)
        {
            int length = _bitFlags.Length;
            if (bitCount < length)
            {
                return GetFlag(bitCount) - 1;
            }

            LegacyLog.Warning($"Out of range: {bitCount} vs {length}");
            int bits = 0;
            for (int i = 0; i < length; i++)
            {
                bits |= _bitFlags[i];
            }
            return bits;
        }

        public static bool IsOn(int flags, int bit)
        {
            return (flags & GetFlag(bit)) != 0;
        }

        public static bool IsOn<T>(int flags, T bit) where T : Enum
        {
            return (flags & GetFlag(bit.ToInt())) != 0;
        }

        public static bool IsOn<T>(T flags, T flag) where T : Enum
        {
            return (flags.ToInt() & flag.ToInt()) != 0;
        }

        public static int Add(int flags, int bit)
        {
            return flags | GetFlag(bit);
        }

        public static int Remove(int flags, int bit)
        {
            return flags & (~GetFlag(bit));
        }

        public static T Add<T>(T flags, T flag) where T : Enum
        {
            return (T)(object)(flags.ToInt() | flag.ToInt());
        }

        public static T Remove<T>(T flags, T flag) where T : Enum
        {
            return (T)(object)(flags.ToInt() & (~flag.ToInt()));
        }

        public static int SetOn(int flags, int bit, bool isOn)
        {
            return isOn ? flags | GetFlag(bit) : flags & (~GetFlag(bit));
        }

        public static int SetOn<T>(int flags, T bit, bool isOn) where T : Enum
        {
            return isOn ? flags | GetFlag(bit.ToInt()) : flags & (~GetFlag(bit.ToInt()));
        }

        public static void SetOn(ref int flags, int bit, bool isOn)
        {
            flags = isOn ? flags | GetFlag(bit) : flags & (~GetFlag(bit));
        }

        public static void SetOn<T>(ref int flags, T bit, bool isOn) where T : Enum
        {
            flags = isOn ? flags | GetFlag(bit.ToInt()) : flags & (~GetFlag(bit.ToInt()));
        }

        public static void SetOn(int flags, int bit, bool isOn, Callback<int> changedCallback)
        {
            int newFlags = isOn ? flags | GetFlag(bit) : flags & (~GetFlag(bit));
            if (newFlags != flags)
            {
                changedCallback?.Invoke(newFlags);
            }
        }

        public static bool IsOn(long flags, int bit)
        {
            return (flags & GetLongFlag(bit)) != 0;
        }

        public static long SetOn(long flags, int bit, bool isOn)
        {
            return isOn ? flags | GetLongFlag(bit) : flags & (~GetLongFlag(bit));
        }

        public static void SetOn(long flags, int bit, bool isOn, Callback<long> changedCallback)
        {
            long newFlags = isOn ? flags | GetLongFlag(bit) : flags & (~GetLongFlag(bit));
            if (newFlags != flags)
            {
                changedCallback?.Invoke(newFlags);
            }
        }

        public static string GetLog<T>(int flags) where T : struct
        {
            return GetLog(flags, Helper.GetEnumCount<T>(), bit => ((T)(object)GetFlag(bit)).ToString());
        }

        public static string GetLog<T>(T flags) where T : struct
        {
            return GetLog((int)(object)flags, Helper.GetEnumCount<T>(), bit => ((T)(object)GetFlag(bit)).ToString());
        }

        public static string GetLog(int flags, int bitCount, Func<int, string> bitNameFunc)
        {
            if (bitNameFunc == null)
            {
                bitNameFunc = (bit) => $"Bit{bit}";
            }

            return Helper.CreateString(sb =>
            {
                for (int bit = 0; bit < bitCount; bit++)
                {
                    if (IsOn(flags, bit))
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append($"{{ {bitNameFunc(bit)}");
                        }
                        else
                        {
                            sb.Append($", {bitNameFunc(bit)}");
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Append(" }");
                }
                else
                {
                    sb.Append("{}");
                }
            });
        }

        public static string GetLog(long flags, int bitCount, Func<long, string> bitNameFunc)
        {
            if (bitNameFunc == null)
            {
                bitNameFunc = (bit) => $"Bit{bit}";
            }

            return Helper.CreateString(sb =>
            {
                for (int bit = 0; bit < bitCount; bit++)
                {
                    if (IsOn(flags, bit))
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append($"{{ {bitNameFunc(bit)}");
                        }
                        else
                        {
                            sb.Append($", {bitNameFunc(bit)}");
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Append(" }");
                }
                else
                {
                    sb.Append("{}");
                }
            });
        }
    }
}