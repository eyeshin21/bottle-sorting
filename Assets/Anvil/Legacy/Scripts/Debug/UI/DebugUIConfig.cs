#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugUIConfig : SingletonScriptableObject<DebugUIConfig>
    {
        [SerializeField] GameObject _title;
        [SerializeField] GameObject _button;
        [SerializeField] GameObject _buttonClose;
        [SerializeField] GameObject _buttonSettings;
        [SerializeField] GameObject _popupOption;
        [SerializeField] GameObject _popupInt;
        [SerializeField] GameObject _popupSettings;
        [SerializeField] GameObject _row;
        [SerializeField] GameObject _switch;
        [SerializeField] GameObject _buttonCycleText;

        public GameObject Title => _title;
        public GameObject Button => _button;
        public GameObject ButtonClose => _buttonClose;
        public GameObject ButtonSettings => _buttonSettings;
        public GameObject PopupOption => _popupOption;
        public GameObject PopupInt => _popupInt;
        public GameObject PopupSettings => _popupSettings;
        public GameObject Row => _row;
        public GameObject Switch => _switch;
        public GameObject ButtonCycleText => _buttonCycleText;
    }
}
#endif