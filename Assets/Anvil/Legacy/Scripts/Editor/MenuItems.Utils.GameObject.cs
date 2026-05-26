#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("GameObject/Gametamin/Utils/Set Name By Sprite", false, UtilitiesPriority)]
        static void SetNameBySprite()
        {
            var selectedGameObjects = SelectedGameObjects;
            if (selectedGameObjects != null)
            {
                foreach (var go in selectedGameObjects)
                {
                    go.ForEachTransform(SetNameBySprite);
                }
            }
        }
    }
}
#endif