using UnityEngine;

namespace Anvil
{
    [ExecuteAlways]
    public class ForceChildInheritLayer : MonoBehaviour
    {
        void OnTransformChildrenChanged()
        {
            ApplyLayer();
        }

        void OnValidate()
        {
            ApplyLayer();
        }

        void ApplyLayer()
        {
            int layer = gameObject.layer;

            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = layer;
            }
        }
    }
}