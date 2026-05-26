using UnityEngine;
using System.Collections;

namespace Anvil.Legacy.Actions
{
    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateFixedUpdate,
        LateUpdate
    }

    public enum DeltaTimeType
    {
        Scaled,
        Unscaled,
        UnscaledIfGreaterThanZero
    }

    public enum FinishType
    {
        None,
        DestroyAction,
        DestroyGameObject
    }

    public class ActionBehaviour : MonoBehaviour
    {
        private ActionX _action;
        private float _speed = 1f;
        private DeltaTimeType _deltaTimeType = DeltaTimeType.Scaled;
        private FinishType _finishType = FinishType.DestroyAction;
        private bool _isFinished = true;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float Duration => _action.Duration;
        public bool Finished => _isFinished;

        public string Name { get; set; }
        public string Tag { get; set; }

        static WaitForFixedUpdate _waitForFixedUpdate;
        static WaitForFixedUpdate WaitForFixedUpdate => _waitForFixedUpdate ??= new WaitForFixedUpdate();

        public void Play(ActionX action)
        {
            _action = action;
            _isFinished = false;
            _action.Play();

            CheckFinished();
        }

        public void Replay()
        {
            _isFinished = false;
            _action.Replay();

            CheckFinished();
        }

        public void Stop(bool forceEnd = false)
        {
            if (!_isFinished)
            {
                _action.Stop(forceEnd);
                CheckFinished();
            }
        }

        public ActionBehaviour SetDeltaTimeType(DeltaTimeType deltaTimeType)
        {
            _deltaTimeType = deltaTimeType;
            return this;
        }

        public ActionBehaviour SetFinishType(FinishType finishType)
        {
            _finishType = finishType;
            return this;
        }

        public void ForceUpdate(float deltaTime)
        {
            if (!_isFinished)
            {
                OnUpdate(deltaTime);
            }
        }

        void OnUpdate()
        {
            if (!_isFinished)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        void OnUpdate(float deltaTime)
        {
            if (_deltaTimeType == DeltaTimeType.Unscaled)
            {
                float timeScale = Time.timeScale;
                if (timeScale > 0)
                {
                    deltaTime /= timeScale;
                }
                else
                {
                    deltaTime = Time.unscaledDeltaTime;
                }
            }
            else if (_deltaTimeType == DeltaTimeType.UnscaledIfGreaterThanZero)
            {
                float timeScale = Time.timeScale;
                if (timeScale > 0)
                {
                    deltaTime /= timeScale;
                }
                else
                {
                    return;
                }
            }

            _action.Update(deltaTime * _speed);

            CheckFinished();
        }

        void CheckFinished()
        {
            if (_action.Finished)
            {
                _isFinished = true;

                if (_finishType == FinishType.DestroyAction)
                {
                    Destroy(this);
                }
                else if (_finishType == FinishType.DestroyGameObject)
                {
                    Destroy(gameObject);
                }
            }
        }

        #region Classes
        public class UpdateAction : ActionBehaviour
        {
            void Update()
            {
                OnUpdate();
            }
        }

        public class FixedUpdateAction : ActionBehaviour
        {
            void FixedUpdate()
            {
                OnUpdate();
            }
        }

        public class LateFixedUpdateAction : ActionBehaviour
        {
            void Start()
            {
                StartCoroutine(LateFixedUpdate());
            }

            IEnumerator LateFixedUpdate()
            {
                yield return WaitForFixedUpdate;
                OnUpdate();
            }
        }

        public class LateUpdateAction : ActionBehaviour
        {
            void LateUpdate()
            {
                OnUpdate();
            }
        }
        #endregion
    }
}