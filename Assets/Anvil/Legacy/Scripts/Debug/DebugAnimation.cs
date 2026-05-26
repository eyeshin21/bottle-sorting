#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugAnimation : DebugBehaviour
    {
        [SerializeField] string _animationName;
        [SerializeField, OnValueChanged("OnAnimationSpeedChanged")] float _animationSpeed = 1;
        [SerializeField] bool _alwaysSetShow;

        void PlayAnimation(string name)
        {
            if (_alwaysSetShow)
            {
                gameObject.SetShow(true);
            }
            gameObject.PlayAnimation(name);
        }

        void OnAnimationSpeedChanged()
        {
            gameObject.SetAnimationSpeed(_animationSpeed);
        }

        public override void OnInspectorGUI()
        {
            GUIHelper.LayoutInspectorBottom(() =>
            {
                if (_animationName.IsNullOrWhiteSpace())
                {
                    if (GUIHelper.Button("Play Show"))
                    {
                        PlayAnimation("Show");
                    }
                    if (GUIHelper.Button("Play Hide"))
                    {
                        PlayAnimation("Hide");
                    }
                }
                else
                {
                    //GUI.enabled = !_animationName.IsNullOrWhiteSpace();
                    if (GUIHelper.Button("Play"))
                    {
                        PlayAnimation(_animationName);
                    }
                    //GUI.enabled = true;
                }
            });
        }
    }
}
#endif