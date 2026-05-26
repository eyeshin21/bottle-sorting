//#if UNITY_EDITOR
//using UnityEngine;
//using System.Collections.Generic;

//namespace Gametamin
//{
//    public static partial class GUIHelper
//    {
//        static Dictionary<string, GUIContent> _contents = new Dictionary<string, GUIContent>();
//        static Dictionary<string, float> _labelWidths = new Dictionary<string, float>();

//        public static GUIContent GetContent(string s)
//        {
//            if (!_contents.TryGetValue(s, out GUIContent content))
//            {
//                content = new GUIContent(s);
//                _contents.Add(s, content);
//            }
//            return content;
//        }

//        public static float GetLabelWidth(string s)
//        {
//            if (!_labelWidths.TryGetValue(s, out float labelWidth))
//            {
//                _content.text = s;
//                labelWidth = GUI.skin.label.CalcSize(_content).x;
//                _labelWidths.Add(s, labelWidth);
//            }
//            return labelWidth;
//        }

//        public static float GetLabelWidth(GUIContent content)
//        {
//            if (!_labelWidths.TryGetValue(content.text, out float labelWidth))
//            {
//                labelWidth = GUI.skin.label.CalcSize(content).x;
//                _labelWidths.Add(content.text, labelWidth);
//            }
//            return labelWidth;
//        }

//        public static float GetLabelWidth(string label, string label2)
//        {
//            var style = GUI.skin.label;
//            return Mathf.Max(style.CalcSize(new GUIContent(label)).x, style.CalcSize(new GUIContent(label2)).x);
//        }

//        public static float GetLabelWidth(string label, string label2, string label3)
//        {
//            var style = GUI.skin.label;
//            return Mathf.Max(style.CalcSize(new GUIContent(label)).x, Mathf.Max(style.CalcSize(new GUIContent(label2)).x, style.CalcSize(new GUIContent(label3)).x));
//        }

//        public static void GetContentAndLabelWidth(string s, out GUIContent content, out float labelWidth)
//        {
//            content = GetContent(s);
//            labelWidth = GetLabelWidth(s);
//        }
//    }
//}
//#endif