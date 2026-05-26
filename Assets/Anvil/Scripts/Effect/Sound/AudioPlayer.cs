using System;
using System.Collections;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class AudioPlayer : MonoBehaviour
    {
        private bool _constructed = false;
        private AudioSource _audioSource;
        public AudioSource audioSource => _audioSource;

        // public List<IEnumerator> callback

        public AudioClip Clip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }

        public bool Loop
        {
            get => GetLoop();
            set => SetLoop(value);
        }

        public virtual float Length
        {
            get
            {
                if (Clip == null)
                {
                    return 0;
                }

                return Clip.length;
            }
        }

        protected virtual void SetLoop(bool loop)
        {
            _audioSource.loop = loop;
        }

        protected virtual bool GetLoop()
        {
            return _audioSource.loop;
        }

        public float Volume
        {
            get => _audioSource.volume;
            set
            {
                // Debug.Log($"volume set {value}");
                _audioSource.volume = value;
            }
        }

        public float time
        {
            get => _audioSource.time;
            // set => _audioSource.time = value;
        }

        public bool isPlaying => _audioSource.isPlaying;


        // private Action _callback;


        public virtual void Play(Action callback = null)
        {
            Stop();
            StartAudioSource(callback);
        }

        protected void StartAudioSource(Action callback, float timeStamp = 0)
        {
            _audioSource.Play();
            _audioSource.time = timeStamp;

            if (callback != null)
            {
                // _callback = callback;
                InsertCallbackAfterSeconds(callback, Length - timeStamp);
            }
        }

        //
        //
        // private void OnMusicEnded()
        // {
        //     Action callbackCopy = _callback;
        //     _callback = null;
        //     callbackCopy?.Invoke();
        // }

        public virtual void Stop(Action callback = null)
        {
            _audioSource.Stop();
            StopAllCoroutines();

            callback?.Invoke();
            // _callback = null;
        }

        protected virtual void Awake()
        {
            Construct();
        }

        public virtual void Construct()
        {
            if (_constructed)
            {
                return;
            }

            _constructed = true;
            _audioSource = gameObject.GetOrAddComponent<AudioSource>();
            // ChangeVolume(1);
            Volume = 1;
        }
        public void InsertCallbackAfterSeconds(Action callback, float seconds)
        {
            InsertCallbackAtTimestamp(callback, time + seconds);
        }
        public void InsertCallbackAtTimestamp(Action callback, float timestamp)
        {
            if (timestamp > Clip.length)
            {
                timestamp = Clip.length;
            }

            if (timestamp <= audioSource.time)
            {
                Debug.LogWarning("[Audioplayer] callback inserted is already expired, calling anyway");
                callback?.Invoke();
                return;
            }

            if (time > 0)
            {
                timestamp -= time;
            }

            StartCoroutine(WaitForMusicTimeStamp(timestamp, callback));
        }

        IEnumerator WaitForMusicTimeStamp(float timeStamp, Action callback)
        {
            // Debug.Log($"wait for music end in {MusicPlayer.clip.length} seconds");
            yield return new WaitForSeconds(timeStamp);
            callback?.Invoke();
        }

        private float _targetVolume;
        private float _volumeDiff;
        private float _volumeChangeDuration = 0.5f;
        private Action volumeChangeCallback;

        public void ChangeVolume(float targetVolume, float duration = -1, Action callback = null)
        {
            volumeChangeCallback = callback;
            _targetVolume = targetVolume;
            _volumeDiff = targetVolume - Volume;
            if (duration > 0)
            {
                _volumeChangeDuration = duration;
            }
            else
            {
                _volumeChangeDuration = AudioConfig.VolumeChangeDuration;
            }
            // Debug.Log($"[{gameObject.name}] changinf volume to {targetVolume} in {_volumeChangeDuration} seconds, diff {_volumeDiff}");

        }
        // IEnumerator LerpChangeVolume(float targetVolume)
        // {
        //
        // }

        private void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            if (_targetVolume - Volume != 0)
            {

                float delta = _targetVolume - Volume;
                float diff = _volumeDiff * Time.deltaTime / _volumeChangeDuration;
                if (delta > 0)
                {
                    Volume += diff;
                    if (Volume > _targetVolume)
                    {
                        Volume = _targetVolume;
                    }
                }
                else if (delta < 0)
                {
                    Volume += diff;

                    if (Volume < _targetVolume)
                    {
                        Volume = _targetVolume;
                    }
                }


                if (Math.Abs(Volume - _targetVolume) < 0.01f)
                {
                    volumeChangeCallback?.Invoke();
                    volumeChangeCallback = null;
                }
            }
        }
    }
}
