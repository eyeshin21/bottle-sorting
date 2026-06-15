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
        public Action onInputAny;
        public Action onInputAnyOnce;
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

            CheckForInput();
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

        private void CheckForInput()
        {
            if (Input.GetMouseButton(0) ||
                Input.touchCount > 0)
            {
                onInputAny?.Invoke();                
                onInputAnyOnce?.Invoke();
                onInputAnyOnce = null;
            }
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
    private static MonoBehaviourMessageForwarder _commonMessageForwarder;

    public static CommonMessageInstace CommonMessageInstance
    {
        get
        {
            if (_commonMessageInstace == null)
            {
                _commonMessageInstace = new CommonMessageInstace();
                var commonMessageForwarder = Create(_commonMessageInstace);
                DontDestroyOnLoad(commonMessageForwarder.gameObject);
                _commonMessageForwarder = commonMessageForwarder;
                // _commonMessageInstace.onDestroy += ()=>{ _commonMessageInstace = null; };
            }

            return _commonMessageInstace;
        }
    }
    public static MonoBehaviourMessageForwarder CommonMessageForwarder
    {
        get
        {
            if (_commonMessageForwarder == null)
            {
                var _ = CommonMessageInstance;
            }

            return _commonMessageForwarder;
        }
    }

    public static void RegisterCommonDestroy(Action onDestroy)
    {
        CommonMessageInstance.onDestroy += onDestroy;
    }

    public static void RegisterCommonAwake(Action onAwake)
    {
        CommonMessageInstance.onAwake += onAwake;
    }

    public static void RegisterCommonStart(Action onStart)
    {
        CommonMessageInstance.onStart += onStart;
    }

    public static void RegisterCommonUpdate(Action onUpdate)
    {
        CommonMessageInstance.onUpdate += onUpdate;
    }
    public static void UnRegisterCommonUpdate(Action onUpdate)
    {
        CommonMessageInstance.onUpdate -= onUpdate;
    }

    /// <summary>
    /// Only work when game is running.
    /// NOT TIME ACCURATE
    /// </summary>
    public static void RegisterCommonClockUpdate(Action onClockUpdate)
    {
        CommonMessageInstance.clockUpdate += onClockUpdate;
    }

    public static void UnRegisterCommonClockUpdate(Action onClockUpdate)
    {
        CommonMessageInstance.clockUpdate -= onClockUpdate;
    }
    
    public static void RegisterCommonPauseEvent(Action<bool> onPause)
    {
        CommonMessageInstance.onApplicationPause += onPause;
    }
#endregion

    public IMonoBevhaviourMessageHandler _targetListener;

    public static MonoBehaviourMessageForwarder Create(IMonoBevhaviourMessageHandler target)
    {
        var forwarder = Create(target.GetType().Name);
        forwarder._targetListener = target;
        return forwarder;
    }

    public static MonoBehaviourMessageForwarder Create(string name = "")
    {
        GameObject go = new GameObject($"{name}_Forwarder");
        MonoBehaviourMessageForwarder forwarder = go.AddComponent<MonoBehaviourMessageForwarder>();
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
