using System;
using System.Collections.Generic;
using Anvil;
using MarbleMania;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum TrayType
{
    ThreeByTwo,
    TwoByThree,
    TwoByTwo,
    OneByOne,
    TwoByOne,
    ThreeByOne,
    FourByOne,
    FourByTwo,
}
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private float _slotWidth;
    [SerializeField] private float _slotHeight;
    // [FormerlySerializedAs("_bottleByColor")] 
    // [ElementName(typeof(BottleType))] [SerializeField] private List<Bottle> _bottleByType;
    [SerializeField] private GameObject _bottlePrefab;
    [ElementName(typeof(BoxType))] [SerializeField] private List<Box> _crateByType;
    [ElementName(typeof(TrayType))] [SerializeField] private List<Tray> _trayByType;
    [ElementName(typeof(ColorType))] [SerializeField] private List<Color> _colorByType;
    [SerializeField] private MainGameUIAsset _mainGameUIAsset;
    public static MainGameUIAsset MainGameUIAsset => Instance._mainGameUIAsset;
    public static GameObject BottlePrefab => Instance._bottlePrefab;
    public static Color GetColor(ColorType color)
    {
        return Instance._colorByType.TryGet((int)color);
    }
    // public static Bottle GetBottlePrefab(ColorType color)
    // {
        // return Instance._bottleByType.TryGet((int)color);
    // }
    public static Box GetCratePrefab(BoxType type)
    {
        return Instance._crateByType.TryGet((int)type);
    }
    public static Tray GetTrayPrefab(TrayType positionDataType)
    {
        var ret = Instance._trayByType.TryGet((int)positionDataType);
        return ret;
    }

    [Button]
    private void Validate()
    {
        for (var i = 0; i < _trayByType.Count; i++)
        {
            var item = _trayByType[i];
            if (item == null) continue;
            item.Type = (TrayType)i;
            EditorUtility.SetDirty(item);
        }
        AssetDatabase.SaveAssets();
    }

    public static float SlotWidth => Instance._slotWidth;
    public static float SlotHeight => Instance._slotHeight;
    public static List<Tray> TrayPrefabs => Instance._trayByType;
    public static List<Box> CratePrefabs => Instance._crateByType;
}