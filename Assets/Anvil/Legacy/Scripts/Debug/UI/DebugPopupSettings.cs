#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class DebugPopupSettings : DebugPopupBehaviour
    {
        Button _saveButton;
        List<Func<bool>> _changedFuncs = new();

        void UpdateSaveButton(bool changed)
        {
            if (changed)
            {
                _saveButton.interactable = true;
            }
            else
            {
                foreach (var changedFunc in _changedFuncs)
                {
                    if (changedFunc())
                    {
                        return;
                    }
                }
                _saveButton.interactable = false;
            }
        }

        protected override Transform Content
        {
            get
            {
                if (_content == null)
                {
                    _content = Popup.GetChild("Content");
                    var layoutGroup = _content.AddComponent<VerticalLayoutGroup>();
                    layoutGroup.padding = new RectOffset(40, 40, 40, 40);
                    layoutGroup.spacing = 40;
                    layoutGroup.childAlignment = TextAnchor.UpperLeft;
                    layoutGroup.childControlWidth = layoutGroup.childControlHeight = false;
                    layoutGroup.childForceExpandWidth = layoutGroup.childForceExpandHeight = false;
                }
                return _content;
            }
        }

        static GameObject _rowPrefab;
        static GameObject RowPrefab => _rowPrefab ??= Config.Row;

        static GameObject _switchPrefab;
        static GameObject SwitchPrefab => _switchPrefab ??= Config.Switch;

        static GameObject _buttonCycleTextPrefab;
        static GameObject ButtonCycleTextPrefab => _buttonCycleTextPrefab ??= Config.ButtonCycleText;

        DebugRow CreateRow(string label)
        {
            var row = RowPrefab.Create<DebugRow>(Content);
            row.SetLabel(label);
            return row;
        }

        public DebugSwitch AddSwitch(string label, bool on)
        {
            var row = CreateRow(label);
            var @switch = SwitchPrefab.Create<DebugSwitch>(row.transform);
            @switch.Construct(on, () => UpdateSaveButton(@switch.StateChanged));
            _changedFuncs.Add(() => @switch.StateChanged);
            return @switch;
        }

        public DebugButtonCycleText AddButtonCycle<T>(string label, List<IntString> valueTexts, T value) where T : System.Enum
        {
            return AddButtonCycle(label, valueTexts, (int)(object)value);
        }

        public DebugButtonCycleText AddButtonCycle(string label, List<IntString> valueTexts, int value)
        {
            var row = CreateRow(label);
            var button = ButtonCycleTextPrefab.Create<DebugButtonCycleText>(row.transform);
            button.Construct(valueTexts, value, () => UpdateSaveButton(button.ValueChanged));
            _changedFuncs.Add(() => button.ValueChanged);
            return button;
        }

        public void Show(string title, string buttonSave, Callback saveCallback, Callback closeCallback = null)
        {
            AddTitle(title);
            AddButtonClose(() => Hide(closeCallback));
            _saveButton = AddButton(buttonSave, () => Hide(saveCallback));
            _saveButton.interactable = false;
            name = $"Debug{title.RemoveCharacters(' ')}";

            Content.ForceLayout();
            Show();
        }
    }
}
#endif