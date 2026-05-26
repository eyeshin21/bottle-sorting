using UnityEngine;
using System;
using System.Globalization;
using System.Net;
using Anvil.Legacy;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class TimeHelper
{
    private static float _startSeconds;
    private static DateTime? _localStartDateTime;
    private static DateTime? _globalStartDateTime;

    public static float TimeSinceStartup => Time.realtimeSinceStartup;
#if UNITY_EDITOR
    public static float EditorTimeSinceStartup => (float)EditorApplication.timeSinceStartup;
#endif

    /// <summary>
    /// Saves start seconds.
    /// Sets local and global date time.
    /// </summary>
    public static void Init()
    {
        InitTracker.Track("TimeHelper");
        _startSeconds = SecondsFromStart;
        _localStartDateTime = LocalDateTime;
        _globalStartDateTime = GlobalDateTime;
    }

    /// <summary>
    /// The real time in seconds since the game started.
    /// </summary>
    public static float SecondsFromStart => Time.realtimeSinceStartup;
    public static int SecondsFromStartInt => Mathf.RoundToInt(SecondsFromStart);

    // public static int DaysFromInstall
    // {
    //     get
    //     {
    //         var installTime = UserPrefs.InstallTime;
    //         if (installTime.HasValue)
    //         {
    //             return CurrentDateTime.Subtract(installTime.Value).Days;
    //         }
    //         return 0;
    //     }
    // }

    public static DateTime LocalDateTime
    {
        get
        {
            var dateTime = DateTime.Now;
#if DEBUG_MODE
            var hackSeconds = HackSeconds;
            if (hackSeconds != 0)
            {
                dateTime = dateTime.AddSeconds(hackSeconds);
            }
#endif
            return dateTime;
        }
    }

    public static DateTime? GlobalDateTime
    {
        get
        {
            try
            {
                var request = WebRequest.Create("http://www.google.com");
                request.Timeout = 1000; // Miliseconds
                using (var response = request.GetResponse())
                {
                    return DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Returns global current date time based on global start date time (if any).
    /// </summary>
    public static DateTime? GlobalDateTimeByStart
    {
        get
        {
            if (_globalStartDateTime.HasValue)
            {
                float deltaSeconds = SecondsFromStart - _startSeconds;
#if DEBUG_MODE
                deltaSeconds += HackSeconds;
#endif
                return _globalStartDateTime.Value.AddSeconds(deltaSeconds);
            }

            return null;
        }
    }

    /// <summary>
    /// Returns global date time (if any) or local date time.
    /// </summary>
    public static DateTime CurrentDateTime
    {
        get
        {
            var globalDateTime = GlobalDateTimeByStart;
            return globalDateTime.HasValue ? globalDateTime.Value : LocalDateTime;
        }
    }

    public static DateTime UniversalDateTime => CurrentDateTime.ToUniversalTime();

    public static DateTime GetDateTimeOrCurrentDateTime(DateTime? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value : CurrentDateTime;
    }

    public static bool Equals(DateTime? dateTime1, DateTime? dateTime2)
    {
        if (dateTime1.HasValue)
        {
            return dateTime2.HasValue ? dateTime1.Value.CompareTo(dateTime2.Value) == 0 : false;
        }

        return !dateTime2.HasValue;
    }

    public static DateTime GetNextDateTime(int seconds)
    {
        return CurrentDateTime.AddSeconds(seconds);
    }

    public static DateTime GetNextDateTime(DateTime? dateTime, int seconds)
    {
        var currentDateTime = CurrentDateTime;
        if (dateTime.HasValue && dateTime.Value > currentDateTime)
        {
            return dateTime.Value.AddSeconds(seconds);
        }

        return currentDateTime.AddSeconds(seconds);
    }

    /// <summary>
    /// Returns days of current date time's YMD with the specified date time's YMD.
    /// Returns -1 if date time is null.
    /// </summary>
    public static int GetDayFromDateTime(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return CurrentDateTime.GetDayMonthYear().Subtract(dateTime.Value.GetDayMonthYear()).Days;
        }
        return -1;
    }

    /// <summary>
    /// Returns days of current date time's YMD with the specified date time's YMD.
    /// Returns -1 if date time is null.
    /// </summary>
    public static int GetUniversalDayFromDateTime(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return CurrentDateTime.ToUniversalTime().GetDayMonthYear().Subtract(dateTime.Value.ToUniversalTime().GetDayMonthYear()).Days;
        }
        return -1;
    }

    /// <summary>
    /// Returns true if currentTime > dateTime
    /// </summary>
    public static bool IsCurrentTimeOver(DateTime dateTime)
    {
        return CurrentDateTime > dateTime;
    }

    public static bool IsCurrentTimeOver(DateTime? dateTime)
    {
        return dateTime.HasValue ? CurrentDateTime > dateTime.Value : true;
    }

    public static bool IsCurrentTimeOver(DateTime? dateTime1, DateTime? dateTime2)
    {
        if (dateTime1.HasValue && dateTime2.HasValue)
        {
            return dateTime1.Value > dateTime2.Value;
        }
        return true;
    }

    /// <summary>
    /// Return (dateTime - current) in seconds.
    /// </summary>
    public static int SecondsFromCurrent(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return (int)dateTime.Value.Subtract(CurrentDateTime).TotalSeconds;
        }

        return 0;
    }

    public static int SecondsToCurrent(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return (int)CurrentDateTime.Subtract(dateTime.Value).TotalSeconds;
        }

        return 0;
    }

    public static int SecondsFromCurrent(DateTime? current, DateTime? dateTime)
    {
        if (current.HasValue && dateTime.HasValue)
        {
            return (int)dateTime.Value.Subtract(current.Value).TotalSeconds;
        }

        return 0;
    }

    public static int SecondsToCurrent(DateTime? current, DateTime? dateTime)
    {
        if (current.HasValue && dateTime.HasValue)
        {
            return (int)current.Value.Subtract(dateTime.Value).TotalSeconds;
        }

        return 0;
    }

    public static bool IsNullOrLessThanCurrentTime(DateTime? dateTime)
    {
        return !dateTime.HasValue || dateTime.Value < CurrentDateTime;
    }

    public static bool IsNullOrGreaterThanCurrentTime(DateTime? dateTime)
    {
        return !dateTime.HasValue || dateTime.Value > CurrentDateTime;
    }

    public static DateTime FromString(string date)
    {
        if (date == null || date == "")
            return CurrentDateTime;

        return DateTime.Parse(date);
    }

    public static DateTime AddSeconds(DateTime? dateTime, int seconds)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.AddSeconds(seconds);
        }
        return CurrentDateTime.AddSeconds(seconds);
    }
    public static DateTime ToDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
    public static DateTime ToDay(this DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
        }
        return DateTime.Today;
    }
