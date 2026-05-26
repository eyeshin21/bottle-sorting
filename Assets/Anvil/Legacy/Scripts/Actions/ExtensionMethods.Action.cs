using Anvil.Legacy.Actions;
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        static ActionBehaviour AddActionBehaviour(GameObject go, UpdateType updateType)
        {
            if (updateType == UpdateType.Update) return go.AddComponent<ActionBehaviour.UpdateAction>();
            if (updateType == UpdateType.FixedUpdate) return go.AddComponent<ActionBehaviour.FixedUpdateAction>();
            if (updateType == UpdateType.LateFixedUpdate) return go.AddComponent<ActionBehaviour.LateFixedUpdateAction>();
            if (updateType == UpdateType.LateUpdate) return go.AddComponent<ActionBehaviour.LateUpdateAction>();

            Assert.Todo(updateType);
            return null;
        }

        #region GameObject
        public static ActionBehaviour PlayAction(this GameObject go, ActionX action, UpdateType updateType = UpdateType.Update)
        {
            if (go != null)
            {
                var actionBehaviour = AddActionBehaviour(go, updateType);
                actionBehaviour.Play(action);
                return actionBehaviour;
            }
            return default;
        }

        public static void StopAction(this GameObject go, bool forceEnd = false)
        {
            if (go != null)
            {
                var action = go.GetComponent<ActionBehaviour>();
                if (action != null)
                {
                    action.Stop(forceEnd);
                }
            }
        }

        public static void StopAction(this GameObject go, string actionName, bool forceEnd = false)
        {
            if (go != null)
            {
                go.transform.ForEachComponent<ActionBehaviour>(action =>
                {
                    if (action.Name == actionName)
                    {
                        action.Stop(forceEnd);
                        return false;
                    }
                    return true;
                });
            }
        }

        public static void StopAllActions(this GameObject go, bool forceEnd = false)
        {
            if (go != null)
            {
                var actions = go.GetComponents<ActionBehaviour>();
                foreach (var action in actions)
                {
                    action.Stop(forceEnd);
                }
            }
        }

        public static void DelayCall(this GameObject go, float delay, Callback callback)
        {
            if (go != null)
            {
                if (delay > 0)
                {
                    if (callback != null)
                    {
                        var action = DelayCallFunc.Create(delay, callback);
                        PlayAction(go, action);
                        return;
                    }
                }
            }
            callback?.Invoke();
        }
        #endregion


        #region Component
        public static ActionBehaviour PlayAction(this Component component, ActionX action, UpdateType updateType = UpdateType.Update)
        {
            return PlayAction(component?.gameObject, action, updateType);
        }

        public static void StopAction(this Component component, bool forceEnd = false)
        {
            StopAction(component?.gameObject, forceEnd);
        }

        public static void StopAction(this Component component, string actionName, bool forceEnd = false)
        {
            StopAction(component?.gameObject, actionName, forceEnd);
        }

        public static void StopAllActions(this Component component, bool forceEnd = false)
        {
            StopAllActions(component?.gameObject, forceEnd);
        }

        public static void DelayCall(this Component component, float delay, Callback callback)
        {
            DelayCall(component?.gameObject, delay, callback);
        }
        #endregion
    }
}