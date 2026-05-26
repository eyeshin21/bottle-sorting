using NaughtyAttributes;
using UnityEngine;

namespace Anvil
{
    /// <summary>
    /// A LocalDataSerializer Wraper
    /// </summary>
    [CreateAssetMenu(fileName = "LocalDataObject", menuName = "PKM/LocalDataObject")]
    public class LocalDataObject : ScriptableObject
    {
        [SerializeField] private string fileName = "LocalData.json";

        [SerializeField] private LocalDataSerializer _serializer;

        public LocalDataSerializer Serializer
        {
            get { return _serializer; }
        }

        [Button]
        public void Init()
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath;
#else
            path = Application.persistentDataPath;
#endif
            _serializer = new LocalDataSerializer(path);
            // Debug.Log("local data serializer created");

            if (_serializer == null)
            {
                Debug.LogError("canot create data object");
                return;
            }

            _serializer.Init();
        }

        public void Unload()
        {
            _serializer = null;
        }
    }
}