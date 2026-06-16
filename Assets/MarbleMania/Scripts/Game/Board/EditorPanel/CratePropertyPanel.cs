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
        private Crate _activeCrate;
        public void Load(Crate crate)
        {
            _activeCrate = crate;
            GridLayoutGroup gridLayoutGroup = _buttonContainer.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.constraintCount = 3;
            for (int i = 0; i< crate.SlotCount; i++)
            {
                int index = i;
                FaceButton button = GameObjectPool.CreateObject<FaceButton>(_buttonContainer, _slotButtonPrefab);
                _slotButtons.Add(button);
                button.AddListener(()=>
                {
                    OnSlotClick(button, index);
                });
                Bottle bottle = crate.Bottles[i];
                button.SetDisplayButtonColor(bottle.ColorType.ToColor());
                Debug.Log("added");
            }
        }

        private void OnSlotClick(FaceButton button, int i)
        {
            _activeCrate.SetBottleColorAt(i, ColorManager.activeColor);
            button.SetDisplayButtonColor(ColorManager.activeColor.ToColor());
        }
    }
}