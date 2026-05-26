#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/PolygonCollider2D/Copy Points")]
        static void PolygonCollider2DCopyPoints(MenuCommand menuCommand)
        {
            var collider = menuCommand.To<PolygonCollider2D>();
            int pathCount = collider.pathCount;
            if (pathCount == 0)
            {
                LegacyLog.Warning("Missing path!");
                return;
            }

            if (pathCount == 1)
            {
                EditorContext.SetPoints(collider.GetPath(0));
            }
            else
            {
                GUIHelper.ShowInputInt("Copy Points", $"Path ({pathCount})", 1, path =>
                {
                    if (path > 0 && path <= pathCount)
                    {
                        EditorContext.SetPoints(collider.GetPath(path - 1));
                        return true;
                    }
                    return false;
                });
            }
        }

        [MenuItem("CONTEXT/PolygonCollider2D/Paste Points")]
        static void PolygonCollider2DPastePoints(MenuCommand menuCommand)
        {
            var points = EditorContext.Points;
            if (points.IsNullOrEmpty())
            {
                LegacyLog.Warning("Points required!");
                return;
            }

            var collider = menuCommand.To<PolygonCollider2D>();
            int pathCount = collider.pathCount;
            if (pathCount == 0)
            {
                EditorHelper.Set(collider, "Add Points", () => collider.points = points.ToArrayVector2());
            }
            else if (pathCount == 1)
            {
                EditorHelper.Set(collider, "Set Points", () => collider.SetPath(0, points.ToArrayVector2()));
            }
            else
            {
                GUIHelper.ShowInputInt("Paste Points", $"Path ({pathCount})", 1, path =>
                {
                    if (path > 0 && path <= pathCount)
                    {
                        EditorHelper.Set(collider, $"Set Points - Path {path}", () => collider.SetPath(path - 1, points.ToArrayVector2()));
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}
#endif