using Anvil.Legacy;
using System;
using UnityEngine;
using IAnimationController = Anvil.Legacy.IAnimationController;
using IMoveController = Anvil.Legacy.IMoveController;

namespace Anvil
{
    public enum effectCallbackAction
    {
        ReturnToPool,
        DisableAwakeLoop,
        Nothing,
    }

    public enum effectAwakeAction
    {
        Nothing,
        Play,
    }

    public interface IEffectHandler
    {
        public Action onEndCallback { get; set; }
        public void Init(EffectController_v0 controllerV0);

        public void Play();

        // public void SetPlayOnAwake(bool state = true);
        // public void OnEnable();
        public void OnEnd();
    }

    //
    [Serializable]
    public class EffectHandler : IEffectHandler
    {
        protected Action _callback;
        protected EffectController_v0 ControllerV0;

        public Action onEndCallback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void Init(EffectController_v0 controllerV0)
        {
            ControllerV0 = controllerV0;
        }

        public virtual void Init(MoveConfig moveConfig, ITargetDesignator targetDesignator)
        {
        }

        public virtual void Play()
        {
        }

        public virtual void OnEnd()
        {
            _callback?.Invoke();
            // _callback = null;
        }

        // public virtual void SetPlayOnAwake(bool state = true)
        // {
        // }
    }

    public class AnimationEffectHandler : EffectHandler
    {
        [SerializeField] private Legacy.IAnimationController _animationController;

        public Legacy.IAnimationController AnimationController => _animationController;

        public override void Init(EffectController_v0 controllerV0)
        {
            _animationController = controllerV0.gameObject.CreateAnimationController();
            if (_animationController == null)
                Debug.LogError("Animation Effect handler init failed. animation component not found");
        }

        public override void Play()
        {
            _animationController.PlayAnimation(AnimationNames.Show, OnEnd);
        }

        // public override void SetPlayOnAwake(bool state)
        // {
        //     base.SetPlayOnAwake(state);
        // }
    }

    public class ParticleEffectHandler : EffectHandler
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public ParticleSystem ParticleSystem => _particleSystem;
        private ParticleSystem.MainModule _mainModule => _particleSystem.main;

        public override void Init(EffectController_v0 controllerV0)
        {
            if (_particleSystem == null) _particleSystem = controllerV0.gameObject.GetComponent<ParticleSystem>();
            if (_particleSystem == null)
                Debug.LogError("Particle Effect handler init failed. Particle system not found");
        }

        public override void Play()
        {
            if (_particleSystem == null)
            {
                return;
            }
            _particleSystem.Play();
        }
    }

    public class MovementEffectHandler : EffectHandler
    {
        IMoveController _moveController;
        public IMoveController MoveController => _moveController;
        private ITargetDesignator _targetDesignator;


        public override void Init(MoveConfig moveConfig, ITargetDesignator targetDesignator)
        {
            _targetDesignator = targetDesignator;
            _moveController = moveConfig.CreateMoveController();
            _moveController.transform = ControllerV0.transform;
            _moveController.Current = ControllerV0.transform.position;

#if UNITY_EDITOR

            Vector3 currentPos = ControllerV0.transform.position;
            void DrawGizmos()
            {
                if (moveConfig == null)
                {
                    return;
                }

                moveConfig.DrawGizmos(currentPos, _targetDesignator.CalculateTargetPosition(), Color.red);
            }
            // Manager.onDrawGizmos += DrawGizmos;
            // Manager.DelayCall(5f, () =>
            // {
            //     Manager.onDrawGizmos -= DrawGizmos;
            // });
#endif

        }

        public override void OnUpdate(float deltaTime)
        {
            if (_moveController == null)
            {
                return;
            }

            _moveController.End = _targetDesignator.CalculateTargetPosition();
            _moveController.Update(deltaTime);
        }

        public override void Play()
        {
            if (_moveController == null)
            {
                return;
            }
            _moveController.Current = ControllerV0.transform.position;
            _moveController.MoveTo(_targetDesignator.CalculateTargetPosition(), OnEnd);
        }
    }
    // public class ScriptedEffectHandler : EffectHandler
    // {
    //     IScriptedFX _scriptedFx;
    //     private Vector3 _targetpos;
    //     public override void Init(EffectController controller)
    //     {
    //         base.Init(controller);
    //         _scriptedFx = controller.gameObject.GetComponent<IScriptedFX>();
    //     }
    //
    //     public override void Play()
    //     {
    //
    //     }
    // }
}
