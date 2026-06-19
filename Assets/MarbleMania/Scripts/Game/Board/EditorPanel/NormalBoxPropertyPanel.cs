using System;
using System.Collections.Generic;
using Anvil;
using MarbleMania.LevelEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Editor = MarbleMania.LevelEditor.Editor;

namespace MarbleMania.EditorPanel
{
    public interface IBoxPropertyPanel
    {
        public void Load(Box box);
    }
    public class NormalBoxPropertyPanel : MonoBehaviour , IBoxPropertyPanel
    {
        [SerializeField] protected Transform _buttonContainer;
        [SerializeField] protected GameObject _slotButtonPrefab;
        [SerializeField] protected LabeledUIButton _convertButton;
        
        private List<FaceButton> _slotButtons = new(); 
        private Box _activeBox;
        public virtual void Load(Box box)
        {
            GameObjectPool.ClearManagedChild(_buttonContainer.gameObject);
            _activeBox = box;
            GridLayoutGroup gridLayoutGroup = _buttonContainer.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.constraintCount = 3;
            for (int i = 0; i< box.SlotCount; i++)
            {
                int index = i;
                FaceButton button = GameObjectPool.CreateObject<FaceButton>(_buttonContainer, _slotButtonPrefab);
                _slotButtons.Add(button);
                button.AddListener(()=>
                {
                    OnSlotClick(button, index);
                });
                Bottle bottle = box.Bottles[i];
                button.SetDisplayButtonColor(bottle.ColorType.ToColor());
            }

            if (_convertButton == null) return;
            _convertButton.ClearListeners();
            if (box is MysteryBox)
            {
                _convertButton.SetLabel("Convert to Normal");
                _convertButton.AddListener(ConvertToNormal);
            }
            else
            {
                _convertButton.SetLabel("Convert to Mystery");
                _convertButton.AddListener(ConvertToMystery);
            }
        }

        private void ConvertToNormal()
        {
            CrateGrid grid = Editor.CrateGrid;
            Box box = GameObjectPool.CreateObject<Box>(null, GameConfig.GetCratePrefab(BoxType.ThreeByThree).gameObject);
            box.SetColor(ColorManager.activeColor);
            grid.RegisterCrate(box, _activeBox.row, _activeBox.col);
            GameObjectPool.RemoveObject(_activeBox.gameObject);
            Load(box);
        }

        private void ConvertToMystery()
        {
            CrateGrid grid = Editor.CrateGrid;
            var prefab = GameConfig.GetCratePrefab(BoxType.Mystery3x3);
            MysteryBox box = GameObjectPool.CreateObject<MysteryBox>(null, prefab.gameObject, resetScale: false);
            box.SetColor(ColorManager.activeColor);
            grid.RegisterCrate(box, _activeBox.row, _activeBox.col);
            EditorGUIUtility.PingObject(box.gameObject);
            GameObjectPool.RemoveObject(_activeBox.gameObject);
            Load(box);
        }

        private void OnSlotClick(FaceButton button, int i)
        {
            if (_activeBox is MysteryBox mBox)
            {
                mBox.SetColor(ColorManager.activeColor);
                Load(mBox);
                return;
            }
            _activeBox.SetBottleColorAt(i, ColorManager.activeColor);
            button.SetDisplayButtonColor(ColorManager.activeColor.ToColor());
            BoxEditor.Instance.RebuildColorIndicator();
        }
    }
}