#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugRectTransform : DebugBehaviour
    {
        [SerializeField] Color _color = Color.green;

        string _info;

        public DebugRectTransform SetColor(Color color)
        {
            _color = color;
            return this;
        }

        public override void OnInspectorGUI()
        {
            GUIHelper.InfoBox(_info);
        }

        void OnDrawGizmos()
        {
            var rectTransform = transform as RectTransform;
            if (rectTransform == null) return;

            var pos = transform.position;
            var rect = rectTransform.rect;
            var size = rectTransform.sizeDelta;
            rectTransform.GetAABB(out float left, out float top, out float right, out float bottom);
            _info = $"pos={pos}\nrect={rect}\nsize={size}\naabb=({left}, {top}, {right}, {bottom})";
            GizmosHelper.DrawAABB(left, top, right, bottom, _color);
        }
    }
}
#endif