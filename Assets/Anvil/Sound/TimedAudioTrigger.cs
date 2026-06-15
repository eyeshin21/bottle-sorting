using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    public class TimedAudioTrigger : MonoBehaviour
    {
        [Serializable]
        private struct AudioTriggerData
        {
            public AudioClip clip;
            public float delay;
        }

        [SerializeField] private List<AudioTriggerData> _audios;
        private List<AudioTriggerData> _activeAudios;

        private void OnEnable()
        {
            if (_audios == null || _audios.Count == 0)
            {
                return;
            }

            _activeAudios = new List<AudioTriggerData>(_audios);
        }

        private void Update()
        {
            if (_activeAudios == null)
            {
                return;
            }

            for (int i = _activeAudios.Count - 1; i >= 0; i--)
            {
                var data = _activeAudios[i];
                data.delay -= Time.deltaTime;
                if (data.delay <= 0)
                {
                    _activeAudios.RemoveAt(i);
                    if (data.clip != null && AudioManager.SoundEnabled)
                    {
                        AudioManager.Instance.PlayAudioClip(data.clip);
                    }
                }
                else
                {
                    _activeAudios[i] = data;
                }
            }
        }
    }
}