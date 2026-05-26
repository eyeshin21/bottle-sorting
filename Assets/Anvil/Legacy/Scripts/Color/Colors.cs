using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static class Colors
    {
        /// <summary>
        /// "r_g_b_..."
        /// </summary>
        static Color[] GetColors(string s)
        {
            var colors = new List<Color>();
            int length = s.Length;
            float inverse = 1f / 255f;
            int r = 0, g = 0;
            int count = 0;
            int lastIndex = 0;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == '_')
                {
                    int n = s.Substring(lastIndex, i - lastIndex).ToInt();
                    lastIndex = i + 1;

                    count++;
                    if (count == 1)
                    {
                        r = n;
                    }
                    else if (count == 2)
                    {
                        g = n;
                    }
                    else
                    {
                        colors.Add(new Color(r * inverse, g * inverse, n * inverse, 1));
                        count = 0;
                    }
                }
            }
            Assert.IsEquals(count, 2);
            int b = s.Substring(lastIndex).ToInt();
            colors.Add(new Color(r * inverse, g * inverse, b * inverse, 1));
            return colors.ToArray();
        }

        static Color[] _redColors;
        static Color[] RedColors => _redColors ??= GetColors("205_92_92_240_128_128_250_128_114_233_150_122_255_160_122_220_20_60_255_0_0_178_34_34_139_0_0");

        static Color[] _pinkColors;
        static Color[] PinkColors => _pinkColors ??= GetColors("255_192_203_255_182_193_255_105_180_255_20_147_199_21_133_219_112_147");

        static Color[] _orangeColors;
        static Color[] OrangeColors => _orangeColors ??= GetColors("255_160_122_255_127_80_255_99_71_255_69_0_255_140_0_255_165_0");

        static Color[] _yellowColors;
        static Color[] YellowColors => _yellowColors ??= GetColors("255_215_0_255_255_0_255_255_224_255_250_205_250_250_210_255_239_213_255_228_181_255_218_185_238_232_170_240_230_140_189_183_107");

        static Color[] _purpleColors;
        static Color[] PurpleColors => _purpleColors ??= GetColors("230_230_250_216_191_216_221_160_221_238_130_238_218_112_214_255_0_255_255_0_255_186_85_211_147_112_219_102_51_153_138_43_226_148_0_211_153_50_204_139_0_139_128_0_128_75_0_130_106_90_205_72_61_139_123_104_238");

        static Color[] _greenColors;
        static Color[] GreenColors => _greenColors ??= GetColors("173_255_47_127_255_0_124_252_0_0_255_0_50_205_50_152_251_152_144_238_144_0_250_154_0_255_127_60_179_113_46_139_87_34_139_34_0_128_0_0_100_0_154_205_50_107_142_35_128_128_0_85_107_47_102_205_170_143_188_139_32_178_170_0_139_139_0_128_128");

        static Color[] _blueColors;
        static Color[] BlueColors => _blueColors ??= GetColors("0_255_255_0_255_255_224_255_255_175_238_238_127_255_212_64_224_208_72_209_204_0_206_209_95_158_160_70_130_180_176_196_222_176_224_230_173_216_230_135_206_235_135_206_250_0_191_255_30_144_255_100_149_237_123_104_238_65_105_225_0_0_255_0_0_205_0_0_139_0_0_128_25_25_112");

        static Color[] _brownColors;
        static Color[] BrownColors => _brownColors ??= GetColors("255_248_220_255_235_205_255_228_196_255_222_173_245_222_179_222_184_135_210_180_140_188_143_143_244_164_96_218_165_32_184_134_11_205_133_63_210_105_30_139_69_19_160_82_45_165_42_42_128_0_0");

        static Color[] _whiteColors;
        static Color[] WhiteColors => _whiteColors ??= GetColors("255_255_255_255_250_250_240_255_240_245_255_250_240_255_255_240_248_255_248_248_255_245_245_245_255_245_238_245_245_220_253_245_230_255_250_240_255_255_240_250_235_215_250_240_230_255_240_245_255_228_225");

        static Color[] _grayColors;
        static Color[] GrayColors => _grayColors ??= GetColors("220_220_220_211_211_211_192_192_192_169_169_169_128_128_128_105_105_105_119_136_153_112_128_144_47_79_79_0_0_0");

        public static ColorFactory CreateColorFactory()
        {
            return new ColorFactory
            (
                RedColors,
                PinkColors,
                OrangeColors,
                YellowColors,
                PurpleColors,
                GreenColors,
                BlueColors,
                BrownColors,
                WhiteColors,
                GrayColors
            );
        }

        public static Color Get(string name)
        {
            if (name == ColorNames.Black) return Color.black;
            if (name == ColorNames.Blue) return Color.blue;
            if (name == ColorNames.Cyan) return Color.cyan;
            if (name == ColorNames.Gray) return Color.gray;
            if (name == ColorNames.Green) return Color.green;
            if (name == ColorNames.Grey) return Color.grey;
            if (name == ColorNames.Magenta) return Color.magenta;
            if (name == ColorNames.Red) return Color.red;
            if (name == ColorNames.White) return Color.white;
            if (name == ColorNames.Yellow) return Color.yellow;
            LegacyLog.Todo(name);
            return Color.white;
        }

