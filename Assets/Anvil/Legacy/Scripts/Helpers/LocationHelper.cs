using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Anvil.Legacy
{
    /*
     * https://pro.ip-api.com
     {
        "city":"Hanoi",
        "country":"Vietnam",
        "countryCode":"VN",
        "query":"113.22.35.106",
        "regionName":"Hanoi",
        "zip":""
     }
     */
    public class IPAPIJson
    {
        public string country;
        public string countryCode;
        public string regionName;
        public string city;
        public string zip;
        public string query;

#if DEBUG_MODE
        public override string ToString()
        {
            return Dictionary.ToString(dict =>
            {
                dict.CheckAdd("country", country);
                dict.CheckAdd("countryCode", countryCode);
                dict.CheckAdd("regionName", regionName);
                dict.CheckAdd("city", city);
                dict.CheckAdd("zip", zip);
                dict.CheckAdd("query", query);
            });
        }
#endif
    }

    /*
     * https://ipinfo.io/?token
     {
        "ip": "113.22.35.106",
        "city": "Da Nang",
        "region": "Da Nang",
        "country": "VN",
        "loc": "16.0678,108.2208",
        "org": "AS18403 FPT Telecom Company",
        "postal": "50250",
        "timezone": "Asia/Ho_Chi_Minh"
     }
     */
    public class IPInfoJson
    {
        public string ip;
        public string city;
        public string region;
        public string country;
    }

    /*
     * https://v4.api.ipinfo.io/lite/me?token
     {
        "ip": "113.22.35.106",
        "asn": "AS18403",
        "as_name": "FPT Telecom Company",
        "as_domain": "fpt.vn",
        "country_code": "VN",
        "country": "Vietnam",
        "continent_code": "AS",
        "continent": "Asia"
     }
     */
    public class IPInfoLiteJson
    {
        public string ip;
        public string country;
        public string country_code;
        public string continent;
    }

    public static class LocationHelper
    {
        static readonly string IP_API = "https://pro.ip-api.com/json/?fields=country,countryCode,regionName,city,zip,query&key=okTs5CB7NDAzSzg";
        //static readonly string IP_INFO = "https://ipinfo.io/?token=624a6d74ede2f1";
        static readonly string IP_INFO_LITE = "https://v4.api.ipinfo.io/lite/me?token=624a6d74ede2f1";

        static string _location = "";
        static string _country = "";
        static string _countryCode = "";

        public static string Location => _location;
        public static string Country => _country;
        public static string CountryCode => _countryCode;

        // static bool UseIPAPI => RemoteConfig.UseIPAPI;
        static bool UseIPAPI => false;

        public static void Init()
        {
            InitTracker.Track("LocationHelper");
            var location = LocalPrefs.GetString(LocalKeys.Location);
            if (!string.IsNullOrEmpty(location))
            {
                SetLocation(location);
            }

            if (UseIPAPI)
            {
                Helper.StartCoroutine(DetectLocation(IP_API));
            }
            else
            {
                //Helper.StartCoroutine(DetectLocation(IP_INFO));
                Helper.StartCoroutine(DetectLocation(IP_INFO_LITE));
            }
        }

        static IEnumerator DetectLocation(string uri)
        {
            var request = UnityWebRequest.Get(uri);
            //request.chunkedTransfer = false;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (request.isDone)
                {
                    string location = request.downloadHandler.text;
                    if (!string.IsNullOrEmpty(location))
                    {
                        // Remove line breaks
                        int length = location.Length;
                        for (int i = 0; i < length; i++)
                        {
                            if (location[i] == '\n')
                            {
                                var chars = new char[length - 1];
                                for (int j = 0; j < i; j++)
                                {
                                    chars[j] = location[j];
                                }
                                int index = i;
                                for (int j = i + 1; j < length; j++)
                                {
                                    char c = location[j];
                                    if (c != '\n')
                                    {
                                        chars[index++] = c;
                                    }
                                }
                                location = new string(chars, 0, index);
                                break;
                            }
                        }

                        var currentLocation = LocalPrefs.GetString(LocalKeys.Location);
                        if (location != currentLocation)
                        {
                            LocalPrefs.SetString(LocalKeys.Location, location);
                            SetLocation(location);
                        }
                    }
                }
            }
            else
            {
                LegacyLog.Error(request.result);
            }

            request.Dispose();
        }

        static void SetLocation(string json)
        {
            //Log.Debug($"SetLocation({json})");
            try
            {
                if (json.Contains("query"))
                {
                    var data = JsonUtility.FromJson<IPAPIJson>(json);
                    if (data != null)
                    {
                        var city = data.city;
                        var regionName = data.regionName;
                        _location = Join(data.zip, Join(city != regionName ? Join(city, regionName) : city, data.query));
                        _country = data.country;
                        _countryCode = data.countryCode;
                    }
                }
                else if (json.Contains("timezone"))
                {
                    var data = JsonUtility.FromJson<IPInfoJson>(json);
                    if (data != null)
                    {
                        var city = data.city;
                        var region = data.region;
                        _location = Join(city != region ? Join(city, region) : city, data.ip);
                        _countryCode = data.country;
                        _country = CountryHelper.GetCountryName(_countryCode);
                    }
                }
                else
                {
                    var data = JsonUtility.FromJson<IPInfoLiteJson>(json);
                    if (data != null)
                    {
                        _location = Join(data.continent, data.ip);
                        _country = data.country;
                        _countryCode = data.country_code;
                    }
                }
                //Log.Debug($"location=\"{_location}\", country=\"{_country}\", countryCode=\"{_countryCode}\"");
            }
            catch (Exception e)
            {
                LegacyLog.Warning(e);
            }
        }

        static string Join(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return s2;
            if (string.IsNullOrEmpty(s2)) return s1;
            return $"{s1}_{s2}";
        }
    }
}