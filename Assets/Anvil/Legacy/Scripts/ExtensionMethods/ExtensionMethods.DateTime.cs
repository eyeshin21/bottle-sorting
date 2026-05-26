using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Returns string of date time's binary.
        /// </summary>
        public static string ToSaveString(this DateTime dateTime)
        {
            return dateTime.ToBinary().ToString();
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static string ToSaveString2(this DateTime dateTime)
        {
            return dateTime.ToString2();
        }

        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToLocalTime() : null;
        }

        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : null;
        }

        /// <summary>
        /// Returns string of date time's binary.
        /// </summary>
        public static string ToSaveString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToBinary().ToString() : "";
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static string ToSaveString2(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToSaveString2() : "";
        }

        public static int GetTotalDays(this DateTime dateTime, DateTime fromDateTime)
        {
            return (int)dateTime.Subtract(fromDateTime).TotalDays;
        }

        public static int GetTotalDays(this DateTime dateTime, DateTime? fromDateTime)
        {
            return fromDateTime.HasValue ? (int)dateTime.Subtract(fromDateTime.Value).TotalDays : 0;
        }

        /// <summary>
        /// Returns (this - other) in seconds.
        /// </summary>
        public static float GetTotalSeconds(this DateTime? dateTime, DateTime other)
        {
            if (dateTime.HasValue)
            {
                return (float)dateTime.Value.Subtract(other).TotalSeconds;
            }
            return 0;
        }

        /// <summary>
        /// Returns (this - other) in seconds.
        /// </summary>
        public static float GetTotalSeconds(this DateTime? dateTime, DateTime? other)
        {
            if (dateTime.HasValue && other.HasValue)
            {
                return (float)dateTime.Value.Subtract(other.Value).TotalSeconds;
            }
            return 0;
        }

        /// <summary>
        /// Returns (this - other) in seconds.
        /// </summary>
        public static int GetTotalSecondsInt(this DateTime dateTime, DateTime? other)
        {
            if (other.HasValue)
            {
                return Helper.RoundToInt((float)dateTime.Subtract(other.Value).TotalSeconds);
            }
            return 0;
        }

        /// <summary>
        /// Returns (this - other) in seconds.
        /// </summary>
        public static int GetTotalSecondsInt(this DateTime? dateTime, DateTime? other)
        {
            if (dateTime.HasValue && other.HasValue)
            {
                return Helper.RoundToInt((float)dateTime.Value.Subtract(other.Value).TotalSeconds);
            }
            return 0;
        }

        public static bool IsDayEquals(this DateTime dateTime, DateTime other)
        {
            return dateTime.DayOfYear == other.DayOfYear;
        }

        public static bool IsDayGreaterThan(this DateTime dateTime, DateTime other)
        {
            return dateTime.DayOfYear != other.DayOfYear && dateTime > other;
        }

        /// <summary>
        /// Returns (nextDateTime - dateTime)'s total seconds.
        /// </summary>
        public static int GetSecondsToNextDateTime(this DateTime dateTime, DateTime nextDateTime)
        {
            return Mathf.RoundToInt((float)nextDateTime.Subtract(dateTime).TotalSeconds);
        }

        public static int GetSecondsToEndOfDay(this DateTime dateTime)
        {
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;

            int seconds = 60 - second;
            int minutes = 60 - (minute + 1);
            int hours = 24 - (hour + 1);

            return hours * 3600 + minutes * 60 + seconds;
        }

        public static DateTime GetDayMonthYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static DateTime GetNextDayMonthYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
        }

        public static string ToStringDDMMYYYY(this DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;

            return $"{day:00}/{month:00}/{year:0000}";
        }

        /// <summary>
        /// Returns dd/MM/yy HH:mm:ss
        /// </summary>
        public static string ToStringDDMMYYHHMMSS(this DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year % 100;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;

            if (hour > 0 || minute > 0 || second > 0)
            {
                return $"{day:00}/{month:00}/{year:00} {hour:00}:{minute:00}:{second:00}";
            }

            return $"{day:00}/{month:00}/{year:00}";
        }

        /// <summary>
        /// Returns dd/MM/yy HH:mm:ss
        /// </summary>
        public static string ToStringDDMMYYHHMMSS(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToStringDDMMYYHHMMSS() : "";
        }

        /// <summary>
        /// Returns dd/MM/yy HH:mm:ss (delta time)
        /// </summary>
        public static string ToStringDDMMYYHHMMSS2(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime.Value.ToStringDDMMYYHHMMSS()} ({((float)dateTime.Value.Subtract(TimeHelper.CurrentDateTime).TotalSeconds).ToHMSString()})" : "";
        }

        /// <summary>
        /// Returns yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToStringYYYYMMDDHHMMSS(this DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year % 100;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;

            if (hour > 0 || minute > 0 || second > 0)
            {
                return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";
            }

            return $"{year:0000}/{month:00}/{day:00}";
        }

        /// <summary>
        /// Returns yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToStringYYYYMMDDHHMMSS(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToStringYYYYMMDDHHMMSS() : "";
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static string ToString2(this DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;

            if (hour > 0 || minute > 0 || second > 0)
            {
                return $"{day:00}/{month:00}/{year:0000} {hour:00}:{minute:00}:{second:00}";
            }

            return $"{day:00}/{month:00}/{year:0000}";
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static string ToString2(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString2() : "";
        }

        /// <summary>
        /// Returns (null) or dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static string ToNullOrString2(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString2() : "(null)";
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss (delta time)
        /// </summary>
        public static string ToString3(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime.Value.ToString2()} ({((float)dateTime.Value.Subtract(TimeHelper.CurrentDateTime).TotalSeconds).ToHMSString()})" : "";
        }

        /// <summary>
        /// Returns dd/MM/yyyy HH:mm:ss (delta time)
        /// </summary>
        public static string ToUniversalString3(this DateTime dateTime)
        {
            return $"{dateTime.ToString2()} ({((float)dateTime.Subtract(TimeHelper.UniversalDateTime).TotalSeconds).ToHMSString2()})";
        }

        public static string ToString(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : "";
        }

        /// <summary>
        /// long or
        /// dd/MM or dd-MM
        /// dd/MM/yy or dd-MM-yy
        /// dd/MM/yyyy or dd-MM-yyyy or yyyy/MM/dd or yyyy-MM-dd or
        /// dd/MM/yyyy HH:mm:ss or dd-MM-yyyy HH:mm:ss or yyyy/MM/dd HH:mm:ss or yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static DateTime? ToDateTime2(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int length = s.Length;
                if (s.IndexOf(' ') > 0)
                {
                    if (length >= 19)
                    {
                        // dd/MM/yyyy
                        int n = s[2] - 48;
                        if (n < 0 || n > 9)
                        {
                            int day = GetInt2(s, 0);
                            int month = GetInt2(s, 3);
                            int year = GetInt4(s, 6);
                            int hour = GetInt2(s, 11);
                            int minute = GetInt2(s, 14);
                            int second = GetInt2(s, 17);
                            return new DateTime(year, month, day, hour, minute, second);
                        }

                        // yyyy/MM/dd
                        n = s[4] - 48;
                        if (n < 0 || n > 9)
                        {
                            int year = GetInt4(s, 0);
                            int month = GetInt2(s, 5);
                            int day = GetInt2(s, 8);
                            int hour = GetInt2(s, 11);
                            int minute = GetInt2(s, 14);
                            int second = GetInt2(s, 17);
                            return new DateTime(year, month, day, hour, minute, second);
                        }
                    }

                    LegacyLog.Warning($"Invalid date time \"{s}\"");
                    return null;
                }

                if (length == 5)
                {
                    // dd/MM
                    int n = s[2] - 48;
                    if (n < 0 || n > 9)
                    {
                        int day = GetInt2(s, 0);
                        int month = GetInt2(s, 3);
                        int year = TimeHelper.CurrentDateTime.Year;
                        return new DateTime(year, month, day);
                    }
                }
                else if (length == 8)
                {
                    // dd/MM/yy
                    int n = s[2] - 48;
                    if (n < 0 || n > 9)
                    {
                        int day = GetInt2(s, 0);
                        int month = GetInt2(s, 3);
                        int year = 2000 + GetInt2(s, 6);
                        return new DateTime(year, month, day);
                    }
                }
                else if (length == 10)
                {
                    // dd/MM/yyyy
                    int n = s[2] - 48;
                    if (n < 0 || n > 9)
                    {
                        int day = GetInt2(s, 0);
                        int month = GetInt2(s, 3);
                        int year = GetInt4(s, 6);
                        return new DateTime(year, month, day);
                    }

                    // yyyy/MM/dd
                    n = s[4] - 48;
                    if (n < 0 || n > 9)
                    {
                        int year = GetInt4(s, 0);
                        int month = GetInt2(s, 5);
                        int day = GetInt2(s, 8);
                        return new DateTime(year, month, day);
                    }
                }
                else if (length > 8)
                {
                    return DateTime.FromBinary(s.ToLong());
                }

                LegacyLog.Warning($"Invalid date time \"{s}\"");
                return null;
            }

            return null;
        }

        static int GetInt2(string s, int index)
        {
            int n1 = s[index] - 48;
            int n2 = s[index + 1] - 48;
            return n1 * 10 + n2;
        }

        static int GetInt4(string s, int index)
        {
            int n1 = s[index++] - 48;
            int n2 = s[index++] - 48;
            int n3 = s[index++] - 48;
            int n4 = s[index] - 48;
            return n1 * 1000 + n2 * 100 + n3 * 10 + n4;
        }

        public static bool IsEquals(this DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1.HasValue)
            {
                if (!dateTime2.HasValue) return false;
                return dateTime1.Value.CompareTo(dateTime2.Value) == 0;
            }

            return !dateTime2.HasValue;
        }

        public static int Compare(this DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1.HasValue)
            {
                if (!dateTime2.HasValue) return 1;
                return dateTime1.Value.CompareTo(dateTime2.Value);
            }

            return dateTime2.HasValue ? -1 : 0;
        }

        #region Helper
        public static void GetHMS(this int seconds, out int hours, out int mins, out int secs)
        {
            if (seconds < 60)
            {
                hours = 0;
                mins = 0;
                secs = Mathf.Max(seconds, 0);
            }
            else
            {
                int m = seconds / 60;
                secs = seconds - m * 60;

                if (m < 60)
                {
                    hours = 0;
                    mins = m;
                }
                else
                {
                    hours = m / 60;
                    mins = m - hours * 60;
                }
            }
        }

        /// <summary>
        /// Return seconds in format: "[?d] [?h] [?m] [?s]"
        /// </summary>
        public static string ToDHMSString(this int seconds)
        {
            seconds.GetHMS(out int hours, out int mins, out int secs);
            int days = 0;
            if (hours >= 24)
            {
                days = hours / 24;
                hours = hours - days * 24;
            }

            string s = days > 0 ? $"{days}d" : null;
            if (hours > 0)
            {
                s = s != null ? $"{s} {hours}h" : $"{hours}h";
            }
            if (mins > 0)
            {
                s = s != null ? $"{s} {mins}m" : $"{mins}m";
            }
            if (secs > 0)
            {
                s = s != null ? $"{s} {secs}s" : $"{secs}s";
            }
            return s != null ? s : "";
        }

        /// <summary>
        /// Return seconds in format: "?d [?h]" | "?h [?m]" | "?m [?s]" | "?s"
        /// </summary>
        public static string ToHMSString(this int seconds)
        {
            seconds.GetHMS(out int hours, out int mins, out int secs);

            if (hours > 0)
            {
                if (hours >= 24)
                {
                    int days = hours / 24;
                    hours = hours - days * 24;

                    return hours > 0 ? $"{days}d {hours}h" : $"{days}d";
                }

                return mins > 0 ? $"{hours}h {mins}m" : $"{hours}h";
            }

            if (mins > 0)
            {
                return secs > 0 ? $"{mins}m {secs}s" : $"{mins}m";
            }

            return $"{secs}s";
        }

        public static string ToHMSString(this float seconds)
        {
            return ToHMSString(Mathf.CeilToInt(seconds));
        }

        /// <summary>
        /// Check if seconds is negative
        /// </summary>
        /// <returns></returns>
        public static string ToHMSString2(this int seconds)
        {
            return seconds < 0 ? $"-{ToHMSString(-seconds)}" : ToHMSString(seconds);
        }

        /// <summary>
        /// Check if seconds is negative
        /// </summary>
        /// <returns></returns>
        public static string ToHMSString2(this float seconds)
        {
            return seconds < 0 ? $"-{ToHMSString(-seconds)}" : ToHMSString(seconds);
        }
        #endregion
    }
}