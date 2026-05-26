using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class DictionaryObject : Dictionary<string, object>
    {
        /// <summary>
        /// Only add if value is true. // Add(key, 1)
        /// </summary>
        public void CheckAdd(string key, bool value)
        {
            if (value)
            {
                //Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, 1);
            }
        }

        /// <summary>
        /// Only add if value != 0.
        /// </summary>
        public void CheckAdd(string key, int value)
        {
            if (value != 0)
            {
                //Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, value);
            }
        }

        /// <summary>
        /// Only add if value not null and not empty.
        /// </summary>
        public void CheckAdd(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                //Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, value);
            }
        }

        /// <summary>
        /// Adds integer value (0|1).
        /// </summary>
        public void Add(string key, bool value)
        {
            base.Add(key, value ? 1 : 0);
        }

        public void Add(string key, int value)
        {
            base.Add(key, value);
        }

        public void Add(string key, long value)
        {
            base.Add(key, value);
        }

        public void Add(string key, string value)
        {
            base.Add(key, value);
        }

        /// <summary>
        /// Adds string value.
        /// </summary>
        public void Add<T>(string key, T value) where T : Enum
        {
            base.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds {time:0.00}.
        /// </summary>
        public void AddTime(string key, float time)
        {
            base.Add(key, time.ToString("0.00"));
        }

        public void Add(string key, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                base.Add(key, dateTime.Value.ToString2());
            }
        }

        public object GetOrAdd(string key, object defaultValue)
        {
            if (TryGetValue(key, out object value))
            {
                return value;
            }

            base.Add(key, defaultValue);
            return defaultValue;
        }

        public void Set(string key, int value)
        {
            base.Remove(key);
            base.Add(key, value);
        }

        /// <summary>
        /// Returns {"key1":intValue,"key2":"stringValue",...}
        /// </summary>
        public string ToJson()
        {
            int count = Count;
            if (count == 0) return "{}";

            var sb = Helper.GetStringBuilder();
            bool isEmpty = true;
            foreach (var entry in this)
            {
                if (isEmpty)
                {
                    isEmpty = false;
                    sb.Append($"{{\"{entry.Key}\":{JsonHelper.GetJson(entry.Value)}");
                }
                else
                {
                    sb.Append($",\"{entry.Key}\":{JsonHelper.GetJson(entry.Value)}");
                }
            }

            if (isEmpty) return "{}";
            sb.Append("}");

            var json = sb.ToString();
            Helper.ReturnStringBuilder(sb);
            return json;
        }

        public string ToLineString()
        {
            return Helper.CreateString(sb =>
            {
                foreach (var entry in this)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append($"{entry.Key}:{JsonHelper.GetJson(entry.Value)}");
                    }
                    else
                    {
                        sb.Append($", {entry.Key}:{JsonHelper.GetJson(entry.Value)}");
                    }
                }
            });
        }

        public override string ToString()
        {
            return Helper.CreateString(sb =>
            {
                bool newLine = false;
                foreach (var entry in this)
                {
                    if (newLine)
                    {
                        sb.Append($",\n{entry.Key}:{entry.Value}");
                    }
                    else
                    {
                        sb.Append($"{{{entry.Key}:{entry.Value}");
                        newLine = true;
                    }
                }
                if (newLine)
                {
                    sb.Append("}");
                }
            });
        }
    }
}