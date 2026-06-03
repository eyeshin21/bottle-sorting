using System.Collections.Generic;
using Anvil.Legacy;
using MarbleMania;
using UnityEngine;

enum TrayType
{
    
}
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private float _slotWidth;
    [SerializeField] private float _slotHeight;
    [ElementName(typeof(ColorType))] [SerializeField] private List<Bottle> _bottleByColor;
    [ElementName(typeof(CrateType))] [SerializeField] private List<Crate> _crateByType;

    public static Bottle GetBottlePrefab(ColorType color)
    {
        return Instance._bottleByColor.TryGet((int)color);
    }
    public static Crate GetCratePrefab(CrateType type)
    {
        return Instance._crateByType.TryGet((int)type);
    }
    
    public static float SlotWidth => Instance._slotWidth;
    public static float SlotHeight => Instance._slotHeight;
    
}