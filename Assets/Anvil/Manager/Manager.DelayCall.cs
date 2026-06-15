using System;

namespace Anvil
{
    public partial class Manager
    {
        static readonly float DelayCallEpsilon = 0.001f;
        static DelayCallManager _delayCallManager = new DelayCallManager();

        public static DelayCallController DelayCall(float delay, Action callback, int repeat = 1)
        {
            return _delayCallManager.DelayCall(delay, callback, repeat);
        }

        public static DelayCallController DelayCall<T>(float delay, Action<T> callback, T value)
        {
            return _delayCallManager.DelayCall(delay, callback, value);
        }

        public static DelayCallController CallOnUpdate(Action callback)
        {
            return _delayCallManager.CallOnUpdate(callback);
        }

        public static DelayCallController CallOnUpdate<T>(Action<T> callback, T value)
        {
            return _delayCallManager.CallOnUpdate(callback, value);
        }

        public static DelayCallController CallOnMainThread(Action callback)
        {
            return _delayCallManager.DelayCall(DelayCallEpsilon, callback);
        }

        public static DelayCallController CallOnMainThread<T>(Action<T> callback, T value)
        {
            return _delayCallManager.DelayCall(DelayCallEpsilon, callback, value);
        }

        public static void StopDelayCall(ref DelayCallController controller)
        {
            if (controller != null)
            {
                controller.Stop();
                controller = null;
            }
        }
    }
}