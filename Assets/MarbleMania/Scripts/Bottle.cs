using System;
using System.Collections.Generic;
using Anvil;
using UnityEngine;

public enum BottleType
{
    Normal,
}
public class Bottle : MonoBehaviour
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private DiscreteObjectDynamicComponent _dynamicComponent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private MeshRenderer _renderer;   
    [ElementName(typeof(ColorType))][SerializeField] private List<Material> _materials;
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

    public void SetColor(ColorType activeColor)
    {
        _colorType = activeColor;
        var mats = _renderer.materials;
        mats[1] = _materials.TryGet((int)activeColor);
        _renderer.materials = mats;
    }
}