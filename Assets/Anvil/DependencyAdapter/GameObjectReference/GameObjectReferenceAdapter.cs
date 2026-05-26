using System;
using UnityEditor;
using UnityEngine;
using Anvil.Legacy;

namespace Anvil
{
    public interface IGameObjectReferenceAdapter
    {
        GameObject GetGameObjectReference(string refID);
        GameObject gameObject { get; }
    }   
    public class GameObjectReferenceAdapter : IGameObjectReferenceAdapter
    {
        public static IGameObjectReferenceAdapter Create(GameObject gameObject)
        {
            return null;
        }
        
        GameObject _gameObject;
        public virtual GameObject GetGameObjectReference(string refID)
        {
            return null;
        }
        public virtual GameObject gameObject => _gameObject;

        public void LogError()
        {
            Debug.LogError("[GameObjectReferenceAdapter] GameObjectReference not found, the default adapter will not work as expected");
        }
    }
    public static partial class ExtensionMethod
    {
        public static IGameObjectReferenceAdapter CreateGameObjectReferenceAdapter(this GameObject root)
        {
            if (root == null)
            {
                return null;
            }

            return GameObjectReferenceAdapter.Create(root);
        }
        public static T GetComponent<T>(this IGameObjectReferenceAdapter adapter) where T : Component
        {
            if (adapter == null)
            {
                return null;
            }
            if (adapter.gameObject == null)
            {
                return null;
            }
            var component = adapter.gameObject.GetComponent<T>();
            return component;
        }
        public static T GetComponentReference<T>(this IGameObjectReferenceAdapter adapter, string refID) where T : Component
        {
            if (adapter == null)
            {
                return null;
            }
            GameObject gameObject = adapter.GetGameObjectReference(refID);
            if (gameObject == null)
            {
                return null;
            }
            var component = gameObject.GetComponent<T>();
            return component;
        }
        public static GameObject GetGameObjectReference<T>(this IGameObjectReferenceAdapter adapter, string refID)
        {
            if (adapter == null)
            {
                return null;
            }
            GameObject gameObject = adapter.GetGameObjectReference(refID);
            return gameObject;
        }
        public static void SetActiveGameObjectReference(this  IGameObjectReferenceAdapter adapter, string refID, bool state)
        {
            if (adapter == null)
            {
                return;
            }
            GameObject gameObject = adapter.GetGameObjectReference(refID);
            if (gameObject != null)
            {
                gameObject.SetActive(state);
            }
        }
        public static void PlayAnimation(this  IGameObjectReferenceAdapter adapter, string animationName, Action callback)
        {
            if (adapter == null)
            {
                return;
            }

            GameObject gameObject = adapter.gameObject;
            gameObject.PlayAnimation(animationName, callback);
        }        
        public static void PlayShowAnimation(this IGameObjectReferenceAdapter adapter, Action callback)
        {
            if (adapter == null)
            {
                return;
            }
#if GAMETAMIN_CORE
            
#else
            adapter.PlayAnimation(AnimationNames.Show, callback);
#endif
        }
        public static void PlayHideAnimation(this IGameObjectReferenceAdapter adapter, Action callback)
        {
            if (adapter == null)
            {
                return;
            }
#if GAMETAMIN_CORE
            
#else
            adapter.PlayAnimation(AnimationNames.Hide, callback);
#endif
        }
    }
}