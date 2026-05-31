using UnityEngine;

[System.Serializable]
public class BottleSlot
{
    public ColorType colorType;
    public Vector3 localPosition;
    public Directions directions;
    public Bottle bottle;

    public bool IsEmpty => bottle == null;

    public virtual Vector3 LocalPosition
    {
        get => localPosition;
        set => localPosition = value;
    }
    public bool HasDirection(Directions direction)
    {
        return (directions.HasFlag(direction));
    }

    public void RegisterBottle(Bottle bottle)
    {
        this.bottle = bottle;
    }

    public void RemoveBottle()
    {
        bottle = null;
    }
}

[System.Serializable]
public class TraySlot : BottleSlot
{
    public Transform transform;
    public override Vector3 LocalPosition 
    {
        get => transform.localPosition;
        set => transform.localPosition = value;
    }
}