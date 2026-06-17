using System;
using System.Collections.Generic;
using Anvil;
using MarbleMania.LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MarbleMania.EditorPanel
{
    public class CratePropertyPanel : MonoBehaviour
    {
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private GameObject _slotButtonPrefab;
        private List<FaceButton> _slotButtons = new(); 
        private Box _activeBox;
        public void Load(Box box)
        {
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
                Debug.Log("added");
            }
        }

        private void OnSlotClick(FaceButton button, int i)
        {
            _activeBox.SetBottleColorAt(i, ColorManager.activeColor);
            button.SetDisplayButtonColor(ColorManager.activeColor.ToColor());
            BoxEditor.Instance.RebuildColorIndicator();
        }
    }
}