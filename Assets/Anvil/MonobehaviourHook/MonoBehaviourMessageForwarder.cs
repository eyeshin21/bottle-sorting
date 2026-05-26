using System;
using UnityEngine;

public class MonoBehaviourMessageForwarder : MonoBehaviour
{
#region Static_Forwarder
    public class CommonMessageInstace : IMonoBevhaviourMessageHandler
    {
        public Action onAwake;
        public Action onStart;
        public Action onUpdate;
        public Action clockUpdate;
        public Action onDestroy;
        public Action onEnable;
        public Action onDisable;
        public Action<bool> onApplicationPause;
        private float lastUpdateSpan = 0;

        public void OnAwake()
        {
            onAwake?.Invoke();
        }

        public void OnStart()
        {
            onStart?.Invoke();
        }

        public void OnUpdate(float deltaTime)
        {
            onUpdate?.Invoke();

            lastUpdateSpan += deltaTime;
            if (lastUpdateSpan >= 1)
            {
                clockUpdate?.Invoke();
                lastUpdateSpan -= 1;

                //Prevent from stacking up due to lag
                if (lastUpdateSpan >= 1)
                {
                    lastUpdateSpan = 0;
                }
            }
            // // if (lastUpdateSpan >= 1)
            // {
            //     if (lastUpdateSpan >= 2)
            //     {
            //
            //     }
            //     else
            //     {
            //         clockUpdate?.Invoke();
            //         lastUpdateSpan -= 1;
            //     }
            //
            // }
        }

        public void OnDestroy()
        {
            onDestroy?.Invoke();
        }

        public void OnApplicationPause(bool state)
        {
            onApplicationPause?.Invoke(state);
        }

        public void OnEnable()
        {
            onEnable?.Invoke();
        }

        public void OnDisable()
        {
            onDisable?.Invoke();
        }
    }

    private static CommonMessageInstace _commonMessageInstace;

    public static CommonMessageInstace CommonForwarder
    {
        get
        {
            if (_commonMessageInstace == null)
            {
                _commonMessageInstace = new CommonMessageInstace();
                var commonMessageForwarder = Create(_commonMessageInstace);
                DontDestroyOnLoad(commonMessageForwarder.gameObject);
                // _commonMessageInstace.onDestroy += ()=>{ _commonMessageInstace = null; };
            }

            return _commonMessageInstace;
        }
    }

    public static void RegisterCommonDestroy(Action onDestroy)
    {
        CommonForwarder.onDestroy += onDestroy;
    }

    public static void RegisterCommonAwake(Action onAwake)
    {
        CommonForwarder.onAwake += onAwake;
    }

    public static void RegisterCommonStart(Action onStart)
    {
        CommonForwarder.onStart += onStart;
    }

    public static void RegisterCommonUpdate(Action onUpdate)
    {
        CommonForwarder.onUpdate += onUpdate;
    }
    public static void UnRegisterCommonUpdate(Action onUpdate)
    {
        CommonForwarder.onUpdate -= onUpdate;
    }

    /// <summary>
    /// Only work when game is running.
    /// NOT TIME ACCURATE
    /// </summary>
    public static void RegisterCommonClockUpdate(Action onClockUpdate)
    {
        CommonForwarder.clockUpdate += onClockUpdate;
    }

    public static void UnRegisterCommonClockUpdate(Action onClockUpdate)
    {
        CommonForwarder.clockUpdate -= onClockUpdate;
    }
    
    public static void RegisterCommonPauseEvent(Action<bool> onPause)
    {
        CommonForwarder.onApplicationPause += onPause;
    }
#endregion

    public IMonoBevhaviourMessageHandler _targetListener;

    public static MonoBehaviourMessageForwarder Create(IMonoBevhaviourMessageHandler target)
    {
        GameObject go = new GameObject($"{target.GetType().Name}_Forwarder");
        MonoBehaviourMessageForwarder forwarder = go.AddComponent<MonoBehaviourMessageForwarder>();
        forwarder._targetListener = target;
        return forwarder;
    }

    private void Awake()
    {
        _targetListener?.OnAwake();
    }

    private void Start()
    {
        _targetListener?.OnStart();
    }

    private void Update()
    {
        _targetListener?.OnUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _targetListener?.OnDestroy();
    }

    private void OnEnable()
    {
        _targetListener?.OnEnable();
    }

    private void OnDisable()
    {
        _targetListener?.OnDisable();
    }

    private void OnApplicationPause(bool state)
    {
        _targetListener?.OnApplicationPause(state);
    }
}
