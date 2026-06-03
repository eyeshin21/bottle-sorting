using System;
using Anvil.Legacy;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private DiscreteObjectDynamicComponent _dynamicComponent;
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    public ColorType ColorType => _colorType;
    public void MoveTo(Vector3 position)
    {
        transform.SetPositionAndRotation(position, transform.rotation);
    }

    public void ActivePhysicDynamic(bool active)
    {
        _rigidbody.isKinematic = !active;
    }

    public void MoveTo(ITargetDesignator target)
    {
        MoveTo(target.CalculateTargetPosition());
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void Throw(Vector3 vector)
    {
        ActivePhysicDynamic(true);
        _rigidbody.AddForce(vector, ForceMode.Impulse);
    }
}