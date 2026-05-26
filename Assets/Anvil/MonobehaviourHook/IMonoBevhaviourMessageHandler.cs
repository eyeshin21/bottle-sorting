    public interface IMonoBevhaviourMessageHandler
    {
        void OnAwake();
        void OnStart();
        void OnUpdate(float deltaTime);
        void OnEnable();
        void OnDisable();
        void OnDestroy();
        void OnApplicationPause(bool state);
    }
