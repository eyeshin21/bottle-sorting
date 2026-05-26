using System;
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class SequentialAudioPlayer : AudioPlayer
    {
        private float totalLength = 0;

        public override float Length
        {
            get { return totalLength; }
        }

        [SerializeField] protected AudioSequenceData _data;

        // [SerializeField] private bool _shuffle = false;
        protected List<AudioClip> _audioClips => _data.audioClips;
        private bool _loop => _data.loop;

        protected int _currentClipIndex
        {
            get => _data.ClipIndex;
            set => _data.ClipIndex = value;
        }

        protected float _timeStamp
        {
            get
            {
                if (_data == null)
                {
                    return 0;
                }

                return _data.SavedTimeStamps;
            }
            set => _data.SavedTimeStamps = value;
        }

        protected Action _callback = null;

        protected override void SetLoop(bool value)
        {
            _data.loop = value;
        }

        protected override bool GetLoop()
        {
            return _data.loop;
        }

        // public override void Construct()
        // {
        //     base.Construct();
        //     if (_data.shuffle)
        //     {
        //         _data.audioClips.SimpleShuffle();
        //     }
        // }

        public virtual void SetSequenceData(AudioSequenceData data)
        {
            totalLength = 0;
            if (data == null)
            {
                Debug.Log("sequence data null");
                Stop();
                return;
            }

            AudioSequenceData datacopy = data;
            // Debug.Log($"sample dat {datacopy.SavedTimeStamps}");
            if (isPlaying)
            {
                Stop();
            }

            // Debug.Log($"seting data {datacopy.SavedTimeStamps}");
            _data = datacopy;
            _timeStamp = datacopy.SavedTimeStamps;

            foreach (AudioClip audioClip in datacopy.audioClips)
            {
                totalLength += audioClip.length;
            }
        }

        public override void Stop(Action callback = null)
        {
            if (_data != null)
            {
                _data.ClipIndex = _currentClipIndex - 1;
                _data.SavedTimeStamps = time;
            }

            base.Stop(callback);
        }

        public override void Play(Action callback = null)
        {
            if (callback != null && _loop)
            {
                Debug.LogWarning("[SequantialAudioPlayer] Loop is enabled, callback will not be called");
            }

            bool playing = isPlaying;
            _callback = callback;
            float timeStamp = 0;
            if (!playing && _data.resumeOnPlay)
            {
                timeStamp = _data.SavedTimeStamps;
            }

            CycleAudioSequence(timeStamp);
        }

        private void CycleAudioSequence(float initialTimeStamp = 0)
        {
            // Debug.Log($"cycling audio sequenceat {_currentClipIndex} with time {initialTimeStamp}");
            if (_currentClipIndex >= _audioClips.Count)
            {
                _currentClipIndex = 0;
                if (!_loop)
                {
                    _callback?.Invoke();
                    _callback = null;
                    return;
                }
            }

            Clip = _audioClips.ForceGet(_currentClipIndex);
            _currentClipIndex++;
            StartAudioSource(() => { CycleAudioSequence(); }, initialTimeStamp);
            // StartAudioSource();
        }
    }
}
