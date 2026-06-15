using Anvil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    public partial class AudioManager : SingletonBehaviour<AudioManager>
    {
#region AudioContext

#endregion


        // [SerializeField] [ElementName(typeof(AudioClipName))]
        // private AudioClip[] audioClipList = null;
        private static string _audioPath;

        public static string AudioPath
        {
            get
            {
                if (_audioPath.IsNullOrEmpty())
                {
                    _audioPath = AudioConfig.AudioPath;
                    if (!_audioPath.EndsWith('/'))
                    {
                        _audioPath += '/';
                    }
                }

                return _audioPath;
            }
        }

        private Dictionary<string, AudioClip> _clipTable = new();
        GameObject audioSourcePool;

        private List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
        private Dictionary<string, float> _lastPlayTime = new Dictionary<string, float>();

        [SerializeField] int maxPooledAudioSource = 10;

        public AudioClip GetClip(AudioClipName audioClipName)
        {
            return GetClipWName(audioClipName.ToString());
        }

        public AudioClip GetClipWName(string clipName)
        {
            if (!_clipTable.TryGetValue(clipName, out var clip))
            {
                clip = Resources.Load<AudioClip>(clipName.GetAudioPath());
                if (clip == null)
                {
                    Debug.LogWarning($"canot found audio clip of name {clipName}");
                    return null;
                }

                _clipTable.Add(clipName, clip);
            }

            return clip;
        }

        [SerializeField] private bool musicEnabled = false;
        [SerializeField] private bool sfxEnabled = false;
        public bool MusiscMuted => dontPlayMusic;
        public bool SfxMuted => dontPlaySFX;

        public bool dontPlayMusic
        {
            get => !musicEnabled;
            set => musicEnabled = !value;
        }

        public bool dontPlaySFX
        {
            get => !sfxEnabled;
            set => sfxEnabled = !value;
        }

        public static bool SoundEnabled
        {
            get { return !Instance.dontPlaySFX; }
            set { Instance.dontPlaySFX = !value; }
        }

        public static bool MusicEnabled
        {
            get { return !Instance.dontPlayMusic; }
            set
            {
                Instance.dontPlayMusic = !value;
                if (!value)
                {
                    Instance.StopMusic();
                }
                // else
                // {
                //     Debug.Log("music enabled");
                //     Instance.PlayMusic(MusicId.Menu);
                // }
            }
        }

        public bool MusicPlaying => MusicPlayer.isPlaying;

        // Random pitch
        public float LowPitchRange = .95f;
        public float HighPitchRange = 1.05f;

        private int _currentIterationIndex = 0;

        private bool _inited = false;

        public void Init()
        {
            if (_inited)
            {
                return;
            }

            _inited = true;

            audioSourcePool = new GameObject("audioSourcePool");
            audioSourcePool.transform.SetParent(transform);

            //Initialize last play time dictionary
            foreach (AudioClipName clipName in Enum.GetValues(typeof(AudioClipName)))
            {
                _lastPlayTime[clipName.ToString()] = -Mathf.Infinity;
            }
        }


        public AudioPlayer PlayEffect(AudioClipName clipName, Action callback = null)
        {
            if (dontPlaySFX)
            {
                return null;
            }

            return PlayEffect(clipName.ToString(), callback);
        }

        public float GetLastPlayTime(string clipName)
        {
            return _lastPlayTime.GetValueOrDefault(clipName, 0);
        }

        public AudioPlayer PlayEffect(string clipName, float normalizedVolume = 1f, bool loop = false)
        {
            var source = PlayEffect(clipName, null, normalizedVolume);
            if (source == null)
            {
                return null;
            }

            source.audioSource.loop = loop;
            return source;
        }

        public void StopAllEffects()
        {
            foreach (AudioPlayer audioPlayer in audioPlayers)
            {
                audioPlayer.Reset();
            }
        }

        public AudioPlayer PlayEffect(string clipName, Action onPlayDone = null, float normalizedVolume = 1f)
        {
            if (dontPlaySFX || !_inited)
            {
                return null;
            }

            // float intervalRatio = 0.1f;
            AudioClip clip = GetClipWName(clipName);
            if (clip == null)
            {
                return null;
            }

            return PlayAudioClip(clip, onPlayDone, normalizedVolume);
        }

        public AudioPlayer PlayEffect(AudioClip clip, Action onPlayDone = null, float normalizedVolume = 1f)
        {
            if (dontPlaySFX || !_inited)
            {
                return null;
            }

            if (clip == null)
            {
                return null;
            }

            return PlayAudioClip(clip, onPlayDone, normalizedVolume);
        }

        public AudioPlayer PlayAudioClip(AudioClip clip, Action onPlayDone = null, float normalizedVolume = 1f)
        {
            float intervalRatio = AudioConfig.SfxIntervalLimitRatio;
            float currentTime = Time.time;
            float minIntervalBetweenPlays = clip.length * intervalRatio;
            string clipName = clip.name;
            // Check if enough time has passed since the last play of this clip
            if (currentTime - GetLastPlayTime(clipName) < minIntervalBetweenPlays)
            {
                return null;
            }

            float clipingThreshold = 0.3f;
            float maxTime = 0;
            AudioPlayer longestPlay = null;
            foreach (AudioPlayer audioPlayer in audioPlayers)
            {
                if (!audioPlayer.isPlaying)
                {
                    audioPlayer.Clip = clip;
                    audioPlayer.Volume = normalizedVolume;
                    audioPlayer.Play(onPlayDone);
                    _lastPlayTime[clipName] = currentTime;
                    return audioPlayer;
                }
            }

            foreach (AudioPlayer audioPlayer in audioPlayers)
            {
                if (audioPlayer.Clip == clip
                    && audioPlayer.time >= audioPlayer.Clip.length * clipingThreshold)
                {
                    if (audioPlayer.time >= maxTime)
                    {
                        maxTime = audioPlayer.time;
                        longestPlay = audioPlayer;
                    }
                }
            }

            if (longestPlay != null)
            {
                longestPlay.Clip = clip;
                longestPlay.Volume = normalizedVolume;
                longestPlay.Play(onPlayDone);
                _lastPlayTime[clipName] = currentTime;
                return longestPlay;
            }

            // If no suitable audio source found and pool limit is reached, reuse the oldest audio source
            if (audioPlayers.Count >= maxPooledAudioSource)
            {
                float oldestTime = float.MaxValue;
                AudioPlayer oldestPlay = null;
                foreach (AudioPlayer audioPlayer in audioPlayers)
                {
                    if (audioPlayer.time < oldestTime)
                    {
                        oldestTime = audioPlayer.time;
                        oldestPlay = audioPlayer;
                    }
                }

                if (oldestPlay != null)
                {
                    oldestPlay.Clip = clip;
                    oldestPlay.Volume = normalizedVolume;
                    oldestPlay.Play(onPlayDone);
                    _lastPlayTime[clipName] = currentTime;
                    return oldestPlay;
                }
            }

            GameObject newSourceObj = new GameObject("audioSource" + audioPlayers.Count.ToString());
            AudioPlayer newPlayer = newSourceObj.AddComponent<AudioPlayer>();
            newPlayer.Construct();
            // newSourceObj.AddComponent<AudioSource>();
            newSourceObj.transform.parent = audioSourcePool.transform;
            //target = newPlayer.audioSource;
            newPlayer.Clip = clip;
            newPlayer.Volume = normalizedVolume;
            //target.clip = audioClipList[(int)clipName];
            newPlayer.Play(onPlayDone);
            audioPlayers.Add(newPlayer);
            _lastPlayTime[clipName] = currentTime;
            return newPlayer;
        }

        public void ManagedPlay(AudioPlayer audioPlayer)
        {
            AudioClip clip = audioPlayer.Clip;
            if (audioPlayer == null || dontPlaySFX)
            {
                return;
            }

            float intervalRatio = AudioConfig.SfxIntervalLimitRatio;
            float currentTime = Time.time;
            float minIntervalBetweenPlays = clip.length * intervalRatio;
            string clipName = clip.name;
            // Check if enough time has passed since the last play of this clip
            if (currentTime - GetLastPlayTime(clipName) < minIntervalBetweenPlays)
            {
                return;
            }
            audioPlayer.Play();
            _lastPlayTime[clipName] = currentTime;
        }

        public void StopSFX()
        {
            dontPlaySFX = true;
        }


        public void ResumeSFX()
        {
            dontPlaySFX = false;
        }

        //public void RandomSoundEffect(params AudioClip[] clips)
        //{
        //


        private void IncreaseIndex()
        {
            _currentIterationIndex++;
            if (_currentIterationIndex > audioPlayers.Count - 1)
            {
                _currentIterationIndex = 0;
            }
        }

        static string svolume;

        public static void OnGUIDebug()
        {
            // GUILayout.Label($"Bgm remaining duration: {(Instance._musicPlayer.Clip.length - Instance._musicPlayer.time):0.00}      Volume: {Instance._musicPlayer.Volume}");
            // if (GUILayout.Button("CycleMusic"))
            // {
            //     Instance.LoopCycleMuscic();
            // }
            //
            // GUILayout.BeginHorizontal();
            // {
            //     foreach (AudioClip clip in Instance._ingameMusic)
            //     {
            //         if (GUILayout.Button(clip.name))
            //         {
            //             Instance.StopMusic();
            //             Instance.PlayMusic(clip, Instance.LoopCycleMuscic);
            //         }
            //     }
            // }
            // GUILayout.EndHorizontal();
            // GUILayout.BeginHorizontal();
            // {
            //     svolume = GUILayout.TextField(svolume);
            //     float.TryParse(svolume, out float volume);
            //     if (GUILayout.Button("Set Music Volume"))
            //     {
            //         Instance.SetMusicVolume(volume);
            //     }
            // }
            // GUILayout.EndHorizontal();
        }
    }
}