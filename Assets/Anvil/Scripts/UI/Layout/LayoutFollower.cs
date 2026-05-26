using System;
using Anvil.Legacy;
using UnityEngine;
using UnityEngine.Serialization;
using IMoveController = Anvil.Legacy.IMoveController;

namespace Anvil
{
    public class LayoutFollower : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;
        public float _maxSpeed = 10f;
        IMoveController _moveController;

        [SerializeField] private bool _hasStaticChildIndex = false;
        [SerializeField] private int _staticChildIndex = 0;
        public bool HasStaticChildIndex => _hasStaticChildIndex;
        public int StaticChildIndex => _staticChildIndex;
        // private bool _destinationAchived = false;
        [SerializeField] private bool follow = false;
        public bool IsFollowing => follow;
        private Action _onTargetReach = null;
        public Action _onTargetReachCallback = null;
        public MoveControllerLayoutWraper layoutWraper;
        public IMoveController moveController => _moveController;

        public int Index
        {
            get
            {
                if (target == null)
                {
                    return -1;
                }

                if (HasStaticChildIndex)
                {
                    return StaticChildIndex;
                }

                return target.GetSiblingIndex() - 1;
            }
            set
            {
                if (target == null || HasStaticChildIndex)
                {
                    return;
                }

                SetTargetSiblingIndex(value + 1);
            }
        }
        public void SetTargetSiblingIndex(int index)
        {
            if (target == null)
            {
                return;
            }
            target.SetSiblingIndex(index);
        }
        public void SetStaticChildIndex(int index)
        {
            _staticChildIndex = index;
            Index = index;
            _hasStaticChildIndex = true;
        }
        public void Init(MoveConfig moveConfig = null)
        {
            // smoothTime = duration;
            // _maxSpeed = maxSpeed;

            if (moveConfig == null)
            {
                moveConfig = CommonUIConfig.DefaultMoveConfig;
            }

            _moveController = moveConfig.CreateMoveController();
            _moveController.transform = transform;
            // _moveController.Local = true;
            // _moveController.Local
        }

        public void SetTarget(Transform targetTF)
        {
            if (targetTF == null)
            {
                return;
            }
            target = targetTF;
            targetTF.SetShow(gameObject.activeInHierarchy);
        }

        public void UpdateMoveConfig(MoveConfig newConfig)
        {
            _moveController?.UpdateConfig(newConfig);
        }
        public void StartFollowTarget(Action onFinish = null)
        {
            // Debug.Log("start following target");
            if (!gameObject.activeInHierarchy)
            {
                // TeleportToTarget();
                onFinish?.Invoke();
                return;
            }

            if (IsAtDestination)
            {
                onFinish?.Invoke();
                return;
            }
            follow = true;
            _onTargetReach = onFinish;
            _moveController.MoveTo(target.position, _onTargetReach);
        }

        public void SyncFollowTarget()
        {
            if (target == null)
            {
                return;
            }
            RectTransform targetRTF = target as RectTransform;
            RectTransform rtf = transform as RectTransform;
            if (targetRTF == null || rtf == null)
            {
                Debug.LogError("Canot sync rect size, layout follower only work on ui element with rect transform atttached");
            }
            targetRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rtf.rect.width);
            targetRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rtf.rect.height);
            targetRTF.pivot = rtf.pivot;
        }

        public void StopFollowTarget()
        {
            // Debug.Log("stop following target");
            follow = false;
        }

        public void ForceFinish()
        {
            StopFollowTarget();
            TeleportToTarget();
            _onTargetReach?.Invoke();
            _onTargetReach = null;
        }

        public bool IsAtDestination => Vector3.Distance(transform.position, target.position) < 0.03f;

        private void Update()
        {
            if (!follow || target == null)
            {
                return;
            }

            // if (_moveController.End != target.position)
            // {
                _moveController.End = target.position;
            // }
            _moveController.Update(Time.deltaTime);
            // UpdateMovement(Time.deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (target == null)
            {
                return;
            }

            if (IsAtDestination)
            {
                // transform.position = target.position;
                _onTargetReach?.Invoke();
                _onTargetReach = null;
            }

            Vector3 targetPosition = target.position;
            if (transform.position != targetPosition)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime,
                    _maxSpeed);
                if (IsAtDestination)
                {
                    transform.position = target.position;
                    _onTargetReach?.Invoke();
                    _onTargetReach = null;

                    _onTargetReachCallback?.Invoke();
                }
            }
        }

        public void TeleportToTarget()
        {
            // _moveController.Stop();
            // Debug.Log("teleport to target");
            if (target == null)
            {
                return;
            }
            transform.position = target.position;
        }

        public void OnDisable()
        {
            // ForceFinish();
            // StopFollowTarget();
            // TeleportToTarget();

            if (target != null)
            {
                target.SetShow(false);
            }
        }
        public void OnEnable()
        {
            // if (!IsAtDestination)
            // {
            //     // StartFollowTarget();
            //     follow = true;
            // }
            if (target != null)
            {
                target.SetShow(true);
            }
        }

        //
        // private void OnDrawGizmos()
        // {
        //     string gstring = $"{transform.GetSiblingIndex()};";
        //     if (target != null)
        //     {
        //         gstring += target.transform.GetSiblingIndex().ToString();
        //     }
        //     GizmosHelper.DrawText(gstring, transform.position);
        //
        // }
    }
}
