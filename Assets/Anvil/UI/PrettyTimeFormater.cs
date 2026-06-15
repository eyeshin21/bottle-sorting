using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public enum TimeFormat
    {
        /// <summary>
        /// standart 00:00:00
        /// </summary>
        HHMMSS,
        /// <summary>
        /// hh:mm
        /// </summary>
        HHMM,
        /// <summary>
        /// Remove Hour if its zero
        /// </summary>
        HHMMSS_minified,
        /// <summary>
        /// denoted time. 00h:00m:00s
        /// </summary>
        HHMMSS_denoted,
        /// <summary>
        /// /// denoted time. 00h:00m
        /// </summary>
        HHMM_denoted,
        /// <summary>
        /// the leaner version. remove only the edge value if it's zero. 1h 0m 0s will be 1h, 0h 30m 0s will be 30m. but if it's 1h 30m 0s, it will be 1h30m
        /// </summary>
        HHMMSS_denoted_minified,
        /// <summary>
        /// remove all zero value.
        /// </summary>
        HHMMSS_denoted_minified2,
    }
    public static class PrettyTimeFormater
    {
        public static string FormatTime(int seconds, TimeFormat format)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            switch (format)
            {
                case TimeFormat.HHMMSS:
                    return $"{hours:D2}:{minutes:D2}:{secs:D2}";
                case TimeFormat.HHMMSS_denoted:
                    return $"{hours}h {minutes}m {secs}s";
                case TimeFormat.HHMM:
                    return $"{hours:D2}:{minutes:D2}";
                case TimeFormat.HHMM_denoted:
                    return $"{hours}h {minutes}m";
                case TimeFormat.HHMMSS_denoted_minified:
                    return Helper.CreateString(sb =>
                    {
                        if (hours > 0)
                        {
                            sb.Append(hours).Append("h");
                        }
                        if (minutes > 0 || (hours > 0 && secs > 0))
                        {
                            sb.Append(minutes).Append("m");
                        }
                        if (secs > 0)
                        {
                            sb.Append(secs).Append("s");
                        }
                    });
                case TimeFormat.HHMMSS_denoted_minified2:
                    return Helper.CreateString(sb =>
                    {
                        if (hours > 0)
                        {
                            sb.Append(hours).Append("h");
                        }
                        if (minutes > 0)
                        {
                            sb.Append(minutes).Append("m");
                        }
                        if (secs > 0)
                        {
                            sb.Append(secs).Append("s");
                        }
                    });
                case TimeFormat.HHMMSS_minified:
                        return Helper.CreateString(sb =>
                        {
                            if (hours > 0)
                            {
                                sb.Append(hours).Append(":");
                            }
                            // if (minutes > 0 || (hours > 0 && secs > 0))
                            // {
                                sb.Append($"{minutes:D2}").Append(":");
                            // }
                            sb.Append($"{secs:D2}");
                        });
                    break;
                default:
                    return $"{hours:D2}:{minutes:D2}:{secs:D2}";
            }
        }

        public static string FormatTime(TimeSpan timeSpan, TimeFormat format)
        {
            int totalSeconds = (int)timeSpan.TotalSeconds;
            return FormatTime(totalSeconds, format);
        } 
    }
}