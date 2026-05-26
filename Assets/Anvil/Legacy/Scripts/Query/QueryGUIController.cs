using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public class QueryGUIController<T> : IQueryGUIController
    {
        Type _type;
        BaseGUIController<T> _guiController;
        bool _isEnabled;

        public BaseGUIController<T> GUIController => _guiController;
        public bool Enabled => _isEnabled;
        public T Value => _guiController.Value;

        public QueryGUIController(BaseGUIController<T> guiController, bool enabled = false)
        {
            _type = typeof(T);
            _guiController = guiController;
            _isEnabled = enabled;
            guiController.Nested = true;
        }

        public bool IsMatch(bool value)
        {
            if (!_isEnabled) return true;

            if (_type == typeof(bool))
            {
                return _guiController.Value.Equals(value);
            }

            LegacyLog.Warning($"{_guiController.LabelContent.text} ({_type}): Can't compare with {value}!");
            return false;
        }

        public void OnGUI()
        {
            GUIHelper.OnGUI(_guiController, ref _isEnabled);
        }
    }
}