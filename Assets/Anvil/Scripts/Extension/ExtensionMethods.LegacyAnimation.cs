using System;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static void PlayLegacyAnimation(this GameObject gameObject, string animationName)
        {
            var animation = gameObject.GetComponent<Animation>();
            if (animation == null)
            {
                Debug.LogError("No Animation Component");
                return;
            }
            if (animation.GetClip(animationName) == null)
            {
                Debug.LogError($"No Animation Clip with name {animationName}");
                return;
            }
            animation.Play(animationName);
        }
        public static void PlayLegacyAnimation(this GameObject gameObject)
        {
            if (!gameObject)
            {
                return;
            }
            var animation = gameObject.GetComponent<Animation>();
            if (animation == null)
            {
                Debug.LogWarning($"[{gameObject.name}] No Animation Component");
                return;
            }

            if (animation.isPlaying)
            {
                animation.Stop();
            }
            animation.Play(PlayMode.StopAll);
        }
        public static void PlayLegacyAnimation(this GameObject gameObject, string animaitonName, Action callback)
        {
            Legacy.AnimationControllerWraper animationController = gameObject.GetOrAddComponentSafe<Legacy.AnimationControllerWraper>();
            animationController.Init();
            animationController.PlayAnimation(animaitonName, callback);
        }
    }
}
