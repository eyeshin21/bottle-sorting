using System.Collections.Generic;
using Anvil.Legacy;
using UnityEditor;
using UnityEngine;

namespace Anvil
{

    [System.Serializable]
    public class LocalDataSerializer : DataSerializer
    {
        private static readonly char Separator = ',';
        private static readonly char Space = ' ';

        public LocalDataSerializer(string filePath)
        {
            this.filePath = filePath;
        }
        
        private Dictionary<string,IDataWrapper> _data;

        private Dictionary<string,IDataWrapper> Data
        {
            get
            {
                if (!_inited)
                {
                    Init();
                }
                return _data;
            }
        }
        
        private bool _inited = false;
        public override void Init()
        {
            // _data = new Dictionary<string,IDataWrapper>();
            if (System.IO.File.Exists(FullPath))
            {
                string json = System.IO.File.ReadAllText(FullPath);
                Debug.Log($"local data loaded from {FullPath}");
                Deserialize(json);
            }
            else
            {
                ClearInstanceData();
            }

            _inited = true;

            // Debug.Log("data loaded");
        }
        
        public override void Clear()
        {
            ClearInstanceData();
            UpSync();
        }
        private void ClearInstanceData()
        {
            if (_data != null)
            {
                _data.Clear();
            }
            else _data = new();
        }
        // private Gametamin.Dictionary _data;
         
        // [SerializeField] private string _subPath;
        private string filePath;
        private string FilePath
        {
            get
            {
                return filePath;

            }
        }
        private static readonly string _fileName = "LocalData.pkm";
        
        public string FullPath => $"{FilePath}/{_fileName}";
        
        public override void UpSync()
        {
            string json = Serialize();
            
            System.IO.File.WriteAllText(FullPath, json);
            // Debug.Log($"local data saved {FullPath}");
        }
        public override IDataWrapper Save(string key,string value, bool upSync = false)
        {
            IDataWrapper wrapper = null;
            if (Data.ContainsKey(key))
            {
                Data[key].Value = value;
                wrapper = Data[key];
            }
            else
            {
                wrapper = new DataWrapper(value);
                Data.Add(key, wrapper);
            }

            if (upSync)
            {
                UpSync();
            }
            return wrapper;
        }

        public override IDataWrapper GetOrAddDataWrapper(string key)
        {
            if (Data.TryGetValue(key,out IDataWrapper wrapper))
            {
                return wrapper;
            }

            wrapper = new DataWrapper();
            Data.Add(key,wrapper);
            return wrapper;
        }

        public override string GetString(string key, string defaultValue = "")
        {
            if (Data.TryGetValue(key,out IDataWrapper wrapper))
            {
                return wrapper.Value;
            }

            return defaultValue;
        }

        public override int GetInt(string key, int defaultValue = 0)
        {
            if (Data == null)
            {
                Debug.Log("data is null");
            }
            if (Data.TryGetValue(key,out IDataWrapper wrapper))
            {
                return wrapper.Value.ToInt(defaultValue);
            }

            return defaultValue;
        }

        public override float GetFloat(string key, float defaultValue = 0)
        {
            if (Data.TryGetValue(key,out IDataWrapper wrapper))
            {
                bool valid = JsonSerializer.TryParseSerializedFloat(wrapper.Value, out float result);
                if (valid)
                {
                    return result;
                }
            }

            return defaultValue;
        }

        public override bool GetBool(string key,bool defaultValue = false)
        {
            if (Data.TryGetValue(key,out IDataWrapper wrapper))
            {
                return wrapper.Value.ToBool(defaultValue);
            }

            return defaultValue;
        }

        public override void SetString(string key,string value)
        {
            Save(key, value);
        }

        public override void SetInt(string key,int value)
        {
            Save(key, value.ToString());
        }

        public override void SetFloat(string key,float value)
        {
            Assert.IsFalse(Data.ContainsKey(key), $"{key}:{value}");

            Data.Add(key, new DataWrapper(value.ToString()));
        }

