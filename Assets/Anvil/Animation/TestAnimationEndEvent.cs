#if UNITY_EDITOR
using UnityEngine;

namespace Anvil
{
	[RequireComponent(typeof(AnimationEndEvent))]
	public class TestAnimationEndEvent : MonoBehaviour
	{
        [SerializeField] string[] _animationNames;
        [SerializeField] int _guiLeft;
        [SerializeField] int _guiTop;

        private AnimationEndEvent _animationEndEvent;

        void Awake()
        {
            _animationEndEvent = GetComponent<AnimationEndEvent>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(_guiLeft, _guiTop, 300, 300));
            GUILayout.Label($"{name}:");
            int animationCount = _animationNames.Length;
            for (int i = 0; i < animationCount; i++)
            {
                var animationName = _animationNames[i];
                if (GUILayout.Button(animationName))
                {
                    _animationEndEvent.PlayAnimation(animationName, () =>
                    {
                        Debug.Log($"{name}: \"{animationName}\" ended!");
                    });
                }
            }
            GUILayout.EndArea();
        }
    }
}
#endif