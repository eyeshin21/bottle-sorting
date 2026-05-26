using System;

namespace Anvil.Legacy
{
    public abstract class VibrationController
    {
#if DEBUG_MODE
        public string Config { get; set; }
#endif
        public abstract long Milliseconds { get; }

        public virtual void Vibrate()
        {
            Vibration.Vibrate();
        }

        class CallbackVibrationController : VibrationController
        {
            Action _callback;

            public CallbackVibrationController(Action callback)
            {
                _callback = callback;
            }

            public override long Milliseconds
            {
                get
                {
                    if (_callback == Vibration.VibratePop) return 50;
                    if (_callback == Vibration.VibratePeek) return 100;
                    if (_callback == Vibration.VibrateNope) return 150;
                    return 100;
                }
            }

            public override void Vibrate()
            {
                _callback?.Invoke();
            }
        }

        class MillisecondsVibrationController : VibrationController
        {
            long _milliseconds;

            public MillisecondsVibrationController(long milliseconds)
            {
                _milliseconds = milliseconds;
            }

            public override long Milliseconds => _milliseconds;

            public override void Vibrate()
            {
                Vibration.Vibrate(_milliseconds);
            }
        }

        class PatternVibrationController : VibrationController
        {
            long[] _pattern;

            public PatternVibrationController(long[] pattern)
            {
                _pattern = pattern;
            }

            public override long Milliseconds
            {
                get
                {
                    long milliseconds = 0;
                    for (int i = _pattern.Length - 1; i >= 0; i--)
                    {
                        milliseconds += _pattern[i];
                    }
                    return milliseconds;
                }
            }

            public override void Vibrate()
            {
                Vibration.Vibrate(_pattern, -1);
            }
        }

        public static VibrationController Create(VibrationData data)
        {
            if (data != null)
            {
#if UNITY_IOS
                var callback = GetVibrationCallback(data.iOS);
                if (callback != null)
                {
                    return Create(callback);
                }
#else
                var config = data.Android;
                if (!string.IsNullOrEmpty(config))
                {
                    return Create(config);
                }
#endif
            }

            return null;
        }

        static VibrationController Create(string config)
        {
            VibrationController controller;

            var items = config.Split(',');
            int count = items.Length;
            if (count == 1)
            {
                controller = new MillisecondsVibrationController(config.ToInt());
            }
            else
            {
                var pattern = new long[1 + count];
                pattern[0] = 0;
                for (int i = 0; i < count; i++)
                {
                    pattern[i + 1] = items[i].ToInt();
                }

                controller = new PatternVibrationController(pattern);
            }
#if DEBUG_MODE
            controller.Config = config;
#endif
            return controller;
        }

#if UNITY_IOS
        static Action GetVibrationCallback(iOSVibrationType vibrationType)
        {
            if (vibrationType == iOSVibrationType.Pop) return Vibration.VibratePop;
            if (vibrationType == iOSVibrationType.Peek) return Vibration.VibratePeek;
            if (vibrationType == iOSVibrationType.Nope) return Vibration.VibrateNope;
            return null;
        }

        static VibrationController Create(Action callback)
        {
            var controller = new CallbackVibrationController(callback);
#if DEBUG_MODE
            if (callback == Vibration.VibratePop)
            {
                controller.Config = "50";
            }
            else if (callback == Vibration.VibratePeek)
            {
                controller.Config = "100";
            }
            else if (callback == Vibration.VibrateNope)
            {
                controller.Config = "50,50,50";
            }
#endif
            return controller;
        }
#endif
    }
}