        public override void SetBool(string key,bool value)
        {
            Debug.Log("saving bool: " + key + ":" + value);
            Save(key, value.ToString());
        }
        public string Serialize()
        {
            int count = _data.GetCount();
            if (count == 0) return "";

            string json = "";
            bool isFirst = true;
            foreach (var entry in _data)
            {
                if (isFirst)
                {
                    json = $"[{entry.Key}:{entry.Value.Value}";
                    isFirst = false;
                }
                else
                {
                    json = $"{json},{entry.Key}:{entry.Value.Value}";
                }
            }

            return json + "]";
        }
        
        /// <summary>
        /// Parses json string to dictionary.
        /// </summary>
        /// <param name="json">[key1:value1,key2:[key21:value21,key22:value22],...,keyN:valueN].</param>
        /// <param name="isSimpleJson">{"key1":value1,"key2":{"key21":value21,...},...,"keyN":valueN}</param>
        public void Deserialize(string json, bool isSimpleJson = false)
        {
            ClearInstanceData();
            if (string.IsNullOrEmpty(json)) return;

            char openChar = '[';
            char closeChar = ']';
            if (isSimpleJson)
            {
                openChar = '{';
                closeChar = '}';
            }
            else
            {
                JsonSerializer.ReduceOpenClose(ref json);
                // Assert.IsFalse(JsonHelper.IsDoubleOpenClose(json), json);
            }

            int length = json.Length;
            string key = null;
            string value = null;
            int keyStartIndex = -1;
            int valueStartIndex = -1;
            bool isKeyQuoted = false;
            bool isStringValue = false;
            char nestedOpenChar = openChar;
            char nestedCloseChar = closeChar;
            int openCount = 0;

            for (int i = 0; i < length; i++)
            {
                char c = json[i];

                // Key
                if (key == null)
                {
                    // Start index
                    if (keyStartIndex < 0)
                    {
                        if (c != Space && c != openChar && c != Separator)
                        {
                            keyStartIndex = i;
                            isKeyQuoted = c == '"';
                        }
                    }
                    // End index
                    else
                    {
                        if (c == ':')
                        {
                            for (int j = i - 1; j >= keyStartIndex; j--)
                            {
                                if (json[j] != Space)
                                {
                                    if (isKeyQuoted)
                                    {
                                        key = json.Substring(keyStartIndex + 1, j - keyStartIndex - 1);
                                    }
                                    else
                                    {
                                        key = json.Substring(keyStartIndex, j - keyStartIndex + 1);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                // Value
                else
                {
                    // Start index
                    if (valueStartIndex < 0)
                    {
                        if (c != Space)
                        {
                            if (c == Separator || c == closeChar || i == length - 1)
                            {
                                value = "";
                            }
                            else
                            {
                                valueStartIndex = i;
                                openCount = 0;
                                if (c == '\"')
                                {
                                    isStringValue = true;
                                }
                                else
                                {
                                    isStringValue = false;

                                    if (c == '[')
                                    {
                                        nestedOpenChar = '[';
                                        nestedCloseChar = ']';
                                        openCount = 1;
                                    }
                                    else if (c == '{')
                                    {
                                        nestedOpenChar = '{';
                                        nestedCloseChar = '}';
                                        openCount = 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // String value
                        if (isStringValue)
                        {
                            if (c == '\"')
                            {
                                value = json.Substring(valueStartIndex + 1, i - valueStartIndex - 1); // Not get ""
                            }
                        }
                        else
                        {
                            // Nested json
                            if (openCount > 0)
                            {
                                if (c == nestedOpenChar)
                                {
                                    openCount++;
                                }
                                else if (c == nestedCloseChar)
                                {
                                    openCount--;
                                    if (openCount == 0)
                                    {
                                        value = json.Substring(valueStartIndex, i - valueStartIndex + 1);
                                    }
                                }
                            }
                            else
                            {
                                if (c == Separator || c == closeChar || i == length - 1)
                                {
                                    for (int j = i - 1; j >= valueStartIndex; j--)
                                    {
                                        if (json[j] != Space)
                                        {
                                            value = json.Substring(valueStartIndex, j - valueStartIndex + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (value != null)
                    {
                        Assert.IsFalse(_data.ContainsKey(key), $"{key}:{value}");
                        _data.Add(key, new DataWrapper(value));

                        key = null;
                        value = null;
                        keyStartIndex = -1;
                        valueStartIndex = -1;
                    }
                }
            }

        }

    }
}