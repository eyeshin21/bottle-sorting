using Anvil.Legacy;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private float _slotWidth;
    [SerializeField] private float _slotHeight;

    public static float SlotWidth => Instance._slotWidth;
    public static float SlotHeight => Instance._slotHeight;
}