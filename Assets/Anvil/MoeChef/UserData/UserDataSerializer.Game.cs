using System;
using System.Collections.Generic;
using Anvil.Legacy;

namespace Anvil
{
    public partial class UserDataSerializer
    {
        private static List<IBaseDataController> _dataControllers;

        public static void RegisterDataController(IBaseDataController controller)
        {
            if (_dataControllers == null)
            {
                _dataControllers = new List<IBaseDataController>();
            }

            _dataControllers.Add(controller);
        }

        private static LocalIntDataController CreateLocalIntData(string key, int defaultValue = 0, int priority = 30)
        {
            var controller = new LocalIntDataController(key, defaultValue, priority);
            // RegisterDataController(controller);
            return controller;
        }

        private static LocalBoolDataController CreateLocalBoolData(string key, bool defaultValue = false, int priority = 30)
        {
            var controller = new LocalBoolDataController(key, defaultValue, priority);
            // RegisterDataController(controller);
            return controller;
        }

        private static LocalDateTimeDataController CreateLocalDateTimeData(string key, DateTime? defaultValue = null, int priority = 30)
        {
            var controller = new LocalDateTimeDataController(key, defaultValue, priority);
            // RegisterDataController(controller);
            return controller;
        }

        private static LocalEnumHashSetDataController<TEnum> CreateLocalEnumHashSetDataController<TEnum>(string key, int priority = 30)
            where TEnum : struct, Enum
        {
            var controller = new LocalEnumHashSetDataController<TEnum>(key, priority);
            // RegisterDataController(controller);
            return controller;
        }

        private static LocalHashSetDataController<string> CreateLocalStringHashSetDataController(string key, int priority = 30)
        {
            var controller = new LocalHashSetDataController<string>(key, priority);
            // RegisterDataController(controller);
            return controller;
        }

        public static LocalBoolDataController _firstTime = CreateLocalBoolData(CommonKeys.FirstTime, true);

        public static bool FirstTime
        {
            get => _firstTime.Value;
            set => _firstTime.Value = value;
        }
        private static LocalIntDataController _currentLevel = CreateLocalIntData(CommonKeys.CurrentLevel, 1);

        public static int CurrentLevel
        {
            get => _currentLevel.Value;
            set => _currentLevel.Value = value;
        }

#region DebugInfo
        private static void InitializeDebug()
        {
            Anvil.Debugger.Initialize();
        }
#endregion
#if DEBUG_MODE
        public static void OnGUIDebug()
        {
            // _maxLevel.OnGUI(value =>
            // {
            //     SetMaxLevel(value);
            // });
           
            GUIHelper.Line();
        }
#endif

    }
}
