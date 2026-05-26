using UnityEngine;

namespace Anvil.Legacy
{
    [System.Serializable]
    public class SoundData
    {
        [SerializeField] string _name;
        [SerializeField] string _fileName;
        [SerializeField] AudioClip _audio;

        public string Name => _name;
        public string FileName => string.IsNullOrWhiteSpace(_fileName) ? _name : _fileName;
        public AudioClip Audio => _audio;
    }
}