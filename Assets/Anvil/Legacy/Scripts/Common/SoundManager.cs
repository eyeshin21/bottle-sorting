//#if UNITY_EDITOR
//#define DEBUG_SOUND_MANAGER
//#endif
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public partial class SoundManager : SingletonBehaviour<SoundManager>
    {
        AudioSource _musicSource;
        AudioSource _oneShotSource;
        List<AudioSource> _sources = new();
        bool _isMusicEnabled = true;
        bool _isSoundEnabled = true;

        #region Private
        bool _MusicEnabled
        {
            get => _isMusicEnabled;
            set
            {
                if (_isMusicEnabled != value)
                {
                    _isMusicEnabled = value;

                    if (_isMusicEnabled)
                    {
                        _musicSource.UnPause();
                    }
                    else
                    {
                        _musicSource.Pause();
                    }
                }
            }
        }

        bool _SoundEnabled
        {
            get => _isSoundEnabled;
            set
            {
                if (_isSoundEnabled != value)
                {
                    _isSoundEnabled = value;

                    int count = _sources.Count;
                    if (!_isSoundEnabled)
                    {
                        _oneShotSource.Stop();

                        for (int i = 0; i < count; i++)
                        {
                            _sources[i].Stop();
                        }
                    }

                    _oneShotSource.enabled = value;

                    for (int i = 0; i < count; i++)
                    {
                        _sources[i].enabled = value;
                    }
                }
            }
        }

        float _MusicVolume
        {
            get => _musicSource.volume;
            set => _musicSource.volume = value;
        }

        AudioClip _MusicAudioClip => _musicSource.clip;

        void _SetMusic(AudioClip audioClip)
        {
            bool isPlaying = _musicSource.isPlaying;
#if DEBUG_SOUND_MANAGER
           LegacyLog.Debug($"Set music \"{_musicSource.clip}\" => \"{audioClip.name}\": playing={isPlaying}");
#endif
            _musicSource.clip = audioClip;
            _musicSource.Play();

            if (!isPlaying || !_MusicEnabled)
            {
                _musicSource.Pause();
            }
        }

        void _PlayMusic()
        {
#if DEBUG_SOUND_MANAGER
           LegacyLog.Debug($"Play music \"{_musicSource.clip}\": playing={_musicSource.isPlaying}");
#endif
            _musicSource.Play();

            if (!_MusicEnabled)
            {
                _musicSource.Pause();
            }
        }

        void _StopMusic()
        {
#if DEBUG_SOUND_MANAGER
           LegacyLog.Debug($"Stop music \"{_musicSource.clip}\"");
#endif
            _musicSource.Stop();
        }

        void _PlaySound(AudioClip clip, bool loop)
        {
            if (_SoundEnabled)
            {
                if (loop)
                {
                    var source = _GetOrAddSource(clip);
                    source.loop = true;
                    source.Play();
                }
                else
                {
                    _oneShotSource.PlayOneShot(clip);
                }
            }
        }

        void _PlaySingleSound(AudioClip clip)
        {
            if (_SoundEnabled)
            {
                var source = _GetOrAddSource(clip);
                if (!source.isPlaying)
                {
                    source.Play();
                }
            }
        }

        void _ReplaySound(AudioClip clip)
        {
            if (_SoundEnabled)
            {
                var source = _GetOrAddSource(clip);
                source.Play();
            }
        }

        void _StopSound(AudioClip clip)
        {
            int count = _sources.Count;
            for (int i = 0; i < count; i++)
            {
                var source = _sources[i];
                if (source.clip == clip)
                {
                    source.Stop();
                    source.loop = false;
                    return;
                }
            }

            //Log.Warning($"Sound not found \"{clip}\"!");
        }

        void _StopAllLoopSounds()
        {
            int count = _sources.Count;
            for (int i = 0; i < count; i++)
            {
                var source = _sources[i];
                if (source.loop)
                {
                    source.Stop();
                    source.loop = false;
                }
            }
        }

        void _StopAllSounds()
        {
            int count = _sources.Count;
            for (int i = 0; i < count; i++)
            {
                var source = _sources[i];
                source.Stop();
                source.loop = false;
            }
        }

        void _ClearAllSounds()
        {
            int count = _sources.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                Destroy(_sources[i]);
            }

            _sources.Clear();
        }

        void _SetPaused(bool paused)
        {
            int count = _sources.Count;
            for (int i = 0; i < count; i++)
            {
                var source = _sources[i];
                if (source.loop)
                {
                    if (paused)
                    {
                        source.Pause();
                    }
                    else
                    {
                        source.UnPause();
                    }
                }
            }
        }

        AudioSource _GetOrAddSource(AudioClip clip)
        {
            int count = _sources.Count;
            for (int i = 0; i < count; i++)
            {
                var source = _sources[i];
                if (source.clip == clip)
                {
                    return source;
                }
            }

            var newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.clip = clip;
            _sources.Add(newSource);

            return newSource;
        }

        #region Change Volume
        private static readonly float ChangeVolumeSpeed = 1;
        private LerpFloatController _musicVolumeController = new LerpFloatController();
        private AudioClip _nextMusicAudioClip;

        void _IncreaseMusicVolume(float volume = 1)
        {
            if (Mathf.Approximately(_musicVolumeController.Value, 0))
            {
                _PlayMusic();
            }

            _SetMusicVolume(volume);
        }

        void _DecreaseMusicVolume()
        {
            _SetMusicVolume(0);
        }

        void _SetMusicVolume(float volume)
        {
            float currentVolume = _MusicVolume;
#if DEBUG_SOUND_MANAGER
           LegacyLog.Debug($"Change volume: {currentVolume:0.0} to {volume:0.0}");
#endif
            if (Mathf.Approximately(currentVolume, volume))
            {
                _musicVolumeController.Stop();
            }
            else
            {
                _musicVolumeController.Construct(currentVolume, volume, Mathf.Abs(volume - currentVolume) / ChangeVolumeSpeed);
            }
        }

        void _ChangeMusic(AudioClip audioClip)
        {
#if DEBUG_SOUND_MANAGER
           LegacyLog.Debug($"Change music: {_musicSource.clip} => {audioClip.name}");
#endif
            _nextMusicAudioClip = audioClip;
            _DecreaseMusicVolume();
        }

        void Update()
        {
            if (!_musicVolumeController.Finished)
            {
                if (_nextMusicAudioClip != null)
                {
                    _MusicVolume = _musicVolumeController.Update(Time.deltaTime * 2f);
                    if (_musicVolumeController.Finished)
                    {
                        _SetMusic(_nextMusicAudioClip);
                        _nextMusicAudioClip = null;
                        _IncreaseMusicVolume();
                    }
                }
                else
                {
                    _MusicVolume = _musicVolumeController.Update(Time.deltaTime);
                }
            }
        }
        #endregion
        #endregion

        protected override void OnAwake()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;

            _oneShotSource = gameObject.AddComponent<AudioSource>();
            _oneShotSource.playOnAwake = false;
        }

        public static bool MusicEnabled
        {
            get => Instance._MusicEnabled;
            set => Instance._MusicEnabled = value;
        }

        public static bool SoundEnabled
        {
            get => Instance._SoundEnabled;
            set => Instance._SoundEnabled = value;
        }

        public static float MusicVolume
        {
            get => Instance._MusicVolume;
            set => Instance._MusicVolume = value;
        }

        public static AudioClip MusicAudioClip => Instance._MusicAudioClip;

        public static void SetMusic(AudioClip audioClip)
        {
            Instance._SetMusic(audioClip);
        }

        public static void PlayMusic(AudioClip audioClip, float volume = 1)
        {
            Instance._SetMusic(audioClip);
            MusicVolume = volume;
            Instance._IncreaseMusicVolume(volume);
        }

        public static void StopMusic()
        {
            if (_instance != null)
            {
                _instance._StopMusic();
            }
        }

        public static void PlaySound(AudioClip clip, bool loop = false)
        {
            LogPlaySound(clip);
            if (clip == null) return;
            Instance._PlaySound(clip, loop);
        }

        public static void PlaySingleSound(AudioClip clip)
        {
            LogPlaySound(clip);
            if (clip == null) return;
            Instance._PlaySingleSound(clip);
        }

        public static void ReplaySound(AudioClip clip)
        {
            LogPlaySound(clip);
            if (clip == null) return;
            Instance._ReplaySound(clip);
        }

        public static void StopSound(AudioClip clip)
        {
            if (clip == null) return;
            if (_instance != null)
            {
                _instance._StopSound(clip);
            }
        }

        public static void StopAllLoopSounds()
        {
            if (_instance != null)
            {
                _instance._StopAllLoopSounds();
            }
        }

        public static void StopAllSounds()
        {
            if (_instance != null)
            {
                _instance._StopAllSounds();
            }
        }

        public static void ClearAllSounds()
        {
            if (_instance != null)
            {
                _instance._ClearAllSounds();
            }
        }

        public static void SetPaused(bool paused)
        {
            Instance._SetPaused(paused);
        }

        public static void IncreaseMusicVolume(float volume = 1f)
        {
            if (_instance != null)
            {
                _instance._IncreaseMusicVolume(volume);
            }
        }

        public static void DecreaseMusicVolume()
        {
            if (_instance != null)
            {
                _instance._DecreaseMusicVolume();
            }
        }

        public static void ChangeMusic(AudioClip audioClip)
        {
            if (_instance != null)
            {
                _instance._ChangeMusic(audioClip);
            }
        }

        [System.Diagnostics.Conditional("DEBUG_MODE")]
        static void LogPlaySound(AudioClip clip)
        {
#if DEBUG_MODE
            if (DebugManager.LogPlaySound)
            {
                if (clip != null)
                {
                   LegacyLog.Debug($"Play sound <b>\"{clip.name}\"</b>");
                }
                else
                {
                   LegacyLog.Warning($"Play sound (null)");
                }
            }
#endif
        }
    }
}