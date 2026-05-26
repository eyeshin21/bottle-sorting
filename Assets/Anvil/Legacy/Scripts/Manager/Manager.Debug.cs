#if DEBUG_MODE
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public partial class Manager
    {
        class GroupController
        {
            string _name;
            Callback _guiCallback;

            public string Name => _name;

            public GroupController(string name)
            {
                _name = name;
            }

            public void AddCallback(Callback callback)
            {
                _guiCallback += callback;
            }

            public void RemoveCallback(Callback callback)
            {
                _guiCallback -= callback;
            }

            public void OnGUI()
            {
                _guiCallback?.Invoke();
            }
        }

        GUITabController _tabController;
        List<GroupController> _groupControllers = new();
        bool _isGUIInited;

        public static void AddDebugGUI<TGroup>(TGroup groupType, Callback callback) where TGroup : Enum
        {
            AddDebugGUI(groupType.ToString(), callback);
        }

        public static void RemoveDebugGUI<TGroup>(TGroup groupType, Callback callback) where TGroup : Enum
        {
            RemoveDebugGUI(groupType.ToString(), callback);
        }

        public static void AddDebugGUI(string groupName, Callback callback)
        {
            var groupControllers = Instance._groupControllers;
            var groupController = groupControllers.Get(item => item.Name == groupName);
            if (groupController == null)
            {
                groupController = new GroupController(groupName);
                groupControllers.Add(groupController);
            }
            groupController.AddCallback(callback);
        }

        public static void RemoveDebugGUI(string groupName, Callback callback)
        {
            if (_instance == null) return;
            var groupControllers = _instance._groupControllers;
            var groupController = groupControllers.Get(item => item.Name == groupName);
            if (groupController != null)
            {
                groupController.RemoveCallback(callback);
            }
            else
            {
               LegacyLog.Warning($"Group \"{groupName}\" not found!");
            }
        }

        static bool _isShowDebug;

        public static bool ShowDebug
        {
            get => _isShowDebug;
            set
            {
                _isShowDebug = value;
                if (value)
                {
                    InputEnabled = false;
                }
                else
                {
                    DelayCall(0.5f, () =>
                    {
                        InputEnabled = true;
                    });
                }
            }
        }

        public static void HideDebug()
        {
            ShowDebug = false;
        }

        public static void HideDebugAndReloadScene()
        {
            HideDebug();
            Helper.ReloadScene();
        }

        static bool IsToggleDebug()
        {
            return Input.GetKeyDown(KeyCode.D) && (Input.GetKey(KeyCode.LeftControl) ||
                                                   Input.GetKey(KeyCode.RightControl) ||
                                                   Input.GetKey(KeyCode.LeftCommand) ||
                                                   Input.GetKey(KeyCode.RightCommand));
        }

        /// <summary>
        /// Returns true if processed key.
        /// </summary>
        bool UpdateKey()
        {
            if (_isShowDebug)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || IsToggleDebug())
                {
                    ShowDebug = false;
                    return true;
                }
            }
            else
            {
                if (IsToggleDebug())
                {
                    ShowDebug = true;
                    return true;
                }
            }

            return false;
        }

        void UpdateDebug()
        {
            UpdateKey();
        }

        void OnGUI()
        {
            if (!_isGUIInited)
            {
                GUIHelper.LazyInit();
                AddDebugGUI("Debug", TimeHelper.OnGUIDebug);
                AddDebugGUI("Settings", DebugManager.OnGUIDebug);
                _isGUIInited = true;
            }

            if (!DebugEnabled) return;

            bool showDebug = GUIHelper.ToggleDebug(_isShowDebug);
            if (showDebug != _isShowDebug)
            {
                ShowDebug = showDebug;
            }
            if (!showDebug) return;

            var rect = GUIHelper.ScreenRect;
            GUIHelper.Box(rect);

            // Close button
            GUILayout.BeginArea(rect);
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", GUILayout.Width(50)))
                {
                    ShowDebug = false;
                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                // Tabs
                int groupCount = _groupControllers.Count;
                if (groupCount == 0) return;

                if (_tabController == null || _tabController.TabCount != groupCount)
                {
                    var groupNames = new string[groupCount];
                    for (int i = 0; i < groupCount; i++)
                    {
                        groupNames[i] = _groupControllers[i].Name;
                    }

                    var tabKey = "_managerTab";
                    var tabController = new GUITabController(groupNames);
                    tabController.CurrentTab = Mathf.Clamp(LocalPrefs.GetInt(tabKey), 0, groupCount - 1);
                    tabController.onTabChanged += tab => LocalPrefs.SetInt(tabKey, tab);
                    _tabController = tabController;
                }

                int tabIndex = _tabController.OnGUI();
                GUIHelper.Line();
                _groupControllers[tabIndex].OnGUI();

                // Bottom
                OnGUIBottom();
            }
            GUILayout.EndArea();
        }

        void OnGUIBottom()
        {
            GUILayout.FlexibleSpace();
            GUIHelper.Line();
            GUILayout.BeginHorizontal();
            {
                if (GUIHelper.Button("Reload Scene"))
                {
                    ShowDebug = false;
                    Helper.ReloadScene();
                }

                //if (GUIHelper.Button("Clear All"))
                //{
                //    ShowDebug = false;
                //    UserData.OnClickClearAll();
                //}

                //if (GUIHelper.Button("Delete Account"))
                //{
                //    ShowDebug = false;
                //    UserData.OnClickDeleteAccount();
                //}

                //if (GameManager.Active)
                //{
                //    if (Button("Quit"))
                //    {
                //        ShowDebug = false;
                //        GameManager.OnQuit();
                //    }
                //}

                GUILayout.FlexibleSpace();
                GUIHelper.Label(TimeHelper.CurrentDateTime.ToString2());
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(100);
        }

        void OnDrawGizmos()
        {
            _onDrawGizmos?.Invoke();
        }
    }
}
#endif