#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Anvil.Legacy
{
    public class DebugGUIGroupController : DebugGUIController
    {
        string _name;
        string _label;
        List<IDebugGUIController> _controllers = new();
        //Callback _appendLabelGUI;
        Callback _firstCallback;
        Callback _lastCallback;
        Vector2 _scrollPos;

        public string Name => _name;
        public List<IDebugGUIController> Controllers => _controllers;

        //public DebugGUIGroupController AppendLabelGUI(Callback callback)
        //{
        //    _appendLabelGUI += callback;
        //    return this;
        //}

        public DebugGUIGroupController SetFirstCallback(Callback callback)
        {
            _firstCallback = callback;
            return this;
        }

        public DebugGUIGroupController SetLastCallback(Callback callback)
        {
            _lastCallback = callback;
            return this;
        }

        public DebugGUIGroupController(string name)
        {
            _name = name;
        }

        public DebugGUIGroupController(string name, params IDebugGUIController[] controllers)
        {
            _name = name;
            _controllers.AddRange(controllers);
        }

        public DebugGUIGroupController(string name, params IBaseDataController[] controllers)
        {
            _name = name;
            foreach (var controller in controllers)
            {
                _controllers.Add(controller.DebugGUIController);
            }
        }

        //public DebugGroupGUIController(string label)
        //{
        //    _label = GetLabel(label);
        //}

        public DebugGUIGroupController Add(IDebugGUIController controller)
        {
            Assert.NotContains(_controllers, controller);
            _controllers.Add(controller);
            return this;
        }

        public DebugGUIGroupController Add<T>(IDataController<T> dataController)
        {
            IDebugGUIController controller;
            //var saveData = dataController.Value as SaveData;
            //if (saveData != null)
            //{
            //    //Log.Warning(Keys.GetName(dataController.Key));
            //    controller = saveData.GetDebugGUIController(Keys.GetName(dataController.Key));
            //}
            //else
            {
                controller = dataController.DebugGUIController;
            }

            if (controller != null)
            {
                //AddKey(dataController);
                Add(controller);
            }
            else
            {
               LegacyLog.Warning($"{dataController}: Missing GUI controller");
            }
            return this;
        }

        public DebugGUIGroupController Add<T>(params IDataController<T>[] dataControllers)
        {
            int count = dataControllers.Length;
            for (int i = 0; i < count; i++)
            {
                var controller = dataControllers[i].DebugGUIController;
                if (controller != null)
                {
                    //AddKey(dataControllers[i]);
                    Add(controller);
                }
                else
                {
                   LegacyLog.Warning($"{dataControllers[i]}: Missing GUI controller");
                }
            }
            return this;
        }

        public override bool Contains(string search)
        {
            return _label.Contains(search, StringComparison.InvariantCultureIgnoreCase);
        }

        public override void OnGUI()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);

            //if (_appendLabelGUI != null)
            //{
            //    GUILayout.BeginHorizontal();
            //    {
            //        Label(_label);
            //        _appendLabelGUI();
            //        GUILayout.FlexibleSpace();
            //    }
            //    GUILayout.EndHorizontal();
            //}
            //else
            //{
            //    Label(_label);
            //}

            _firstCallback?.Invoke();

            GUILayout.BeginHorizontal();
            {
                //GUILayout.Space(30);
                GUILayout.BeginVertical();
                {
                    int count = _controllers.Count;
                    for (int i = 0; i < count; i++)
                    {
                        _controllers[i].OnGUI();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            _lastCallback?.Invoke();

            GUILayout.EndScrollView();
        }
    }
}
#endif