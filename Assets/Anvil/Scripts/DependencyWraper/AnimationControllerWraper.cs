// using System;
// using Anvil.Legacy.Legacy;
// using UnityEngine;
//
// namespace Anvil
// {
//     public class AnimationControllerWraper : MonoBehaviour
//     {
//         private IAnimationController _animationController;
//
//         public IAnimationController IAnimationController => _animationController;
//
//         private bool _inited = false;
//         private void Awake()
//         {
//             Init();
//         }
//
//         public void Init()
//         {
//             if (_inited)
//             {
//                 return;
//             }
//
//             _animationController = gameObject.CreateAnimationController();
//             _inited = true;
//         }
//
//         public void PlayAnimation(string animName)
//         {
//             _animationController.PlayAnimation(animName);
//         }
//
//         public void PlayAnimation(string animName, bool loop)
//         {
//             _animationController.PlayAnimation(animName, loop);
//         }
//
//         public void PlayAnimation(string animName, Action callback)
//         {
//             _animationController.PlayAnimation(animName, callback);
//         }
//
//         public void PlayAnimations(string animName, string name2, bool loop = false)
//         {
//             _animationController.PlayAnimations(animName, name2, loop);
//         }
//
//         public void PlayAnimations(string animName, string name2, string name3, bool loop = false)
//         {
//             _animationController.PlayAnimations(animName, name2, name3, loop);
//         }
//
//         public void AddEvent(string eventName, Action callback)
//         {
//             _animationController.AddEvent(eventName, callback);
//         }
//
//         public void AddEvent(string eventName, Action<int> callback)
//         {
//             _animationController.AddEvent(eventName, callback);
//         }
//
//         public void ForcePlayAnimation(string animName)
//         {
//             _animationController.ForcePlayAnimation(animName);
//         }
//
//         public void ForceEndAnimation()
//         {
//             _animationController.ForceEndAnimation();
//         }
//
//         public void SetSpeed(float speed)
//         {
//             _animationController.SetSpeed(speed);
//         }
//
//         public void SetShow(bool show)
//         {
//             _animationController.SetShow(show);
//         }
//
//         public void SetPosition(Vector3 pos)
//         {
//             _animationController.SetPosition(pos);
//         }
//
//         public void OnShow(Action onComplete = null)
//         {
//             PlayAnimation(AnimationNames.Show, onComplete);
//         }
//
//         public void OnHide(Action onComplete = null)
//         {
//             PlayAnimation(AnimationNames.Hide, onComplete);
//         }
//
//         public void OnSetShow()
//         {
//             throw new NotImplementedException();
//         }
//
//         public void OnSetHide()
//         {
//             throw new NotImplementedException();
//         }
//     }
// }
