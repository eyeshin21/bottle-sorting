// #if UNITY_EDITOR
// using Anvil;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(CurvePreview))]
// public class CurvePreviewEditor : Editor
// {
//     void OnSceneGUI()
//     {
//         var c = (CurvePreview)target;
//         if (!c.enabled) return;
//
//         Handles.color = Color.yellow;
//
//         EditorGUI.BeginChangeCheck();
//         c._handlePos1 = Handles.PositionHandle(c._handlePos1, Quaternion.identity);
//         c._handlePos2 = Handles.PositionHandle(c._handlePos2, Quaternion.identity);
//         if (EditorGUI.EndChangeCheck())
//         {
//             Undo.RecordObject(c, "Move Curve Handle");
//             c.ApplyHandlesToParam();
//             EditorUtility.SetDirty(c);
//         }
//     }
// }
// #endif