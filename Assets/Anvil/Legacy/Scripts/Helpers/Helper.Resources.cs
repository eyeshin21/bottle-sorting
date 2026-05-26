using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static AudioClip LoadAudio(string path, string fileName)
        {
            path = CheckGetPath(path, fileName);
            var audio = Resources.Load<AudioClip>(path);
            if (audio == null)
            {
                LegacyLog.Warning($"Can't load audio at \"{path}\"!");
            }
            return audio;
        }
    }
}