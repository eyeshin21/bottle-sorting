#region
using System;
using Anvil.Legacy;
using UnityEngine;
using IMoveController = Anvil.Legacy.IMoveController;

#endregion

namespace Anvil
{
    public class MarioWonderCoinFx : MonoBehaviour,IScriptedFX
    {
        [SerializeField] private GameObject _spriteObject;
        [SerializeField] private MoveConfig _moveConfig = new MoveConfig();
        private float _delayedCallback = 0f;
        ISpriteAdapter _spriteAdapter;
        Legacy.IAnimationController _animationController;
        IMoveController _moveController;
        ITargetDesignator _targetDesignator;

        private void Awake()
        {
            _spriteAdapter = SpriteAdapter.Create(_spriteObject);
            _animationController = gameObject.CreateAnimationController();
            _moveController = _moveConfig.CreateMoveController();
            _moveController.transform = transform;
        }

        public void Init(Vector3 start,ITargetDesignator endDesignator,Sprite sprite,MoveConfig moveConfig = null,float delayedCallback = 0f)
        {
            if (sprite != null)
            {
                _spriteAdapter.Sprite = sprite;
            }

            transform.position = new Vector3(start.x,start.y,transform.position.z);
            _targetDesignator = endDesignator;
            if (moveConfig != null)
            {
                _moveConfig = moveConfig;
                _moveController = _moveConfig.CreateMoveController();
                _moveController.transform = transform;
            }

            _delayedCallback = delayedCallback;
        }

        private void Update()
        {
            if (_moveController.Finished)
            {
                return;
            }

            _moveController.End = (Vector2)_targetDesignator.CalculateTargetPosition();
            _moveController.Update(Time.deltaTime);
        }

        public IScriptedFX Construct(IScriptedFxParam param)
        {
            throw new NotImplementedException();
        }

        public void Play(Action callback)
        {
            // _animationController.PlayAnimation(AnimationNames.Start,
            _animationController.PlayAnimation(AnimationNames.Start,
                ()=>
                {
                    _animationController.PlayAnimation("Fly");
                    _moveController.MoveTo(_targetDesignator.CalculateTargetPosition(),()=>
                    {
                        if (_delayedCallback > 0)
                        {
                            Manager.DelayCall(_delayedCallback,()=>
                            {
                                ReturnToPool();
                                callback?.Invoke();
                            });
                            return;
                        }

                        ReturnToPool();
                        callback?.Invoke();
                    });
                });
        }

        private void ReturnToPool()
        {
            foreach (Transform childTF in transform)
            {
                var Fx = childTF.GetComponent<EffectController_v0>();
                if (Fx != null)
                {
                    Fx.transform.SetParent(null);
                    Fx.DelayReturnToPool();
                }
            }

            GameObjectPool.RemoveObject(gameObject);
        }
    }
}