using UnityEditor;
using UnityEngine;

namespace Anvil.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class AtlasIndexOverlayWindow : EditorWindow
    {
        private Texture2D atlas;
        private int columns = 8;
        private int rows = 8;

        private Vector2 scroll;

        private float zoom = 1f;
        private const float MinZoom = 0.25f;
        private const float MaxZoom = 4f;

        private GUIStyle indexStyle;

        [MenuItem("Tools/Atlas Index Overlay")]
        public static void Open()
        {
            GetWindow<AtlasIndexOverlayWindow>("Atlas Index Overlay");
        }

        private void OnEnable()
        {
            indexStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 12,
                normal =
                {
                    textColor = new Color(1f, 0.9f, 0.2f, 0.95f)
                }
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Atlas Settings", EditorStyles.boldLabel);

            atlas = (Texture2D)EditorGUILayout.ObjectField(
                "Atlas Texture", atlas, typeof(Texture2D), false);

            columns = Mathf.Max(1, EditorGUILayout.IntField("Columns", columns));
            rows = Mathf.Max(1, EditorGUILayout.IntField("Rows", rows));

            GUILayout.Space(6);

            zoom = EditorGUILayout.Slider("Zoom", zoom, MinZoom, MaxZoom);

            GUILayout.Space(8);

            if (atlas == null)
            {
                EditorGUILayout.HelpBox("Assign a texture atlas to begin.", MessageType.Info);
                return;
            }

            HandleZoomEvents();

            scroll = EditorGUILayout.BeginScrollView(scroll);

            float width = atlas.width * zoom;
            float height = atlas.height * zoom;

            Rect atlasRect = GUILayoutUtility.GetRect(
                width,
                height,
                GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false)
            );

            GUI.DrawTexture(atlasRect, atlas, ScaleMode.StretchToFill);

            DrawIndices(atlasRect);

            EditorGUILayout.EndScrollView();
        }

        private void HandleZoomEvents()
        {
            Event e = Event.current;

            if (e.type == EventType.ScrollWheel)
            {
                float zoomDelta = -e.delta.y * 0.03f;
                float oldZoom = zoom;

                zoom = Mathf.Clamp(zoom + zoomDelta, MinZoom, MaxZoom);

                if (!Mathf.Approximately(oldZoom, zoom))
                {
                    e.Use();
                    Repaint();
                }
            }
        }

        private void DrawIndices(Rect atlasRect)
        {
            float cellWidth = atlasRect.width / columns;
            float cellHeight = atlasRect.height / rows;

            int index = 0;

            // Scale font with zoom (clamped so it doesn't go feral)
            indexStyle.fontSize = Mathf.Clamp(Mathf.RoundToInt(12 * zoom), 8, 32);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Rect cellRect = new Rect(
                        atlasRect.x + x * cellWidth,
                        atlasRect.y + y * cellHeight,
                        cellWidth,
                        cellHeight
                    );

                    // Shadow
                    Rect shadowRect = cellRect;
                    shadowRect.x += 1;
                    shadowRect.y += 1;

                    GUI.Label(shadowRect, index.ToString(), EditorStyles.boldLabel);
                    GUI.Label(cellRect, index.ToString(), indexStyle);

                    index++;
                }
            }
        }
    }
}