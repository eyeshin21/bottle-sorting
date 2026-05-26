using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Anvil
{
    [Serializable]
    public class AudioTransitionData
    {
        public AudioTransitionType transitionType;
        public float _duration;
        [Range(0,1)] public float transitionRatio;
    }

    [Serializable]
    public class AudioTransitionController
    {
        AudioTransitionData _transitionData;
        private AudioPlayer _exitPlayer;
        private AudioPlayer _entryplayer;
        private float _exitDuration;
        private float _entryDuration;
        Action _onClipTarnsition;

        public AudioTransitionController(AudioPlayer firstPlayer = null,AudioPlayer secondPlayer = null,
            AudioTransitionData transitionData = null,Action OnClipTransition = null)
        {
            Construct(firstPlayer,secondPlayer,transitionData);
            _onClipTarnsition = OnClipTransition;
        }

        public static void CreateTransition(AudioPlayer firstPlayer,AudioPlayer secondPlayer,
            AudioTransitionData transitionData,Action OnClipTransition = null)
        {
            var audioTransitionController =
                new AudioTransitionController(firstPlayer,secondPlayer,transitionData,OnClipTransition);
            audioTransitionController.StartTransition();
        }

        private void Construct(AudioPlayer firstPlayer,AudioPlayer secondPlayer,AudioTransitionData transitionData)
        {
            if (firstPlayer == null && secondPlayer == null)
            {
                Debug.LogError("Error creating AudioTransitionController: Both audioplayer cannot be null");
                return;
            }

            _exitPlayer = firstPlayer;
            _entryplayer = secondPlayer;
            _transitionData = transitionData;
        }

        public virtual void StartTransition()
        {
            if (_transitionData == null)
            {
                return;
            }
            float transitionRatio = _transitionData.transitionRatio;
            _exitDuration = _transitionData._duration * transitionRatio;
            _entryDuration = _transitionData._duration * (1 - transitionRatio);

            float entryClipLength = _entryplayer != null ? _entryplayer.Length : 0;
            float exitClipLength = _exitPlayer != null ? _exitPlayer.Length : 0;

            if (entryClipLength < _entryDuration)
            {
                _entryDuration = entryClipLength;
            }

            if (exitClipLength < _exitDuration)
            {
                _exitDuration = exitClipLength;
            }

            if (_exitPlayer == null)
            {
                _entryplayer.Play();
                OnEntryTransitionStart();
                return;
            }

            OnExitTransitionStart();
            // _exitPlayer.InsertCallbackAtTimestamp(() =>
            // {
            //
            // }, exitClipLength);
        }

        private const int safeStep = 500;

        protected virtual void OnExitTransitionStart()
        {
            // Debug.Log("start exit transition");
            if (_exitPlayer == null)
            {
                _entryplayer.Play();
                OnEntryTransitionStart();
                return;
            }

            // Debug.Log($"lowering exit volume {_exitPlayer.gameObject.name}");

            float timeStamp = _exitPlayer.time;
            _exitPlayer.ChangeVolume(0,_exitDuration,()=>
            {
                // Debug.Log("volume chaned complete");
                OnEntryTransitionStart();

                _exitPlayer.Stop();
            });
        }

        protected virtual void OnEntryTransitionStart()
        {
            // Debug.Log($"entry Info {_entryplayer.time} {_entryplayer.Length}, {_entryplayer.Clip.name}, duration{_entryDuration}");

            _onClipTarnsition?.Invoke();
            if (_entryplayer == null)
            {
                return;
            }

            _entryplayer.Play();

            _entryplayer.Volume = 0;
            _entryplayer.ChangeVolume(AudioConfig.BaseMusicVolume,_entryDuration
            );
        }
    }
}
