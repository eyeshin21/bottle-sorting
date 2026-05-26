using System;
using System.Collections;
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static void ExecuteNextFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            if (monoBehaviour == null)
            {
                action?.Invoke();
                return;
            }

            monoBehaviour.StartCoroutine(coRoutine());

            IEnumerator coRoutine()
            {
                yield return null;
                action?.Invoke();
            }
        }

        public static void ExecuteNextFrame(this Action action,Action lateSecondaryAction = null)
        {
            if (Manager.Instance == null)
            {
                action?.Invoke();
                lateSecondaryAction?.Invoke();
                return;
            }

            Manager.Instance.StartCoroutine(coRoutine());

            IEnumerator coRoutine()
            {
                yield return null;
                action?.Invoke();
                lateSecondaryAction?.Invoke();
            }
        }

        public static void DelayedExecuteSecond(this Action action,float delay)
        {
            if (Manager.Instance == null)
            {
                action?.Invoke();
                return;
            }

            Manager.Instance.StartCoroutine(coRoutine());

            IEnumerator coRoutine()
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }

        public static void DelayedExecuteFrame(this Action action,int frame)
        {
            if (Manager.Instance == null)
            {
                action?.Invoke();
                return;
            }

            Manager.Instance.StartCoroutine(coRoutine());

            IEnumerator coRoutine()
            {
                while (frame > 0)
                {
                    yield return null;
                    frame--;
                }

                action?.Invoke();
            }
        }

        // public static Vector3 CalculateNativeUIScale(this Vector3 desiredScale)
        // {
        //     GameObject canvasObj = PoolHelper.ParentPopup.gameObject;
        //     Vector3 canvasScale = canvasObj.transform.localScale;
        //     Debug.Log("canvas scale: " + canvasScale);
        //     Vector3 nativeScale = new Vector3(desiredScale.x * canvasScale.x,desiredScale.y * canvasScale.y,desiredScale.z * canvasScale.z);
        //     Debug.Log($"native scale {nativeScale}");
        //     return nativeScale;
        // }

        // public static T2 GetOrAdd<T1,T2>(this Dictionary<T1,T2> dictionary, T1 key) where T2 : new()
        // {
        //     T2 value = new T2();
        //     if (dictionary.TryGetValue(key, out value))
        //     {
        //         return value;
        //     }
        //     dictionary.Add(key, value);
        //     return value;
        // }
        public static TValue GetOrCreate<TKey,TValue>(this IDictionary<TKey,TValue> dict,TKey key)
            where TValue : new()
        {
            if (!dict.TryGetValue(key,out TValue val))
            {
                val = new TValue();
                dict.Add(key,val);
            }

            return val;
        }
        public static TValue GetOrCreateNull<TKey,TValue>(this IDictionary<TKey,TValue> dict,TKey key)
            where TValue : class
        {
            if (!dict.TryGetValue(key,out TValue val))
            {
                dict.Add(key,null);
            }

            return null;
        }

        public static List<int> CreateIntList(this int count)
        {
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }

            return list;
        }

        public static void ExecuteSequence(this List<IScriptedEvent> events,Action callback)
        {
            if (events.IsNullOrEmpty())
            {
                callback?.Invoke();
                return;
            }

            int index = 1;

            void NextEvent()
            {
                if (index >= events.Count)
                {
                    callback?.Invoke();
                    return;
                }
                // Debug.Log($"executing {index}");
                IScriptedEvent scriptedEvent = events[index];
                scriptedEvent.ExecuteSafe(()=>
                {
                    index++;
                    NextEvent();
                });
            }

            IScriptedEvent firstEvent = events[0];
            // Debug.Log("executing 0");
            firstEvent.ExecuteSafe(NextEvent);
        }
        public static void ExecuteSequence<T>(this List<T> events,Action callback) where T : IScriptedEvent
        {
            List<IScriptedEvent> scriptedEvents = new List<IScriptedEvent>(events.Count);
            foreach (T scriptedEvent in events)
            {
                scriptedEvents.Add(scriptedEvent);
            }
            ExecuteSequence(scriptedEvents, callback);
        }
        public static void ExecuteAll(this List<IScriptedEvent> events)
        {
            if (events.IsNullOrEmpty())
            {
                return;
            }

            foreach (IScriptedEvent scriptedEvent in events)
            {
                scriptedEvent.ExecuteSafe();
            }
        }
        public static void ExecuteAll(this List<IScriptedEvent> events, Action callbackFromLast)
        {
            if (events.IsNullOrEmpty())
            {
                callbackFromLast?.Invoke();
                return;
            }
            for (int i = 0; i < events.Count; i++)
            {
                if (i == events.Count - 1)
                {
                    events[i].ExecuteSafe(callbackFromLast);
                }
                else
                {
                    events[i].ExecuteSafe();
                }
            }
        }
        public static string ToRichTextTag(this Color color)
        {
            return $"<color=#{color.ToHtlmRGBA()}>";
        }
        public static string ToHtlmRGBA(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        public static string ToHtlmRGBATag(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }

        public static void TryPlayUIAnimation(this GameObject gameObject, string animationName, Action callback = null)
        {
            if (gameObject == null)
            {
                callback?.Invoke();
                return;
            }

            IUIAnimationController animationController = gameObject.GetComponent<IUIAnimationController>();
            if (animationController == null)
            {
                callback?.Invoke();
                return;
            }
            animationController.PlayAnimation(animationName, callback);
        }
    }
}
