using Anvil.Legacy;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private DiscreteObjectDynamicComponent _dynamicComponent;
    public void MoveTo(Vector3 position)
    {
        transform.SetPositionAndRotation(position, transform.rotation);
    }
}