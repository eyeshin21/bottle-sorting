#if DEBUG_MODE || UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class GUITabController
    {
        class TabController
        {
            GUIContent _content;
            float _width;
            GUILayoutOption _optionWidth;

            public GUIContent Content => _content;

            public float Width
            {
                get => _width;
                set
                {
                    _width = value;
                    _optionWidth = GUILayout.Width(value);
                }
            }

            public bool NewRow { get; set; }

            static GUIStyle _tabStyle;
            static GUIStyle TabStyle => _tabStyle ??= GUIHelper.CreateButtonStyle().SetMarginHorizontal(0);

            public TabController(string name)
            {
                _content = new GUIContent(name);
            }

            public TabController(GUIContent content)
            {
                _content = content;
            }

            public void OnGUISelected(GUIStyle style)
            {
                if (_width > 0)
                {
                    GUILayout.Label(_content, style, _optionWidth);
                }
                else
                {
                    GUILayout.Label(_content, style);
                }
            }

            public bool OnGUI()
            {
                return _width > 0 ? GUILayout.Button(_content, TabStyle, _optionWidth) : GUILayout.Button(_content, TabStyle);
            }
        }

        List<TabController> _tabControllers = new();
        GUIStyle _selectedTabStyle;
        WindowWidthTracker _windowWidthTracker;
        bool _isMultipleRow;
        int _currentTab; // Index
        bool _isInited;

#if UNITY_EDITOR
        public EditorWindow Window { get; set; }
#endif
        public GUIStyle GroupStyle { get; set; }
        public event Listener<int> onTabChanged;

        public int TabCount => _tabControllers.Count;

        public int CurrentTab
        {
            get => _currentTab;
            set
            {
                if (_currentTab != value)
                {
                    _currentTab = value;
                    onTabChanged?.Invoke(value);
                }
            }
        }

        public GUITabController(Type enumType) : this(Enum.GetNames(enumType))
        {

        }

        public GUITabController(params string[] names)
        {
            foreach (var name in names)
            {
                _tabControllers.Add(new TabController(name));
            }
        }

        public GUITabController(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                _tabControllers.Add(new TabController(name));
            }
        }

        public void AddTab(string name)
        {
            _tabControllers.Add(new TabController(name));
        }

        public void AddTabs(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                _tabControllers.Add(new TabController(name));
            }
        }

        public void AddTabs<T>(IEnumerable<T> objs, Func<T, string> nameFunc)
        {
            foreach (var obj in objs)
            {
                _tabControllers.Add(new TabController(nameFunc(obj)));
            }
        }

        static GUIStyle _defaultSelectedTabStyle;
        static GUIStyle DefaultSelectedTabStyle => _defaultSelectedTabStyle ??= GUIHelper.CreateButtonStyle().SetTextColor(Color.yellow);

        static GUIStyle _windowSelectedTabStyle;
        static GUIStyle WindowSelectedTabStyle => _windowSelectedTabStyle ??= GUIHelper.CreateButtonStyle().SetTextColor(Color.blue).SetBold().SetMarginHorizontal(0);

        void SetTabStyle(GUIStyle tabStyle)
        {
            int count = _tabControllers.Count;
            for (int i = 0; i < count; i++)
            {
                var tabController = _tabControllers[i];
                tabController.Width = Mathf.Max(tabStyle.CalcSize(tabController.Content).x, 150);
            }
            _selectedTabStyle = tabStyle;
        }

        void SetWidth(float width)
        {
            float maxWidth = width;
            width = 0;
            _isMultipleRow = false;
            int count = _tabControllers.Count;
            for (int i = 0; i < count; i++)
            {
                var tabController = _tabControllers[i];
                float tabWidth = tabController.Width;
                bool newRow = false;
                width += tabWidth;
                if (i > 0)
                {
                    if (width > maxWidth)
                    {
                        width = tabWidth;
                        newRow = true;
                        _isMultipleRow = true;
                    }
                }
                tabController.NewRow = newRow;
            }
        }

        /// <summary>
        /// Returns current tab index.
        /// </summary>
        public int OnGUI()
        {
            if (!_isInited)
            {
                _isInited = true;
                GUIStyle tabStyle;
                float width;
#if UNITY_EDITOR
                var window = Window;
                if (window != null)
                {
                    tabStyle = WindowSelectedTabStyle;
                    _windowWidthTracker = new WindowWidthTracker(window);
                    width = _windowWidthTracker.WindowWidth;
                }
                else
#endif
                {
                    tabStyle = DefaultSelectedTabStyle;
                    width = Screen.width;
                }
                SetTabStyle(tabStyle);
                SetWidth(width);
            }
            else
            {
                if (_windowWidthTracker != null && _windowWidthTracker.Update())
                {
                    SetWidth(_windowWidthTracker.WindowWidth);
                }
            }

            var groupStyle = GroupStyle;
            int tabCount = _tabControllers.Count;
            if (_isMultipleRow)
            {
                if (groupStyle != null)
                {
                    GUILayout.BeginVertical(groupStyle);
                }
                else
                {
                    GUILayout.BeginVertical();
                }
                GUILayout.BeginHorizontal();
                for (int i = 0; i < tabCount; i++)
                {
                    var tabController = _tabControllers[i];
                    if (tabController.NewRow)
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                    }
                    if (i == _currentTab)
                    {
                        tabController.OnGUISelected(_selectedTabStyle);
                    }
                    else if (tabController.OnGUI())
                    {
                        CurrentTab = i;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            else
            {
                if (groupStyle != null)
                {
                    GUILayout.BeginHorizontal(groupStyle);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                }
                {
                    for (int i = 0; i < tabCount; i++)
                    {
                        var tabController = _tabControllers[i];
                        if (i == _currentTab)
                        {
                            tabController.OnGUISelected(_selectedTabStyle);
                        }
                        else if (tabController.OnGUI())
                        {
                            CurrentTab = i;
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            return _currentTab;
        }
    }
}
#endif