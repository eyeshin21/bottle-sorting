using UnityEngine;
using System;

namespace Anvil
{
    public static partial class JsonSerializer
    {
        public static readonly char Open = '[';
        public static readonly char Close = ']';
        //const char Separator = ',';
        //static readonly char Space = ' ';

        public const char ParamSeparator = '_';
        public const char ValueSeparator = ';';
        public const char ItemSeparator = '|';

        /// <summary>
        /// Returns true if json is "[[...]]".
        /// </summary>
        public static bool IsDoubleOpenClose(string json)
        {
            int length = json != null ? json.Length : 0;
            if (length >= 4)
            {
                if (json[0] == Open && json[1] == Open && json[length - 1] == Close && json[length - 2] == Close)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// "[[...]]" => "[...]"
        /// </summary>
        public static void ReduceOpenClose(ref string json)
        {
            int length = json != null ? json.Length : 0;
            if (length >= 4)
            {
                if (json[0] == Open && json[1] == Open && json[length - 1] == Close && json[length - 2] == Close)
                {
                    //Log.Debug($"Reduce \"{json}\"");
                    json = json.Substring(1, length - 2);
                }
            }
        }

        static Type TypeBool = typeof(bool);
        static Type TypeInt = typeof(int);
        static Type TypeLong = typeof(long);
        static Type TypeFloat = typeof(float);
        static Type TypeString = typeof(string);

        public static string GetJson(object value)
        {
            if (value == null) return "\"\"";

            var type = value.GetType();
            if (type == TypeString)
            {
                string s = value as string;
                if (s.Length > 0)
                {
                    char c = s[0];
                    if (c == '"' || c == '{' || c == '[')
                    {
                        return s;
                    }
                }

                return $"\"{value}\"";
            }

            if (type == TypeBool)
            {
                bool value2 = (bool)value;
                return value2 ? "true" : "false";
            }

            if (type == TypeInt || /*type == TypeBool ||*/ type == TypeFloat || type == TypeLong) return value.ToString();

            return $"\"{value}\"";
        }

        public static string Join(string key, object value)
        {
            return $"{{\"{key}\":{GetJson(value)}}}";
        }

        public static string Join(string key, object value, string key2, object value2)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16, string key17, object value17)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)},\"{key17}\":{GetJson(value17)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16, string key17, object value17, string key18, object value18)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)},\"{key17}\":{GetJson(value17)},\"{key18}\":{GetJson(value18)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16, string key17, object value17, string key18, object value18, string key19, object value19)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)},\"{key17}\":{GetJson(value17)},\"{key18}\":{GetJson(value18)},\"{key19}\":{GetJson(value19)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16, string key17, object value17, string key18, object value18, string key19, object value19, string key20, object value20)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)},\"{key17}\":{GetJson(value17)},\"{key18}\":{GetJson(value18)},\"{key19}\":{GetJson(value19)},\"{key20}\":{GetJson(value20)}}}";
        }

        public static string Join(string key, object value, string key2, object value2, string key3, object value3, string key4, object value4, string key5, object value5, string key6, object value6, string key7, object value7, string key8, object value8, string key9, object value9, string key10, object value10, string key11, object value11, string key12, object value12, string key13, object value13, string key14, object value14, string key15, object value15, string key16, object value16, string key17, object value17, string key18, object value18, string key19, object value19, string key20, object value20, string key21, object value21)
        {
            return $"{{\"{key}\":{GetJson(value)},\"{key2}\":{GetJson(value2)},\"{key3}\":{GetJson(value3)},\"{key4}\":{GetJson(value4)},\"{key5}\":{GetJson(value5)},\"{key6}\":{GetJson(value6)},\"{key7}\":{GetJson(value7)},\"{key8}\":{GetJson(value8)},\"{key9}\":{GetJson(value9)},\"{key10}\":{GetJson(value10)},\"{key11}\":{GetJson(value11)},\"{key12}\":{GetJson(value12)},\"{key13}\":{GetJson(value13)},\"{key14}\":{GetJson(value14)},\"{key15}\":{GetJson(value15)},\"{key16}\":{GetJson(value16)},\"{key17}\":{GetJson(value17)},\"{key18}\":{GetJson(value18)},\"{key19}\":{GetJson(value19)},\"{key20}\":{GetJson(value20)},\"{key21}\":{GetJson(value21)}}}";
        }
    }
}