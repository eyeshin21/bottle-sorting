using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        // public static ITargetDesignator DesignateTarget(this GameObject gameObject, GameObject targetObj)
        // {
        //     ITargetDesignator designator = gameObject.GetComponent<ITargetDesignator>();
        //     if (designator == null)
        //     {
        //         designator = gameObject.AddComponent<StaticTargetDesignator>();
        //     }
        //     designator.SetTarget(targetObj);
        //     return designator;
        // }
        // public static ITargetDesignator DesignateTarget(this GameObject gameObject, Vector3 targetPos)
        // {
        //     ITargetDesignator designator = gameObject.GetComponent<ITargetDesignator>();
        //     if (designator == null)
        //     {
        //         designator = gameObject.AddComponent<StaticPositionDesignator>();
        //     }
        //     designator.SetTarget(targetPos);
        //     return designator;
        // }
    }
}
