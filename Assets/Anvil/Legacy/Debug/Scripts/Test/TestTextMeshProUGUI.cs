#if DEBUG_MODE
using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public class TestTextMeshProUGUI : TestBehaviour
    {
        [SerializeField] TextMeshProUGUI _textMeshPro;
        [SerializeField] string _text;
        [SerializeField] int _characterStartIndex;
        [SerializeField] int _characterEndIndex;
        [SerializeField] Color _drawColor = Color.blue;
        //[SerializeField] float _drawDuration = 1;

        AABB _aabb = new();
        bool _isDrawAABB;

        public override void OnInspectorGUI()
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = guiEnabled && _textMeshPro != null && !string.IsNullOrEmpty(_text);
            if (GUIHelper.Button("Test AABB"))
            {
                string text = _textMeshPro.text;
                int startIndex = text.IndexOf(_text);
                if (startIndex >= 0)
                {
                    int endIndex = startIndex + _text.Length - 1;
                    _textMeshPro.GetAABB(startIndex, endIndex, out float left, out float top, out float right, out float bottom);
                    //Log.Debug($"({left}, {top}, {right}, {bottom})");
                    //DebugHelper.DrawAABB(left, top, right, bottom, _drawColor, _drawDuration);
                    _aabb.Construct(left, top, right, bottom);
                    _isDrawAABB = true;
                }
                else
                {
                   LegacyLog.Warning($"Text \"{_text}\" not found!");
                    _isDrawAABB = false;
                }
            }
            GUI.enabled = guiEnabled;
        }

        void OnDrawGizmos()
        {
            if (_isDrawAABB)
            {
                _aabb.DrawGizmos(_drawColor);
                RepaintGizmos();
            }
        }
    }
}
#endif