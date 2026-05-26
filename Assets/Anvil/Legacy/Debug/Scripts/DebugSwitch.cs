#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class DebugSwitch : MonoBehaviour, IValue<bool>
    {
        bool _startOn;
        bool _isOn;
        Listener _onChanged;
        IAnimationController _animationController;

        public bool On
        {
            get => _isOn;
            set
            {
                if (_isOn != value)
                {
                    SetOn(value);
                    _onChanged?.Invoke();
                }
            }
        }

        public bool Value
        {
            get => _isOn;
            set => On = value;
        }

        public bool StateChanged => _isOn != _startOn;

        void Awake()
        {
            _animationController = AnimationController.Create(gameObject);

            var button = gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.AddListener(OnSwitch);
        }

        public void Construct(bool on, Listener onChanged)
        {
            _startOn = on;
            _onChanged = onChanged;
            SetOn(on);
        }

        void SetOn(bool on)
        {
            _isOn = on;
            _animationController.PlayAnimation(on ? "On" : "Off");
        }

        void OnSwitch()
        {
            SoundManager.PlaySoundButton();
            SetOn(!_isOn);
            _onChanged?.Invoke();
        }
    }
}
#endif