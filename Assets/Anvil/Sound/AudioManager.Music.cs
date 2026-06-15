using System.Collections.Generic;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public partial class AudioManager : SingletonBehaviour<AudioManager>
    {
#region AudioContext
        public static MusicId CurrentBackgroundMusicId;
        
#endregion
        private AudioPlayer _musicPlayer;

        private Dictionary<MusicId, AudioSequenceData> _musicSequenceDatas =
            new Dictionary<MusicId, AudioSequenceData>();
        private List<AudioPlayer> _musicPlayers = new List<AudioPlayer>();

        public AudioPlayer MusicPlayer
        {
            get
            {
                // if (_musicPlayer == null)
                // {
                //     AudioPlayer musicPlayer = GetEmptySequentialMusicPlayer();
                //     _musicPlayer = musicPlayer;
                // }

                return _musicPlayer;
            }
            set => _musicPlayer = value;
        }

        public void StopMusic()
        {
            OverrideMusic(null);
        }

        public void LowerMusic()
        {
            if (!MusicEnabled)
            {
                return;
            }
            MusicPlayer.ChangeVolume(AudioConfig.LoweredMusicVolume);
        }
        public void ResetMusicVolume()
        {
            if (!MusicEnabled)
            {
                return;
            }
            MusicPlayer.ChangeVolume(AudioConfig.BaseMusicVolume);
        }
        public void ChangeMusicVolume(float targetVolume)
        {
            if (!MusicEnabled)
            {
                return;
            }
            MusicPlayer.ChangeVolume(targetVolume);
        }


        private SequentialAudioPlayer CreateSequentialMusicPlayer()
        {
            GameObject musicObj = new GameObject("SequntialMusicSource" + _musicPlayers.Count.ToString());
            SequentialAudioPlayer musicPlayer = musicObj.AddComponent<SequentialAudioPlayer>();
            musicPlayer.Construct();
            musicPlayer.Volume = AudioConfig.BaseMusicVolume;
            musicObj.transform.parent = transform;
            _musicPlayers.Add(musicPlayer);
            return musicPlayer;
        }

        public SequentialAudioPlayer GetEmptySequentialMusicPlayer()
        {
            foreach (AudioPlayer audioPlayer in _musicPlayers)
            {
                if (audioPlayer.isPlaying)
                {
                    continue;
                }
                if (audioPlayer is SequentialAudioPlayer)
                {
                    return audioPlayer as SequentialAudioPlayer;
                }
            }

            return CreateSequentialMusicPlayer();
        }

        public AudioSequenceData GetMusicSequenceData(MusicId musicId)
        {
            if (_musicSequenceDatas.TryGetValue(musicId, out var data))
            {
                // Debug.Log($"data pulled {data.SavedTimeStamps}");
                return data;
            }
            AudioSequenceData sequenceData = AudioConfig.GetMusicSequenceData(musicId);
            AudioSequenceData clonedMusicData = new AudioSequenceData(sequenceData);
            _musicSequenceDatas.Add(musicId, clonedMusicData);
            // Debug.Log($"data cloned {clonedMusicData.SavedTimeStamps}");

            return clonedMusicData;
        }
        public void PlayMusic(MusicId musicId)
        {
            // Debug.Log($"playing music {musicId}");
            PlayMusic(GetMusicSequenceData(musicId));
            CurrentBackgroundMusicId = musicId;
        }
        public void PlayMusic(AudioSequenceData musicData)
        {
            if (!MusicEnabled)
            {
                // Debug.Log("music not enabled");
                return;
            }
            if (musicData == null)
            {
                Debug.Log("no music data");
                return;
            }

            Debug.Log($"playing music with {musicData.SavedTimeStamps}");
            SequentialAudioPlayer musicPlayer = GetEmptySequentialMusicPlayer();
            musicPlayer.SetSequenceData(musicData);
            OverrideMusic(musicPlayer);
        }

        public void OverrideMusic(AudioPlayer musicPlayer)
        {
            if (musicPlayer == null)
            {
                Debug.Log("music player null");
            }

            AudioTransitionController.CreateTransition(MusicPlayer, musicPlayer, AudioConfig.DefaultMusicTransitionData,
                () =>
                {
                    MusicPlayer = musicPlayer;
                });

            // musicPlayer.Play();
        }

        public void ResumeMusic()
        {
            Debug.Log("resuming music");
            if (MusicPlayer != null)
            {
                MusicPlayer.Play();
                return;
            }

            Debug.Log("no current music player");
            PlayMusic(CurrentBackgroundMusicId);
        }
    }
}
