#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;

namespace Anvil.Legacy
{
    public class SettingsWindow : WindowBehaviour<SettingsWindow>
    {
        static readonly string KeyAutoUpdate = "settings_auto_update";

        [MenuItem("Window/Settings %t")]
        static void _Show()
        {
            ShowWindow();
        }

        List<Symbol> _symbols;
        List<Symbol> Symbols => _symbols ??= new()
        {
            new Symbol("DEBUG_MODE"),
            new Symbol("DOTWEEN"),

        };

        class Symbol
        {
            string _name;
            string _desc;
            bool _isOn;

            public string Name => _name;

            public bool On
            {
                get => _isOn;
                set => _isOn = value;
            }

            /// <summary>
            /// Separator
            /// </summary>
            public Symbol()
            {

            }

            public Symbol(string name)
            {
                _name = name;

                int length = name.Length;
                _desc = StringHelper.GetString(length, chars =>
                {
                    chars[0] = name[0];
                    for (int i = 1; i < length; i++)
                    {
                        char c = name[i];
                        if (c == '_')
                        {
                            chars[i++] = ' ';
                            chars[i] = name[i];
                        }
                        else
                        {
                            chars[i] = char.ToLower(c);
                        }
                    }
                });
            }

            public Symbol(string name, string desc)
            {
                _name = name;
                _desc = desc;
            }

            /// <summary>
            /// Returns true if state changed.
            /// </summary>
            public bool OnGUI()
            {
                if (_name == null)
                {
                    GUIHelper.Line();
                    return false;
                }

                return GUIHelper.CheckToggle(ref _isOn, _desc);
            }
        }

        BuildTargetGroup _targetGroup = BuildTargetGroup.Unknown;
        string _defineSymbols;
        bool _isAutoUpdate;
        bool _isEmpty;
        bool _isDirty;
        Vector2 _scrollPos;

        protected override void Init()
        {
            base.Init();
            _isAutoUpdate = GetBool(KeyAutoUpdate);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadDefineSymbols();
        }

        void LoadDefineSymbols()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            _targetGroup = BuildPipeline.GetBuildTargetGroup(target);
            _defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_targetGroup).Trim();
            SetSymbols(_defineSymbols);
        }

        void SetSymbols(string defineSymbols)
        {
            var symbols = Symbols;
            int symbolCount = symbols.Count;
            for (int i = 0; i < symbolCount; i++)
            {
                symbols[i].On = false;
            }

            _isEmpty = true;
            _isDirty = _defineSymbols != defineSymbols;

            StringHelper.Browse(defineSymbols, ';', defineSymbol =>
            {
                if (!defineSymbol.IsNullOrWhiteSpace())
                {
                    bool found = false;
                    for (int i = 0; i < symbolCount; i++)
                    {
                        var symbol = symbols[i];
                        if (symbol.Name == defineSymbol)
                        {
                            symbol.On = true;
                            found = true;
                            _isEmpty = false;
                            break;
                        }
                    }

                    if (!found)
                    {
                        LegacyLog.Warning($"Skip <b>{defineSymbol}</b>");
                    }
                }
            });
        }

        string GetDefineSymbols(List<Symbol> symbols)
        {
            var defineSymbols = "";
            int symbolCount = symbols.Count;
            for (int i = 0; i < symbolCount; i++)
            {
                var symbol = symbols[i];
                if (symbol.On)
                {
                    if (defineSymbols.IsNullOrEmpty())
                    {
                        defineSymbols = symbol.Name;
                    }
                    else
                    {
                        defineSymbols = $"{defineSymbols};{symbol.Name}";
                    }
                }
            }

            return defineSymbols;
        }

        void SetDefineSymbols(List<Symbol> symbols)
        {
            SetDefineSymbols(GetDefineSymbols(symbols));
        }

        void SetDefineSymbols(string defineSymbols)
        {
            _defineSymbols = defineSymbols;
            _isEmpty = defineSymbols.IsNullOrEmpty();
            _isDirty = false;
            //PlayerSettings.SetScriptingDefineSymbolsForGroup(_targetGroup, defineSymbols);
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(_targetGroup), defineSymbols);
        }

        void OnGUI()
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = guiEnabled && !EditorApplication.isCompiling;
            GUIHelper.LayoutRight(() =>
            {
                if (GUIHelper.CheckToggle(ref _isAutoUpdate, "Auto Update"))
                {
                    SetBool(KeyAutoUpdate, _isAutoUpdate);
                    if (_isAutoUpdate)
                    {
                        if (_isDirty)
                        {
                            SetDefineSymbols(Symbols);
                        }
                    }
                }
            });

            OnGUISymbols();
            GUI.enabled = guiEnabled;
        }

        void OnGUISymbols()
        {
            bool guiEnabled = GUI.enabled;
            Label("Define Symbols:");
            GUIHelper.Line();

            var symbols = Symbols;
            bool isChanged = false;
            GUIHelper.LayoutLeft(10, () =>
            {
                GUIHelper.LayoutVertical(ref _scrollPos, () =>
                {
                    int symbolCount = symbols.Count;
                    for (int i = 0; i < symbolCount; i++)
                    {
                        if (symbols[i].OnGUI())
                        {
                            isChanged = true;
                        }
                    }
                    GUIHelper.Line();
                });
            });

            if (isChanged)
            {
                var defineSymbols = GetDefineSymbols(symbols);
                if (_isAutoUpdate)
                {
                    SetDefineSymbols(defineSymbols);
                }
                else
                {
                    _isEmpty = defineSymbols.IsNullOrEmpty();
                    _isDirty = _defineSymbols != defineSymbols;
                }
            }

            Space();
            Line();
            GUIHelper.LayoutCenter(() =>
            {
                GUI.enabled = guiEnabled && _isDirty;
                if (Button("Update"))
                {
                    SetDefineSymbols(symbols);
                }

                GUI.enabled = guiEnabled && !_isEmpty;
                if (Button("Clear"))
                {
                    SetSymbols("");
                }

                GUI.enabled = guiEnabled;
                if (Button("Reload"))
                {
                    LoadDefineSymbols();
                }
            });
            Line();

            GUI.enabled = guiEnabled;
        }
    }
}
#endif