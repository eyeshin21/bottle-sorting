using System;
using Anvil.Legacy;
using UnityEngine;
using IMoveController = Anvil.Legacy.IMoveController;

namespace Anvil
{
    // Todo: Will be converted to a effect handler after the effect handler/effect controller system is updated
    public class AnimatedMovableObjectCarrier : ObjectCarrier
    {
        private Legacy.IAnimationController _animationController;

        private IMoveController _moveController;
        private string _startAnimName;
        private string _endAnimName;

        private void Awake()
        {
            _animationController = gameObject.CreateAnimationController();
        }

        private Vector3 _targetPos;
        protected FxParams _fxParams;

        public void Init(GameObject obj,Vector3 target,FxParams fxParams = null, MoveConfig moveConfig = null)
        {
            transform.position = obj.transform.position;
            obj.transform.SetParent(_parentTF);
            obj.transform.localPosition = Vector3.zero;
            _targetPos = target;


            if (moveConfig == null)
            {
                //TODO
                moveConfig = new MoveConfig();
            }

            MoveConfig copied = moveConfig.Clone();
            if (fxParams != null)
            {
                copied.SetDuration(fxParams.duration);
            }
            _moveController = copied.CreateMoveController();
            _moveController.transform = transform;
            _moveController.Current = transform.position;

            // if (fxParams == null)
            // {
            //     fxParams = FxConfig.GetFxParams(CollectFxType.Default);
            // }

            _fxParams = fxParams;
        }

        public void Play(Action callback = null, Action moveCallback = null,string startAnimName = "",string endAnimName = "")
        {
            // MoveConfig copied = GameConfig.ItemRewardMoveConfig.Clone();
            // copied.SetDuration(_fxParams.duration);
            // _moveController.UpdateConfig(copied);

            _startAnimName = startAnimName;
            _endAnimName = endAnimName;
            StartMove(()=>
            {
                moveCallback?.Invoke();
                PlayAnimation(_endAnimName,()=>
                {
                    callback?.Invoke();
                    GameObjectPool.RemoveObject(this.gameObject);
                });
            });
        }

        private void StartMove(Action callback)
        {
            // MoveConfig tempConfig = UIConfig.GetCopyOfMoveConfig(UIMovementType.LinearCoin);
            // tempConfig.SetDuration(_fxParams.duration);
            // _moveController.UpdateConfig(tempConfig);

            if ((_fxParams != null && _fxParams.delayBetwenMove <= 0) || _fxParams == null)
            {
                    PlayAnimation(_startAnimName);
                _moveController.MoveTo(_targetPos,callback);
            }
            else
            {
                Manager.DelayCall(_fxParams.delayBetwenMove,()=>
                {
                        PlayAnimation(_startAnimName);
                    _moveController.MoveTo(_targetPos,callback);
                });
            }
        }

        public void PlayAnimation(string animationName,Action callback = null)
        {
            if (animationName.IsNullOrEmpty() || _animationController == null)
            {
                callback?.Invoke();
                return;
            }
            _animationController.PlayAnimation(animationName,callback);
        }

        private void Update()
        {
            _moveController?.Update(Time.deltaTime);
        }
    }
}
