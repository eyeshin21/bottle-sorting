using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class CommonUIConfig : SingletonScriptableObject<CommonUIConfig>
    {
        [SerializeField] private GameObject _uiItemSlotPrefab;
        [SerializeField] private MoveConfig _defaultMoveConfig;
        [SerializeField] private GameObject _smallIconIndicatorPrefab;
        [SerializeField] private GameObject _smallTextIndicatorPrefab;
        [SerializeField] private GameObject _defaultDebuggerUIPrefab;
        
        [SerializeField] private GameObject _rewardFxElementPrefab;
        public static GameObject UIItemSlotPrefab => Instance._uiItemSlotPrefab;
        public static MoveConfig DefaultMoveConfig => Instance._defaultMoveConfig;
        public static GameObject SmallIconIndicatorPrefab => Instance._smallIconIndicatorPrefab;
        public static GameObject SmallTextIndicatorPrefab  => Instance._smallTextIndicatorPrefab;
        public static GameObject DebuggerUIPrefab => Instance._defaultDebuggerUIPrefab;
        public static GameObject RewardFXElement => Instance._rewardFxElementPrefab;
    }
}