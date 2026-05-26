using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static CultureInfo[] _allCultureInfos;
        static CultureInfo[] AllCultureInfos
        {
            get
            {
                if (_allCultureInfos == null)
                {
                    _allCultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
                    //_allCultureInfos.ForEach(cultureInfo =>LegacyLog.Debug($"displayName={cultureInfo.DisplayName}, code={cultureInfo.TwoLetterISOLanguageName}"));
                }
                return _allCultureInfos;
            }
        }

        static Dictionary<SystemLanguage, string> _languageCodes;
        public static string GetLanguageCode(SystemLanguage language)
        {
            if (_languageCodes == null)
            {
                _languageCodes = new Dictionary<SystemLanguage, string>();
            }
            if (!_languageCodes.TryGetValue(language, out string code))
            {
                var name = language.ToString();
                if (name.StartsWith("Chinese"))
                {
                    name = "Chinese";
                }
                bool found = false;
                AllCultureInfos.ForEach(cultureInfo =>
                {
                    if (cultureInfo.DisplayName.StartsWith(name))
                    {
                        code = cultureInfo.TwoLetterISOLanguageName;
                        found = true;
                        return false;
                    }
                    return true;
                });
                if (!found)
                {
                    LegacyLog.Warning($"Can't get code for language {language}!");
                    code = name;
                }
                _languageCodes.Add(language, code);
            }
            return code;
        }
    }
}