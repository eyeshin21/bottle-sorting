using UnityEngine;

namespace Anvil.Legacy
{
    public class GUIButton
    {
        GUIContent _content;
        GUILayoutOption _width;

        public GUIButton(string text)
        {
            _content = new GUIContent(text);
            _width = GUILayout.Width(GUIHelper.GetButtonWidth(_content));
        }

        public bool OnGUI()
        {
            return GUIHelper.Button(_content, _width);
        }
    }
}