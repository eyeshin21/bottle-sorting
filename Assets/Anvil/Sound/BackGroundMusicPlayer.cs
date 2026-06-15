// using System;
// using UnityEngine;
//
// namespace MergeGame
// {
//     public class BackGroundMusicPlayer : SequentialAudioPlayer
//     {
//
//         protected override bool GetLoop()
//         {
//             return true;
//         }
//
//         public void OverrideMusic()
//         {
//             if (AudioManager.Instance.MusicPlayer != null)
//             {
//                 AudioManager.Instance.MusicPlayer.Stop();
//             }
//             AudioManager.Instance.MusicPlayer = this;
//             Play();
//             audioSource.time = _timeStamp;
//         }
//         public override void Stop(Action callback = null)
//         {
//             base.Stop();
//             if (AudioManager.Instance.MusicPlayer == this)
//             {
//                 AudioManager.Instance.MusicPlayer = null;
//             }
//         }
//         //
//         // private void OnEnable()
//         // {
//         //     OverrideMusic();
//         // }
//
//         // public override void SetSequenceData(AudioSequenceData data)
//         // {
//         //     base.SetSequenceData(data);
//         //     _timeStamp = 0;
//         // }
//         //
//         // public override void Play(Action callback = null)
//         // {
//         //     if (callback != null && GetLoop())
//         //     {
//         //         Debug.LogWarning("[SequantialAudioPlayer] Loop is enabled, callback will not be called");
//         //     }
//         //     _callback = callback;
//         //     CycleAudioSequence();
//         // }
//         // private void CycleAudioSequence()
//         // {
//         //     if (_currentClipIndex >= _audioClips.Count)
//         //     {
//         //         _currentClipIndex = 0;
//         //         if (!GetLoop())
//         //         {
//         //             _callback?.Invoke();
//         //             _callback = null;
//         //             return;
//         //         }
//         //     }
//         //
//         //     Clip = _audioClips[_currentClipIndex];
//         //     _currentClipIndex++;
//         //     StartAudioSource(CycleAudioSequence);
//         //     // StartAudioSource();
//         // }
//     }
// }
