using System;

namespace Anvil
{
    public static partial class ScriptedEventExtensions
    {
        public static void ExecuteSafe(this IScriptedEvent scriptedEvent, Action callback = null)
        {
            if (scriptedEvent != null)
            {
                scriptedEvent.Execute(callback);
            }
            else
            {
                callback?.Invoke();
            }
        }
        public static void ExecuteAfterFrame(int delayFrame, Action action)
        {
            if (action == null)
            {
                return;
            }
            ScriptedEvent delayEvent = new ScriptedFrameDelayEvent(delayFrame);
            delayEvent.Execute(()=>
            {
                action?.Invoke();
            });
        }
        public static void ExecuteAfterFrame(this IScriptedEvent scriptedEvent, int delayFrame, Action callback = null)
        {
            if (scriptedEvent != null)
            {
                ScriptedEvent delayEvent = new ScriptedFrameDelayEvent(delayFrame);
                delayEvent.Execute(()=>
                {
                    scriptedEvent.Execute(callback);
                });
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
