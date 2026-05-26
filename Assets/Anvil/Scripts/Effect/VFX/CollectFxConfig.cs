
using UnityEngine;

namespace Anvil
{
    public class CollectFxConfig : ScriptableObject
    {
        [SerializeField] private FxParams _param;
        public FxParams Param=>_param;
    }
}