#if DEBUG_MODE
    static int _hackSeconds;
    static bool _isInited;

    static void LazyInit()
    {
        if (_isInited) return;
        _isInited = true;

        _hackSeconds = LocalPrefs.GetInt(LocalKeys._HackSeconds);
    }

    static int HackSeconds
    {
        get
        {
            LazyInit();
            return _hackSeconds;
        }
        set
        {
            _hackSeconds = value;
            LocalPrefs.SetInt(LocalKeys._HackSeconds, value);
        }
    }

    public static event Listener onTimeChanged;

    static string _addSecondsText;
    public static void OnGUIDebug()
    {
        GUILayout.BeginHorizontal();
        {
            bool guiEnabled = GUI.enabled;
            int hackSeconds = HackSeconds;
            GUILayout.Label($"Hack time: {hackSeconds.ToHMSString()}");

            _addSecondsText = GUILayout.TextField(_addSecondsText, GUILayout.Width(200));
            int addSeconds = _addSecondsText.ToSeconds();
            GUI.enabled = guiEnabled && addSeconds != 0;
            if (GUILayout.Button("Add"))
            {
                HackSeconds = hackSeconds + addSeconds;
                onTimeChanged?.Invoke();
                _addSecondsText = "";
                Helper.ReloadScene();
            }

            GUI.enabled = guiEnabled && hackSeconds != 0;
            if (GUILayout.Button("Clear"))
            {
                HackSeconds = 0;
                onTimeChanged?.Invoke();
            }

            GUI.enabled = guiEnabled;
            GUILayout.Label("|");

            if (GUILayout.Button("Log Time"))
            {
               LegacyLog.Debug($"Local current time: <b>{LocalDateTime}</b>");
               LegacyLog.Debug($"Global current time: <b>{GlobalDateTime}</b>");
            }

            //GUILayout.Label($"({CurrentDateTime.ToString2()})");

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
#endif
}