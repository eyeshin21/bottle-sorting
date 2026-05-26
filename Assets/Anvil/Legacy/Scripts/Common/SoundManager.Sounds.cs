using UnityEngine;

namespace Anvil.Legacy
{
    public partial class SoundManager
    {
        static AudioClip _buttonClip;
        static AudioClip _popupClip;

        public static void SetButtonClip(AudioClip clip) => _buttonClip = clip;
        public static void SetPopupClip(AudioClip clip) => _popupClip = clip;

        public static void PlaySoundButton()
        {
            if (_buttonClip == null)
            {
                _buttonClip = Resources.Load<AudioClip>("Sounds/Button");
            }
            PlaySound(_buttonClip);
        }

        public static void PlaySoundPopup()
        {
            if (_popupClip == null)
            {
                _popupClip = Resources.Load<AudioClip>("Sounds/Popup");
            }
            PlaySound(_popupClip);
        }
    }
}