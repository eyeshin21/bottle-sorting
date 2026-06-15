using System;
using System.Collections;
using System.Collections.Generic;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class TutorialHand : MonoBehaviour
    {
        private enum State
        {
            None,
            Swipe,
            Tap,
        }
        
        private IAnimationController _animationController;
        private DiscreteObjectDynamicComponent _objectDynamic;
        [SerializeField] private float _moveDuration = 0.5f;
        [SerializeField] private float _moveDelay = 0.5f;
        [SerializeField] private float _showDelay = 0.5f;
        private bool _skipDelay = false;

        private Action _onCycleStartCallback;
        private Action _onCycleFinishCallback;

        private void Awake()
        {
            _animationController = gameObject.CreateAnimationController();
            if (_objectDynamic == null)
            {
                _objectDynamic = gameObject.GetComponent<DiscreteObjectDynamicComponent>();
            }
        }

        private void OnEnable()
        {
            ForceReset();
        }

        private State _currentState = State.None;
        private List<Vector3> positions = new List<Vector3>();
        private bool _active = false;
        private bool _cycleActive = false;
        private float _cycleResetTime;
        private Action _callback;
        private bool _loop = false;

        private bool CycleActive
        {
            get { return _cycleActive; }
            set
            {
                _cycleActive = value;
                if (!_cycleActive)
                {
                    _cycleResetTime = _moveDelay;
                }
            }
        }

        private void Update()
        {
            if (_cycleActive || !_active)
            {
                return;
            }

            _cycleResetTime -= Time.deltaTime;
            if (_cycleResetTime <= 0)
            {
                if (_currentState == State.Swipe)
                {
                    StartCoroutine(StartCycle());
                }else if (_currentState == State.Tap)
                {
                    StartCoroutine(StartTapCycle());
                }
            }
        }

        private IEnumerator StartTapCycle()
        {
            CycleActive = true;
            if (_onCycleStartCallback != null)
            {
                _onCycleStartCallback.Invoke();
            }

            bool step = false;
            PlayAnimation(AnimationNames.Show, () =>
            {
                step = true;
            });
            yield return new WaitUntil(() => step);
            step = false;
            PlayAnimation(AnimationNames.Hide, () =>
            {
                step = true;
            });
            yield return new WaitUntil(() => step);
            if (_onCycleFinishCallback != null)
            {
                _onCycleFinishCallback.Invoke();
            }
            CycleActive = false;
        }

        private IEnumerator StartCycle()
        {
            _cycleActive = true;
            transform.position = positions[0];
            float showDelay = _skipDelay ? 0 : _showDelay;
            float moveDelay = _skipDelay ? 0 : _moveDelay;
            yield return new WaitForSeconds(showDelay);

            bool shown = false;
            PlayAnimation(AnimationNames.Show, () => { shown = true; });
            yield return new WaitUntil(() => shown);
            yield return new WaitForSeconds(moveDelay);
            _onCycleStartCallback?.Invoke();
            PlayAnimation(AnimationNames.Move);
            Move(positions, _moveDuration, 0, () =>
            {
                PlayAnimation(AnimationNames.Hide, () =>
                {
                    _onCycleFinishCallback?.Invoke();
                    CycleActive = false;
                });
            });
        }

        public void Move(List<Vector3> positions, float duration, float delay = 0, Action callback = null)
        {
            _objectDynamic.Reset();
            _objectDynamic.DesiredMoveTime = duration;

            if (delay <= 0)
            {
                Move(positions, callback);
                return;
            }

            _cycleActive = true;
            PlayAnimation(AnimationNames.Show,
                () => { Move(positions, () => { PlayAnimation(AnimationNames.Hide, callback); }); });
        }

        private void Move(List<Vector3> positions, Action callback)
        {
            _objectDynamic.MoveTo(positions, callback);
        }

        public void PlayAnimation(string animationName, Action callback = null)
        {
            if (_animationController == null)
            {
                callback?.Invoke();
                return;
            }

            _animationController.PlayAnimation(animationName, callback);
        }

        public void ForceReset()
        {
            PlayAnimation(AnimationNames.Hidden);
            _objectDynamic.StopMovement();
            _active = false;
            _onCycleStartCallback = null;
        }

        public void StartSwipe(List<Vector3> paramPositions, Action onCycleStartCallback = null, Action onCycleFinish = null,
            bool skipDelay = false)
        {
            _currentState = State.Swipe;
            _skipDelay = skipDelay;
            positions = paramPositions;
            _active = true;
            CycleActive = false;

            _onCycleStartCallback = onCycleStartCallback;
            _onCycleFinishCallback = onCycleFinish;
        }

        public void ShowTap(Vector3 position)
        {
            _currentState = State.Tap;
            transform.position = position;
            _active = true;
            CycleActive = false;
        }
    }
}