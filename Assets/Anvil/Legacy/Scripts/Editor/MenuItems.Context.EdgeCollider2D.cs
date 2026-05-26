#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/EdgeCollider2D/Copy Points")]
        static void EdgeCollider2DCopyPoints(MenuCommand menuCommand)
        {
            var collider = menuCommand.To<EdgeCollider2D>();
            EditorContext.SetPoints(collider.points);
        }

        [MenuItem("CONTEXT/EdgeCollider2D/Paste Points")]
        static void EdgeCollider2DPastePoints(MenuCommand menuCommand)
        {
            EdgeCollider2DPastePoints(menuCommand, false);
        }

        [MenuItem("CONTEXT/EdgeCollider2D/Paste Points - Closed")]
        static void EdgeCollider2DPastePointsClosed(MenuCommand menuCommand)
        {
            EdgeCollider2DPastePoints(menuCommand, true);
        }

        static void EdgeCollider2DPastePoints(MenuCommand menuCommand, bool closed)
        {
            var points = EditorContext.Points;
            int count = points.GetCount();
            if (count == 0)
            {
                LegacyLog.Warning("Points required!");
                return;
            }

            var collider = menuCommand.To<EdgeCollider2D>();
            EditorHelper.Set(collider, "Set Points", () =>
            {
                if (closed)
                {
                    if (count > 2)
                    {
                        var startPoint = points[0];
                        var endPoint = points[count - 1];
                        if (!endPoint.Equals(startPoint))
                        {
                            var newPoints = new List<Vector3>(points);
                            newPoints.Add(startPoint);
                            points = newPoints;
                        }
                    }
                }

                if (!collider.SetPoints(points.ToListVector2()))
                {
                    LegacyLog.Warning("Can't set points!");
                }
            });
        }

        [MenuItem("CONTEXT/EdgeCollider2D/Set Closed")]
        static void EdgeCollider2DSetClosed(MenuCommand menuCommand)
        {
            var collider = menuCommand.To<EdgeCollider2D>();
            var points = collider.points;
            int count = points.GetLength();
            if (count > 2)
            {
                var startPoint = points[0];
                var endPoint = points[count - 1];
                if (!endPoint.Equals(startPoint))
                {
                    var newPoints = new List<Vector2>(points);
                    newPoints.Add(startPoint);
                    EditorHelper.Set(collider, "Set Closed", () =>
                    {
                        if (!collider.SetPoints(newPoints))
                        {
                            LegacyLog.Warning($"Can't set closed for {collider.name}!");
                        }
                    });
                }
            }
        }
    }
}
#endif