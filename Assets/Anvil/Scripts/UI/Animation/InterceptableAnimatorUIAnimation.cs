// using Gametamin;
// using Gametamin.Core;
// using System;
// using UnityEngine;
//
// namespace MergeGame
// {
//     public class InterceptableAnimatorUIAnimation : UIAnimation
//     {
//         [SerializeField]private Animator _animator;
//         private AnimationEndEvent _animationEndEvent;
//         private void Awake()
//         {
//             _animator = gameObject.GetComponentSafe<Animator>();
//         }
//         public override void PlayAnimation(string name)
//         {
//             PlayAnimation(name, null);
//         }
//         public override void PlayAnimation(string name, Action callback)
//         {
//             if (_animator != null && _animator.runtimeAnimatorController!= null)
//             {
//                 if (_animator.gameObject.activeInHierarchy)
//                 {
//                     if (_animationEndEvent == null)
//                     {
//                         _animationEndEvent = _animator.gameObject.AddComponent<AnimationEndEvent>();
//                     }
//                     _animationEndEvent.PlayAnimation(name, callback);
//                 }
//                 else
//                 {
//                     Debug.Log($"animator inactive");
//                     //LogInactive(_animator.gameObject);
//                     callback?.Invoke();
//                 }
//             }
//             else
//             {
//                 Debug.Log($"[UIAnimator] cannot play animaiton {name}");
//                 callback?.Invoke();
//             }
//         }
//
//         public override void OnShow(Action onComplete = null)
//         {
//             PlayAnimation(AnimationNames.Show, onComplete);
//         }
//     }
// }

