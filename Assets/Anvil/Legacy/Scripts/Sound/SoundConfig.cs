using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] string _path;
        [SerializeField] SoundData[] _soundDatas;

        Dictionary<string, AudioClip> _nameAudios = new();

        SoundData GetSoundData(string name)
        {
            int count = _soundDatas.GetLength();
            for (int i = 0; i < count; i++)
            {
                var soundData = _soundDatas[i];
                if (soundData.Name == name)
                {
                    return soundData;
                }
            }

            //Log.Warning($"Can't find sound data for \"{name}\"!");
            return null;
        }

        public AudioClip GetAudio(string name)
        {
            if (!_nameAudios.TryGetValue(name, out AudioClip audio))
            {
                var soundData = GetSoundData(name);
                if (soundData != null)
                {
                    audio = soundData.Audio;
                    if (audio == null)
                    {
                        audio = Helper.LoadAudio(_path, soundData.FileName);
                    }
                }
                else
                {
                    audio = Helper.LoadAudio(_path, name);
                }
                _nameAudios.Add(name, audio);
            }
            return audio;
        }
    }
}