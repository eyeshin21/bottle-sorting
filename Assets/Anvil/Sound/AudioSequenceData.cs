using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
    [Serializable]
    public class AudioSequenceData
    {
        public List<AudioClip> audioClips = new List<AudioClip>();
        public bool loop = true;
        public bool shuffle = false;
        public bool resumeOnPlay = true;
        public bool inheritTime = true;
        // public AudioTransitionType transitionType = AudioTransitionType.FadeOutFadeIn;

        private int clipIndex = 0;
        private float savedTimeStamps = 0;

        public AudioSequenceData()
        {
        }

        public AudioSequenceData(AudioSequenceData data)
        {
            audioClips = new List<AudioClip>(data.audioClips);
            loop = data.loop;
            shuffle = data.shuffle;
            // transitionType = data.transitionType;
            resumeOnPlay = data.resumeOnPlay;
            savedTimeStamps = data.savedTimeStamps;
            inheritTime = data.inheritTime;
        }
        public float SavedTimeStamps
        {
            get => savedTimeStamps;
            set => savedTimeStamps = value;
        }
        public int ClipIndex
        {
            get => clipIndex;
            set => clipIndex = value;
        }
    }

}
