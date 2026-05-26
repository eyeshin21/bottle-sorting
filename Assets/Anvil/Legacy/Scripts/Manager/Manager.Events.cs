#if DEBUG_MODE
#define DEBUG_MANAGER_EVENTS
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public partial class Manager
    {
        static event Listener _onUpdate;
        static event Listener _onLateUpdate;

        static event Listener _onApplicationPause;
        static event Listener _onApplicationResume;
        static event Listener _onApplicationQuit;

#if DEBUG_MANAGER_EVENTS
        static List<Listener> _onUpdateListeners = new();
        static List<Listener> _onLateUpdateListeners = new();

        static List<Listener> _onApplicationPauseListeners = new();
        static List<Listener> _onApplicationResumeListeners = new();
        static List<Listener> _onApplicationQuitListeners = new();

        static void CheckAddListener(List<Listener> listeners, Listener listener)
        {
            if (listeners.Contains(listener))
            {
               LegacyLog.Warning($"Listener {listener} already added!");
            }
            else
            {
                listeners.Add(listener);
            }
        }

        static void CheckRemoveListener(List<Listener> listeners, Listener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
            else
            {
               LegacyLog.Warning($"Listener {listener} not added!");
            }
        }
#endif

        public static void AddOnUpdate(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onUpdateListeners, listener);
#endif
            _onUpdate += listener;
        }
        public static void RemoveOnUpdate(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onUpdateListeners, listener);
#endif
            _onUpdate -= listener;
        }

        public static void AddOnLateUpdate(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onLateUpdateListeners, listener);
#endif
            _onLateUpdate += listener;
        }
        public static void RemoveOnLateUpdate(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onLateUpdateListeners, listener);
#endif
            _onLateUpdate -= listener;
        }

        public static void AddOnApplicationPause(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onApplicationPauseListeners, listener);
#endif
            _onApplicationPause += listener;
        }
        public static void RemoveOnApplicationPause(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onApplicationPauseListeners, listener);
#endif
            _onApplicationPause -= listener;
        }

        public static void AddOnApplicationResume(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onApplicationResumeListeners, listener);
#endif
            _onApplicationResume += listener;
        }
        public static void RemoveOnApplicationResume(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onApplicationResumeListeners, listener);
#endif
            _onApplicationResume -= listener;
        }

        public static void AddOnApplicationQuit(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onApplicationQuitListeners, listener);
#endif
            _onApplicationQuit += listener;
        }
        public static void RemoveOnApplicationQuit(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onApplicationQuitListeners, listener);
#endif
            _onApplicationQuit -= listener;
        }

#if DEBUG_MODE
        static event Listener _onDrawGizmos;
#if DEBUG_MANAGER_EVENTS
        static List<Listener> _onDrawGizmosListeners = new();
#endif
        public static void AddOnDrawGizmos(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckAddListener(_onDrawGizmosListeners, listener);
#endif
            _onDrawGizmos += listener;
        }
        public static void RemoveOnDrawGizmos(Listener listener)
        {
#if DEBUG_MANAGER_EVENTS
            CheckRemoveListener(_onDrawGizmosListeners, listener);
#endif
            _onDrawGizmos -= listener;
        }
#endif
    }
}