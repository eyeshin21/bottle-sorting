// using Gametamin;
// using Gametamin.Core;
// using System;
// using UnityEngine;
//
// namespace MergeGame
// {
//     public class UIAnimation : MonoBehaviour, IUIAnimationController, IShowHideAnimation, IPlayAnimation
//     {
//         public virtual void PlayAnimation(string name)
//         {
//             PlayAnimation(name, null);
//         }
//         public virtual void PlayAnimation(string name, Action callback)
//         {
//
//         }
//
//         public void PlayShowAnimation(Action callback = null)
//         {
//             PlayAnimation(AnimationNames.Show, callback);
//         }
//
//         public void PlayHideAnimation(Action callback = null)
//         {
//             PlayAnimation(AnimationNames.Hide, callback);
//         }
//
//         public virtual void OnSetShow()
//         {
//             throw new NotImplementedException();
//         }
//
//         public virtual void OnSetHide()
//         {
//             throw new NotImplementedException();
//         }
//
//         public virtual void OnShow(Action onComplete = null)
//         {
//             PlayShowAnimation(onComplete);
//         }
//
//         public virtual void OnHide(Action onComplete = null)
//         {
//             PlayHideAnimation(onComplete);
//         }
//     }
// }

