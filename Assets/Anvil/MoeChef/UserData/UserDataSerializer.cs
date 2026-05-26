using System;
using System.Collections.Generic;
using Anvil;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

namespace Anvil
{
    [CreateAssetMenu(fileName = "UserDataSerializer", menuName = "PKM/User Data Serializer")]
    public partial class UserDataSerializer : ScriptableObject
    {
        public static bool Active => _instance != null;
        protected static UserDataSerializer _instance;
        private static bool _loaded;
        public static bool Loaded => _loaded;

        public static UserDataSerializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(UserDataSerializer);

                    // Try to load in "Resources"
                    _instance = Resources.Load<UserDataSerializer>(type.Name);
                    if (_instance != null)
                    {
                        // Init();
                        return _instance;
                    }
#if UNITY_EDITOR
                    // Try to load in "Editor Default Resources/Resources"
                    var path = $"Assets/Editor Default Resources/Resources/{type.Name}.asset";
                    _instance = AssetDatabase.LoadAssetAtPath<UserDataSerializer>(path);
                    if (_instance == null)
                    {
                        _instance = Resources.Load<UserDataSerializer>($"EditorConfigs/{type.Name}");
                    }
#endif
                }

                return _instance;
            }
        }

        [SerializeField] private DataSerializer defaultSerializer;


        [SerializeField] private LocalDataObject _localDataObject;
        [SerializeField] private string _dataVersion = "0.1";
        [SerializeField] private int _savePointThreshold = 30;
        [SerializeField] private int _defaultSavePoint = 30;
        private static int _savePoint;
        private IDataSerializer _localSerializer;
        private static int SavePointThreshold => Instance._savePointThreshold;
        private static IDataSerializer DataSerializer
        {
            get
            {
                if (Instance._localSerializer == null)
                {
                    Init();
                }

                return Instance._localSerializer;
            }
        }
#if UNITY_EDITOR
        [MenuItem("Debug/Show Current User Data Serializer")]
        private static void ShowDataSerializer()
        {
            Selection.activeObject = _instance._localDataObject;
        }

#endif
        public static void IncreaseSavePoint(int amount = 30)
        {
            _savePoint += amount;
            if (_savePoint >= Instance._savePointThreshold)
            {
                Instance.ForceSave();
            }
        }

        private string _userID;

        public static string UserIDRaw
        {
            get
            {
                return GetOrInitUserID();
                if (Instance._userID.IsNullOrEmpty())
                {
                    Instance._userID = GetOrInitUserID();
                }

                return Instance._userID;
            }
        }

        /// <summary>
        /// 32 Digit
        /// </summary>
        /// <returns></returns>
        private static string CreateUID()
        {
            
            var guid = Guid.NewGuid().ToString("N");
            int number = UnityEngine.Random.Range(100, 1000);

            return $"POKA{number}_{guid}";
        }

        private static string GetOrInitUserID()
        {
            var userID = GetValue(CommonKeys.UserID);
            if (string.IsNullOrEmpty(userID))
            {
                userID = CreateUID();
                SaveValue(CommonKeys.UserID, userID, true);
                Debug.Log($"userID generated {userID}");
            }
            else Debug.Log($"userID found {userID}");

            return userID;
        }

        public static void Init()
        {
            var isFirstTime = false;
            
            _loaded = false;
            if (Instance._localDataObject != null)
            {
                ClearCachedData();
                Instance._localDataObject.Init();
                Instance._localSerializer = Instance._localDataObject.Serializer;
                _loaded = true;
                Instance.OnLoaded();
                Debug.Log("user data loaded and ready");
                isFirstTime = false;
            }
            else
            {
                string path = "";
#if UNITY_EDITOR
                path = Application.dataPath;
#else
            path = Application.persistentDataPath;
#endif
                if (DataSerializer == null)
                {
                    Instance._localSerializer = new LocalDataSerializer(path);
                    Debug.Log("local data serializer created");
                }

                if (DataSerializer == null)
                {
                    Debug.LogError("canot create data object");
                    return;
                }

                DataSerializer.Init();
                Debug.Log("User Data Serializer initialized, local data path: " + path);
                Debug.Log("user data loaded and ready");
                _loaded = true;
                Instance.OnLoaded();
            }

            isFirstTime = GetValue(CommonKeys.DataVersion).IsNullOrEmpty();
            if (isFirstTime)
            {
                // First time
            }

            GetOrInitUserID();
        }

        private void OnLoaded()
        {
            string saveVersion = GetValue(CommonKeys.DataVersion);

            if (saveVersion != _dataVersion)
            {
                Debug.Log("clearing data due to version mismatch (from " + saveVersion + " to " + _dataVersion + ")");
                DataSerializer.Clear();
            }

            SaveValue(CommonKeys.DataVersion, _dataVersion);
        }

#region data functions

        public static IDataWrapper SaveValue(string key, string value, bool syncNow = false)
        {
            if (DataSerializer == null)
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return null;
            }

            return DataSerializer.Save(key, value, syncNow);
        }

        public static void SaveValue(string key, int value)
        {
            SaveValue(key, value.ToString());
        }

        public static void SaveValue(string key, float value)
        {
            SaveValue(key, value.ToString());
        }

        public static string GetValue(string key, string defaultValue = "")
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return String.Empty;
            }

            return DataSerializer.GetString(key, defaultValue);
        }

        public static IDataWrapper GetValueWraper(string key, string defaultValue = "")
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return null;
            }

            var wraper = DataSerializer.GetOrAddDataWrapper(key);
            if (wraper.Value == string.Empty)
            {
                wraper.Value = defaultValue;
            }

            return wraper;
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (DataSerializer == null)
            {
                Debug.LogError("no Data forwarder assigned");
                return defaultValue;
            }

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("key is empty");
                return defaultValue;
            }

            return DataSerializer.GetInt(key, defaultValue);
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return defaultValue;
            }

            return DataSerializer.GetFloat(key, defaultValue);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return defaultValue;
            }

            return DataSerializer.GetBool(key, defaultValue);
        }

        public static void SetBool(string key, bool value)
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return;
            }

            DataSerializer.SetBool(key, value);
        }

        public static void SetFloat(string key, float value)
        {
            if (DataSerializer == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return;
            }

            DataSerializer.SetFloat(key, value);
        }

#if UNITY_EDITOR
        [MenuItem("Game/Clear Data")]
#endif
        public static void ClearData()
        {
            if (DataSerializer == null)
            {
                Debug.LogError("no Data forwarder assigned or key is empty");
                return;
            }

            DataSerializer.Clear();
            ClearCachedData();
            Debug.Log("data cleared");
        }

        public static void ClearCachedData()
        {
            foreach (var dataController in _dataControllers)
            {
                dataController.ClearInitStatus();
            }
        }

        public static void ForceClear()
        {
            if (DataSerializer == null)
            {
                Init();
            }

            DataSerializer?.Clear();
        }

#endregion


        [SerializeField] private string _testKey;
        [SerializeField] private string _testValue;

        [Button]
        public void ForceSave()
        {
            _savePoint = 0;

            DataSerializer?.UpSync();
        }

        [Button]
        public void TestSave()
        {
            if (DataSerializer == null)
            {
                Init();
            }

            SaveValue(_testKey, _testValue);
        }
    }
}