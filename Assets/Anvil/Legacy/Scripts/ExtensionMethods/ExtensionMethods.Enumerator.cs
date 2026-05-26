using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Returns [item1, ...].
        /// </summary>
        public static string ToString2(this IEnumerator enumerator)
        {
            if (enumerator == null) return "";

            StringBuilder sb = null;
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                if (sb == null)
                {
                    sb = Helper.GetStringBuilder();
                    sb.Append($"[{enumerator.Current}");
                }
                else
                {
                    sb.Append($", {enumerator.Current}");
                }
            }

            if (sb != null)
            {
                sb.Append("]");
                var s = sb.ToString();
                Helper.ReturnStringBuilder(sb);
                return s;
            }

            // Empty
            return "[]";
        }

        /// <summary>
        /// Returns [item1, ...].
        /// </summary>
        public static string ToString2<T>(this IEnumerator<T> enumerator)
        {
            if (enumerator == null) return "";

            StringBuilder sb = null;
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                if (sb == null)
                {
                    sb = Helper.GetStringBuilder();
                    sb.Append($"[{enumerator.Current}");
                }
                else
                {
                    sb.Append($", {enumerator.Current}");
                }
            }

            if (sb != null)
            {
                sb.Append("]");
                var s = sb.ToString();
                Helper.ReturnStringBuilder(sb);
                return s;
            }

            // Empty
            return "[]";
        }
    }
}