#if UNITY_EDITOR
        /// <summary>
        /// "name_..."
        /// </summary>
        static string[] GetNames(string s)
        {
            var names = new List<string>();
            int length = s.Length;
            int lastIndex = 0;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == '_')
                {
                    names.Add(s.Substring(lastIndex, i - lastIndex));
                    lastIndex = i + 1;
                }
            }
            names.Add(s.Substring(lastIndex));
            return names.ToArray();
        }

        static string[] _redNames;
        static string[] RedNames => _redNames ??= GetNames("IndianRed_LightCoral_Salmon_DarkSalmon_LightSalmon_Crimson_Red_FireBrick_DarkRed");

        static string[] _pinkNames;
        static string[] PinkNames => _pinkNames ??= GetNames("Pink_LightPink_HotPink_DeepPink_MediumVioletRed_PaleVioletRed");

        static string[] _orangeNames;
        static string[] OrangeNames => _orangeNames ??= GetNames("LightSalmon_Coral_Tomato_OrangeRed_DarkOrange_Orange");

        static string[] _yellowNames;
        static string[] YellowNames => _yellowNames ??= GetNames("Gold_Yellow_LightYellow_LemonChiffon_LightGoldenrodYellow_PapayaWhip_Moccasin_PeachPuff_PaleGoldenrod_Khaki_DarkKhaki");

        static string[] _purpleNames;
        static string[] PurpleNames => _purpleNames ??= GetNames("Lavender_Thistle_Plum_Violet_Orchid_Fuchsia_Magenta_MediumOrchid_MediumPurple_RebeccaPurple_BlueViolet_DarkViolet_DarkOrchid_DarkMagenta_Purple_Indigo_SlateBlue_DarkSlateBlue_MediumSlateBlue");

        static string[] _greenNames;
        static string[] GreenNames => _greenNames ??= GetNames("GreenYellow_Chartreuse_LawnGreen_Lime_LimeGreen_PaleGreen_LightGreen_MediumSpringGreen_SpringGreen_MediumSeaGreen_SeaGreen_ForestGreen_Green_DarkGreen_YellowGreen_OliveDrab_Olive_DarkOliveGreen_MediumAquamarine_DarkSeaGreen_LightSeaGreen_DarkCyan_Teal");

        static string[] _blueNames;
        static string[] BlueNames => _blueNames ??= GetNames("Aqua_Cyan_LightCyan_PaleTurquoise_Aquamarine_Turquoise_MediumTurquoise_DarkTurquoise_CadetBlue_SteelBlue_LightSteelBlue_PowderBlue_LightBlue_SkyBlue_LightSkyBlue_DeepSkyBlue_DodgerBlue_CornflowerBlue_MediumSlateBlue_RoyalBlue_Blue_MediumBlue_DarkBlue_Navy_MidnightBlue");

        static string[] _brownNames;
        static string[] BrownNames => _brownNames ??= GetNames("Cornsilk_BlanchedAlmond_Bisque_NavajoWhite_Wheat_BurlyWood_Tan_RosyBrown_SandyBrown_Goldenrod_DarkGoldenrod_Peru_Chocolate_SaddleBrown_Sienna_Brown_Maroon");

        static string[] _whiteNames;
        static string[] WhiteNames => _whiteNames ??= GetNames("White_Snow_HoneyDew_MintCream_Azure_AliceBlue_GhostWhite_WhiteSmoke_SeaShell_Beige_OldLace_FloralWhite_Ivory_AntiqueWhite_Linen_LavenderBlush_MistyRose");

        static string[] _grayNames;
        static string[] GrayNames => _grayNames ??= GetNames("Gainsboro_LightGray_Silver_DarkGray_Gray_DimGray_LightSlateGray_SlateGray_DarkSlateGray_Black");

        static void ParseGroup(string s, string name, out string groupName, out string colorNames, out string rgbs)
        {
            groupName = GetTagValue(s, name, 0).GetFirstWord();
            colorNames = "";
            rgbs = "";

            int startIndex = name.Length;
            do
            {
                name = "<td class=\"color-table__cell color-table__cell--name\">";
                int index = s.IndexOf(name, startIndex);
                if (index > 0)
                {
                    var colorName = GetTagValue(s, name, index);
                    //Log.Debug($"Name=\"{colorName}\"");
                    colorNames = $"{colorNames}_{colorName}";
                    startIndex = index + name.Length;
                    name = "<td class=\"color-table__cell color-table__cell--hex\">";
                    index = s.IndexOf(name, startIndex);
                    if (index > 0)
                    {
                        //Log.Debug($"Hex=\"{GetTagValue(html, name, index)}\"");
                        startIndex = index + name.Length;
                        name = "<td class=\"color-table__cell color-table__cell--rgb\">";
                        index = s.IndexOf(name, startIndex);
                        if (index > 0)
                        {
                            var rgb = GetTagValue(s, name, index);
                            rgbs = $"{rgbs}_{GetRGB(rgb)}";
                            //Log.Debug($"RGB=\"{rgb}\"");
                            startIndex = index + name.Length;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (true);

            // Remove '_'
            colorNames = colorNames.Substring(1);
            rgbs = rgbs.Substring(1);
        }

        static string GetTagValue(string s, string name, int index)
        {
            int startIndex = s.IndexOf('>', index + name.Length - 1);
            if (startIndex > 0)
            {
                int endIndex = s.IndexOf('<', startIndex + 1);
                if (endIndex > 0)
                {
                    return s.GetSubstring(startIndex + 1, endIndex - 1);
                }
            }
            return "";
        }

        /// <summary>
        /// "rgb(r, g, b)" => "r_g_b"
        /// </summary>
        static string GetRGB(string s)
        {
            int index = s.IndexOf('(');
            if (index > 0)
            {
                int index2 = s.IndexOf(',', index + 1);
                if (index2 > 0)
                {
                    var r = s.Substring(index + 1, index2 - index - 1).Trim();
                    index = s.IndexOf(',', index2 + 1);
                    if (index > 0)
                    {
                        var g = s.Substring(index2 + 1, index - index2 - 1).Trim();
                        index2 = s.IndexOf(')');
                        if (index2 > 0)
                        {
                            var b = s.Substring(index + 1, index2 - index - 1).Trim();
                            return $"{r}_{g}_{b}";
                        }
                    }
                }
            }
            return "";
        }

        static void ImportColors(string html)
        {
            var sb = new System.Text.StringBuilder();
            string name = "color-group__title";
            int lastIndex = -1;
            int startIndex = 0;
            int index;
            do
            {
                index = html.IndexOf(name, startIndex);
                if (index > 0)
                {
                    if (lastIndex > 0)
                    {
                        ParseGroup(html.GetSubstring(lastIndex, index - 1), name, out string groupName, out string colorNames, out string rgbs);
                        sb.AppendLine($"#{groupName}");
                        sb.AppendLine($"\"{colorNames}\"");
                        sb.AppendLine($"\"{rgbs}\"");
                    }
                    lastIndex = index;
                    startIndex = index + 1;
                }
                else
                {
                    // Last
                    if (lastIndex > 0)
                    {
                        ParseGroup(html.Substring(lastIndex), name, out string groupName, out string colorNames, out string rgbs);
                        sb.AppendLine($"#{groupName}");
                        sb.AppendLine($"\"{colorNames}\"");
                        sb.AppendLine($"\"{rgbs}\"");
                    }
                    break;
                }
            }
            while (true);

            var s = sb.ToString();
            s.CopyToClipboard();
            LegacyLog.Debug(s);
        }

        [MenuItem("Import/Colors")]
        static void ImportColors()
        {
            // https://htmlcolorcodes.com/color-names/
            GUIHelper.ShowInputString("Import Colors", "HTML", "", html =>
            {
                if (!string.IsNullOrWhiteSpace(html))
                {
                    ImportColors(html);
                    return true;
                }
                return false;
            });
        }

        class ColorData
        {
            string _name;
            Color _color;
            string _hexadecimal;
            string _rgb;

            public string Name => _name;
            public Color Color => _color;
            public string Hexadecimal => _hexadecimal;
            public string RGB => _rgb;

            public ColorData(string name, Color color)
            {
                int r = Helper.RoundToInt(color.r * 255);
                int g = Helper.RoundToInt(color.g * 255);
                int b = Helper.RoundToInt(color.b * 255);
                _name = Helper.GetNicifyName(name);
                _color = color;
                _hexadecimal = color.ToHexadecimalString();
                _rgb = $"({r}, {g}, {b})";
            }

            public override string ToString()
            {
                return $"\"{_name}\": hexadecimal={_hexadecimal}, rgb={_rgb}";
            }
        }

        class GroupColorData
        {
            string _name;
            Color _color;
            List<ColorData> _colorDatas;

            public string Name => _name;
            public Color Color => _color;
            public List<ColorData> ColorDatas => _colorDatas;

            static List<ColorData> GetColorDatas(string[] colorNames, Color[] colors)
            {
                int nameCount = colorNames.Length;
                int colorCount = colors.Length;
                Assert.IsEquals(nameCount, colorCount);

                int count = Mathf.Min(nameCount, colorCount);
                var colorDatas = new List<ColorData>(count);
                for (int i = 0; i < count; i++)
                {
                    colorDatas.Add(new ColorData(colorNames[i], colors[i]));
                }
                return colorDatas;
            }

            public GroupColorData(string name, List<ColorData> colorDatas)
            {
                _name = name;
                _colorDatas = colorDatas;
                foreach (var colorData in colorDatas)
                {
                    if (colorData.Name == name)
                    {
                        _color = colorData.Color;
                        break;
                    }
                }
            }

            public GroupColorData(string groupName, string[] colorNames, Color[] colors) : this(groupName, GetColorDatas(colorNames, colors))
            {

            }

            public override string ToString()
            {
                return $"#{_name}: colorCount={_colorDatas.GetCount()}";
            }
        }

        static List<GroupColorData> GetGroupColorDatas()
        {
            return new List<GroupColorData>()
            {
                new GroupColorData("Red", RedNames, RedColors),
                new GroupColorData("Pink", PinkNames, PinkColors),
                new GroupColorData("Orange", OrangeNames, OrangeColors),
                new GroupColorData("Yellow", YellowNames, YellowColors),
                new GroupColorData("Purple", PurpleNames, PurpleColors),
                new GroupColorData("Green", GreenNames, GreenColors),
                new GroupColorData("Blue", BlueNames, BlueColors),
                new GroupColorData("Brown", BrownNames, BrownColors),
                new GroupColorData("White", WhiteNames, WhiteColors),
                new GroupColorData("Gray", GrayNames, GrayColors),
            };
        }

        static List<GroupColorData> _groupColorDatas;
        static List<GroupColorData> GroupColorDatas => _groupColorDatas ??= GetGroupColorDatas();

        static GUIStyle _groupNameStyle;
        static Vector2 _scrollPos;

        public static void OnGUIColors()
        {
            if (_groupNameStyle == null)
            {
                _groupNameStyle = GUIHelper.CreateLabelStyle().SetFontSize(18).SetBold();
            }

            var box = GUIContent.none;
            var nameWidth = GUILayout.Width(135);
            var colorWidth = GUILayout.Width(100);
            var hexaWidth = GUILayout.Width(60);
            var rgbWidth = GUILayout.Width(100);
            var guiColor = GUI.color;

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            var groupColorDatas = GroupColorDatas;
            int groupCount = groupColorDatas.Count;
            //int totalColor = 0;
            for (int i = 0; i < groupCount; i++)
            {
                var groupColorData = groupColorDatas[i];
                var colorDatas = groupColorData.ColorDatas;
                int colorCount = colorDatas.Count;
                //totalColor += colorCount;
                GUILayout.BeginHorizontal();
                {
                    GUI.color = groupColorData.Color;
                    GUILayout.Box(box, GUILayout.Width(20));
                    GUI.color = guiColor;
                    GUILayout.Label($"{groupColorData.Name}", _groupNameStyle);
                    GUILayout.Label($"({colorCount})");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUIHelper.Line();
                for (int j = 0; j < colorCount; j++)
                {
                    var colorData = colorDatas[j];
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(colorData.Name, nameWidth);
                        GUI.color = colorData.Color;
                        GUILayout.Box(box, colorWidth);
                        GUI.color = guiColor;
                        //GUILayout.TextField(colorData.Hexadecimal, hexaWidth);
                        //GUILayout.TextField(colorData.RGB, rgbWidth);
                        if (GUILayout.Button(colorData.Hexadecimal, hexaWidth))
                        {
                            colorData.Hexadecimal.CopyToClipboard();
                        }
                        if (GUILayout.Button(colorData.RGB, rgbWidth))
                        {
                            colorData.RGB.CopyToClipboard();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUIHelper.Line();
            }
            GUILayout.EndScrollView();

            //GUILayout.Label($"Total color: {totalColor}"); // 143
        }
#endif
    }
}