using UnityEngine;

namespace Anvil.Legacy
{
    public partial class Manager
    {
        static readonly float DelayCallEpsilon = 0.001f;
        static DelayCallManager _delayCallManager = new DelayCallManager();

        public static DelayCallController DelayCall(float delay, Callback callback, int repeat = 1)
        {
            return _delayCallManager.DelayCall(delay, callback, repeat);
        }

        public static DelayCallController DelayCall(Component component, float delay, Callback callback, int repeat = 1)
        {
            return _delayCallManager.DelayCall(component, delay, callback, repeat);
        }

        public static DelayCallController DelayCall(GameObject gameObject, float delay, Callback callback, int repeat = 1)
        {
            return _delayCallManager.DelayCall(gameObject, delay, callback, repeat);
        }

        public static DelayCallController DelayCall<T>(float delay, Callback<T> callback, T value)
        {
            return _delayCallManager.DelayCall(delay, callback, value);
        }

        public static DelayCallController CallOnUpdate(Callback callback)
        {
            return _delayCallManager.CallOnUpdate(callback);
        }

        public static DelayCallController CallOnUpdate<T>(Callback<T> callback, T value)
        {
            return _delayCallManager.CallOnUpdate(callback, value);
        }

        public static DelayCallController CallOnMainThread(Callback callback)
        {
            return _delayCallManager.DelayCall(DelayCallEpsilon, callback);
        }

        public static DelayCallController CallOnMainThread<T>(Callback<T> callback, T value)
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

        public static void StopDelayCalls(Component component)
        {
            _delayCallManager.StopDelayCalls(component);
        }

        public static void StopDelayCalls(GameObject gameObject)
        {
            _delayCallManager.StopDelayCalls(gameObject);
        }

        public static void StopDelayCalls()
        {
            _delayCallManager.StopDelayCalls();
        }
    }
}