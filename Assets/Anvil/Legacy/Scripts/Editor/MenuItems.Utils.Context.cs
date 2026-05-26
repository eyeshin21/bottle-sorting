#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        #region Component
        [MenuItem("CONTEXT/Component/Set For Name", false, 10)]
        static void ComponentSetForName(MenuCommand menuCommand)
        {
            var component = menuCommand.To<Component>();
            var name = component.GetType().Name;
            if (component.gameObject.name != name)
            {
                EditorHelper.Set(component, "Set Name", () => component.gameObject.name = name);
            }
        }

        [MenuItem("CONTEXT/Component/Set Sorting Layer", false, 20)]
        static void ComponentSetSortingLayer(MenuCommand menuCommand)
        {
            GUIHelper.ShowSelectSortingLayerID(sortingLayerID =>
            {
                var gameObject = (menuCommand.context as Component).gameObject;
                SortingController.SetSortingLayerID(gameObject, sortingLayerID, out bool changed, true);
                if (changed)
                {
                    gameObject.SetDirty();
                }
            });
        }

        [MenuItem("CONTEXT/Component/Set Sorting Order", false, 20)]
        static void ComponentSetSortingOrder(MenuCommand menuCommand)
        {
            GUIHelper.ShowInputInt("Set Sorting Order", "Sorting Order", 0, sortingOrder =>
            {
                var gameObject = (menuCommand.context as Component).gameObject;
                SortingController.SetSortingOrder(gameObject, sortingOrder);
                gameObject.SetDirty();
            });
        }

        [MenuItem("CONTEXT/Component/Add Sorting Order", false, 20)]
        static void ComponentAddSortingOrder(MenuCommand menuCommand)
        {
            GUIHelper.ShowInputInt("Add Sorting Order", "Delta Order", 0, deltaSortingOrder =>
            {
                if (deltaSortingOrder != 0)
                {
                    var gameObject = (menuCommand.context as Component).gameObject;
                    SortingController.AddSortingOrder(gameObject, deltaSortingOrder, true);
                    gameObject.SetDirty();
                }
            });
        }
        #endregion

        #region Animator
        //[MenuItem("CONTEXT/Animator/Set Speed")]
        //static void SetAnimatorSpeed(MenuCommand menuCommand)
        //{
        //    var animator = menuCommand.To<Animator>();
        //    GUIHelper.ShowInputFloat("Set Speed", "Speed", animator.speed, speed =>
        //    {
        //        animator.speed = speed;
        //    });
        //}
        #endregion
    }
}
#endif