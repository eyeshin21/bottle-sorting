using System;
using System.Collections.Generic;
using Anvil;
using UnityEngine;

namespace MarbleMania.Scripts.Game
{
    [Serializable]
    public class LevelData : ISerialize, IDeserialize
    {

        public static LevelData LoadLevel(string levelID)
        {
            var file = $"Levels/{levelID}.json";
            LevelData data = new  LevelData();
            var levelFile = Resources.Load<TextAsset>(file);
            if (levelFile == null)
            {
                Debug.LogError("level not found: " + file);
            }
            data.Deserialize(levelFile.text);
            data.levelID = levelID;
            return data;
        }
        
        [NonSerialized] public string levelID;
        public List<TrayGridData> trayGridDatas = new List<TrayGridData>();
        public CrateGridData crateGridData;
        public int conveyorSlot;
        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public void Deserialize(string data)
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }

        public LevelData Copy()
        {
            var copy = new LevelData();
            copy.Deserialize(Serialize());
            copy.levelID = levelID;
            return copy;
        }
    }
}