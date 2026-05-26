#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class DebugManager
    {
        static List<IDebugGUIController> _guiControllers;
        static void AddController(IDebugGUIController controller)
        {
            if (_guiControllers == null)
            {
                _guiControllers = new List<IDebugGUIController>();
            }
            _guiControllers.Add(controller);
        }

        public static DebugBoolGUIController CreateBoolController(string label, string key, bool defaultValue = false)
        {
            var controller = new DebugBoolGUIController(label, key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static DebugIntGUIController CreateIntController(string label, string key, int defaultValue = 0)
        {
            var controller = new DebugIntGUIController(label, key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static DebugStringGUIController CreateStringController(string label, string key, string defaultValue = "")
        {
            var controller = new DebugStringGUIController(label, key, defaultValue);
            AddController(controller);
            return controller;
        }
    }
}
#endif