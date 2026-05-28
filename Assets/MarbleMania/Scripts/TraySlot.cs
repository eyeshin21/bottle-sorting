using UnityEngine;

[System.Serializable]
public struct TraySlot
{
    public Transform transform;
    public ColorType colorType;
    public Vector3 localPosition;
    public Directions directions;
    public Bottle bottle;

    public bool IsEmpty => bottle == null;

    public bool HasDirection(Directions direction)
    {
        return (directions.HasFlag(direction));
    }
    public bool IsNull => transform == null;
    public bool IsNotNull => transform != null;
}