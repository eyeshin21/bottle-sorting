using System.Collections.Generic;
using Anvil;
using MarbleMania;
using UnityEngine;

public enum TrayType
{
    ThreeByTwo,
    TwoByThree,
    TwoByTwo,
}
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private float _slotWidth;
    [SerializeField] private float _slotHeight;
    [ElementName(typeof(ColorType))] [SerializeField] private List<Bottle> _bottleByColor;
    [ElementName(typeof(CrateType))] [SerializeField] private List<Crate> _crateByType;
    [ElementName(typeof(TrayType))] [SerializeField] private List<Tray> _trayByType;
    [ElementName(typeof(ColorType))] [SerializeField] private List<Color> _colorByType;
    [SerializeField] private MainGameUIAsset _mainGameUIAsset;
    public static MainGameUIAsset MainGameUIAsset => Instance._mainGameUIAsset;

    public static Color GetColor(ColorType color)
    {
        return Instance._colorByType.TryGet((int)color);
    }
    public static Bottle GetBottlePrefab(ColorType color)
    {
        return Instance._bottleByColor.TryGet((int)color);
    }
    public static Crate GetCratePrefab(CrateType type)
    {
        return Instance._crateByType.TryGet((int)type);
    }
    public static Tray GetTrayPrefab(TrayType positionDataType)
    {
        return Instance._trayByType.TryGet((int)positionDataType);
    }
    
    public static float SlotWidth => Instance._slotWidth;
    public static float SlotHeight => Instance._slotHeight;
    public static List<Tray> TrayPrefabs => Instance._trayByType;
    public static List<Crate> CratePrefabs => Instance._crateByType;
}