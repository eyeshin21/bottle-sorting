using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseLevelData : IData
    {
        protected int _level;
        public int Level => _level;
        public virtual string Name => _level > 0 ? $"Level {_level}" : "";

        public abstract string Serialize();
        public abstract void Deserialize(string json);

        /// <summary>
        /// Resources/{path}.bytes
        /// </summary>
        protected static string LoadJson(string path)
        {
            return FileHelper.LoadResourceBinary<string>(path);
        }

#if UNITY_EDITOR
        protected static int GetMaxLevel(string path, bool absolutePath = false)
        {
            int maxLevel = 1;
            FileHelper.ForEachFileName(path, fileName =>
            {
                if (fileName.EndsWith(".bytes"))
                {
                    maxLevel = Mathf.Max(fileName.Substring(0, fileName.Length - 6).ToInt(), maxLevel);
                }
                //else
                //{
                //   LegacyLog.Warning($"Skip \"{fileName}\"!");
                //}
            }, absolutePath);
            return maxLevel;
        }

        public override string ToString()
        {
            return Name;
        }
#endif
    }
}