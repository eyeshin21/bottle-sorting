#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class DebugGUIManager
    {
        List<DebugGUIGroupController> _groupControllers = new();
        GUITabController _tabController;

        string _search;
        List<IDebugGUIController> _searchControllers = new();
        bool _isSearching;

        public DebugGUIManager(string tabKey, params DebugGUIGroupController[] groupControllers)
        {
            _groupControllers.AddRange(groupControllers);

            int count = groupControllers.Length;
            var names = new string[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = groupControllers[i].Name;
            }
            _tabController = new GUITabController(names);
            _tabController.CurrentTab = LocalPrefs.GetInt(tabKey);
            _tabController.onTabChanged += tab => LocalPrefs.SetInt(tabKey, tab);
        }

        public void AddGUI(DebugGUIGroupController groupController)
        {
            _groupControllers.Add(groupController);
            _tabController.AddTab(groupController.Name);
        }

        public void AddGUI(params DebugGUIGroupController[] groupControllers)
        {
            _groupControllers.AddRange(groupControllers);
            _tabController.AddTabs(groupControllers, groupController => groupController.Name);
        }

        public void AddGUI(IEnumerable<DebugGUIGroupController> groupControllers)
        {
            _groupControllers.AddRange(groupControllers);
            _tabController.AddTabs(groupControllers, groupController => groupController.Name);
        }

        public void OnGUI()
        {
            // Search
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Search");
                var search = GUILayout.TextField(_search, GUILayout.Width(500));
                if (search != _search)
                {
                    _search = search;
                    UpdateSearchControllers();
                    _isSearching = !string.IsNullOrWhiteSpace(search);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    if (GUILayout.Button("Clear"))
                    {
                        _search = "";
                        _searchControllers.Clear();
                        _isSearching = false;
                    }
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUIHelper.Line();

            if (_isSearching)
            {
                int searchCount = _searchControllers.Count;
                for (int i = 0; i < searchCount; i++)
                {
                    _searchControllers[i].OnGUI();
                }
                return;
            }

            // Tabs
            int currentTab = _tabController.OnGUI();
            GUIHelper.TabLine();
            var groupController = _groupControllers.TryGet(currentTab);
            if (groupController != null)
            {
                groupController.OnGUI();
            }
        }

        void UpdateSearchControllers()
        {
            _searchControllers.Clear();
            if (!string.IsNullOrWhiteSpace(_search))
            {
                var search = _search.ToLowerInvariant();
                foreach (var groupController in _groupControllers)
                {
                    if (groupController != null)
                    {
                        foreach (var controller in groupController.Controllers)
                        {
                            if (controller.Contains(search))
                            {
                                _searchControllers.Add(controller);
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif