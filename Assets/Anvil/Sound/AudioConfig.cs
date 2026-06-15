using Anvil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    public enum MusicId
    {
        Menu,
        InGame,

        TestSequence1,
        TestSequence2,
    }
    public class AudioConfig : SingletonScriptableObject<AudioConfig>
    {
        [SerializeField] private string _audioPath = "AudioClip/";
        [SerializeField] private float _volumeChangesDuration = 1f;
        [Range(0, 1)] [SerializeField] private float _baseMusicVolume = 1f;
        [Range(0, 1)] [SerializeField] private float _loweredMusicVolume = 0.5f;
        [SerializeField] private float _sfxIntervalLimitRatio = 0.1f;

        // [SerializeField] private List<AudioClip> _ingameMusics;
        // [SerializeField] private List<AudioClip> _menuMusics;
        [SerializeField] private AudioTransitionData _defaultMusicTransitionData;
        [ElementName(typeof(MusicId))][SerializeField] private List<AudioSequenceData> _musicSequenceDatas;
        //
        // [SerializeField] private List<AudioData> _audioDatas;
        //

        public static string AudioPath => Instance._audioPath;
        public static float SfxIntervalLimitRatio => Instance._sfxIntervalLimitRatio;
        public static float BaseMusicVolume => Instance._baseMusicVolume;
        public static float LoweredMusicVolume => Instance._loweredMusicVolume;
        public static float VolumeChangeDuration => Instance._volumeChangesDuration;
        // public static List<AudioData> AudioDatas => Instance._audioDatas;
        public static AudioTransitionData DefaultMusicTransitionData => Instance._defaultMusicTransitionData;

        public static AudioSequenceData GetMusicSequenceData(MusicId musicId)
        {
            if (Instance._musicSequenceDatas.Count <= (int)musicId)
            {
                Debug.Log("audio index out of range");
                return null;
            }
            return Instance._musicSequenceDatas[(int)musicId];
        }
    }

}
