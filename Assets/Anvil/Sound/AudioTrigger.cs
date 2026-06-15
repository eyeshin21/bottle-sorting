using UnityEngine;

namespace Anvil
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] protected AudioClip onEnable;
        [SerializeField] protected AudioClip onDisable;

        private void OnEnable()
        {
            Trigger(onEnable);
        }

        private void OnDisable()
        {
            Trigger(onDisable);
        }

        public void Trigger(AudioClip clip)
        {
            AudioManager.Instance.PlayEffect(clip);
        }
    }
}