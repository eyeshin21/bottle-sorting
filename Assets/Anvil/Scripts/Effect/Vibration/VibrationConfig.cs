using UnityEngine;
using Anvil.Legacy;

    public class VibrationConfig : SingletonScriptableObject<VibrationConfig>
    {
        [SerializeField, ElementName(typeof(VibrationType))]
        VibrationData[] _vibrations = new VibrationData[Helper.GetEnumCount<VibrationType>()];

        public VibrationData[] VibrationDatas => _vibrations;

        public VibrationData GetVibrationData(VibrationType vibrationType)
        {
            return _vibrations.Get((int)vibrationType);
        }
    }
