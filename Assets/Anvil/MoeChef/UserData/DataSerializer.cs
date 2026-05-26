using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public interface IDataWrapper
    {
        public string Value { get; set; }
    }
    public interface IDataSerializer
    {
        public void Init();
        public void UpSync();
        public void Clear();
        public IDataWrapper Save(string key, string value, bool syncNow = false);
        public IDataWrapper GetOrAddDataWrapper(string key);
        
        public string GetString(string key, string defaultValue = "");
        public int GetInt(string key, int defaultValue = 0);
        public float GetFloat(string key, float defaultValue = 0);
        public bool GetBool(string key, bool defaultValue = false);
        
        public void SetString(string key, string value);
        public void SetInt(string key, int value);
        public void SetFloat(string key, float value);
        public void SetBool(string key, bool value);
        
    }
    public class DataWrapper : IDataWrapper
    {
        public DataWrapper()
        {
            
        }
        public DataWrapper(string value)
        {
            Value = value;
        }

        // private string _key;
        // public string Key => _key;
        public string Value { get; set; }
        public void Clear()
        {
            Value = string.Empty;
        }
    }
    public abstract class DataSerializer : IDataSerializer
    {
        public abstract void UpSync();
        public abstract IDataWrapper Save(string key, string value, bool syncNow = false);
        public abstract IDataWrapper GetOrAddDataWrapper(string key);
        public abstract string GetString(string key, string defaultValue = "");
        public abstract int GetInt(string key, int defaultValue = 0);
        public abstract float GetFloat(string key, float defaultValue = 0); 
        public abstract bool GetBool(string key, bool defaultValue = false);
        public abstract void SetString(string key, string value);
        public abstract void SetInt(string key, int value);
        public abstract void SetFloat(string key, float value);
        public abstract void SetBool(string key, bool value);

        public abstract void Init();


        public abstract void Clear();
    }
}