using System;

namespace Anvil
{
    public class ManagedAudioPlayer : AudioPlayer
    {
        protected override void OnEnable()
        {
            if (_playOnEnable && Clip != null)
            {
                AudioManager.Instance.ManagedPlay(this);
            }
        }
    }
}