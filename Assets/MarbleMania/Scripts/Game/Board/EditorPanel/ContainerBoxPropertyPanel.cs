using System;
using System.Collections.Generic;
using Anvil;
using MarbleMania.LevelEditor;
using TMPro;
using UnityEngine;

namespace MarbleMania.EditorPanel
{
    public class ContainerBoxPropertyPanel : MonoBehaviour, IBoxPropertyPanel
    {
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private Transform _buttonParent;

        [SerializeField] private TMP_InputField input;
        [SerializeField] private UIButton AddButton;
        [SerializeField] private UIButton SetButton;

        [SerializeField] private UIButton DpadUp;
        [SerializeField] private UIButton DpadDown;
        [SerializeField] private UIButton DpadLeft;
        [SerializeField] private UIButton DpadRight;

        private ContainerBox _activeBox;

        private void Awake()
        {
            DpadUp.AddListener(() => SetDirection(Directions.Up));
            DpadDown.AddListener(() => SetDirection(Directions.Down));
            DpadLeft.AddListener(() => SetDirection(Directions.Left));
            DpadRight.AddListener(() => SetDirection(Directions.Right));
            AddButton.AddListener(OnAdd);
            SetButton.AddListener(OnSet);
        }

        private void OnSet()
        {
            int input = int.Parse(this.input.text);
            List<ColorType> colors = _activeBox.Datas;
            int delta = input - colors.Count;
            if (delta > 0)
            {
                for (int i = 0; i < delta; i++)
                {
                    colors.Add(ColorManager.activeColor);
                }
            }
            else if (delta < 0)
            {
                colors.RemoveRange(colors.Count + delta, -delta);
            }
            Load(_activeBox);
        }

        private void OnAdd()
        {
            var color = ColorManager.activeColor;
            int input = int.Parse(this.input.text);
            int currentIndex  = _activeBox.Datas.Count - 1;
            for (int i = 0; i < input; i++)
            {
                currentIndex++;
                int index = currentIndex;
                _activeBox.Datas.Add(color);
                var button  = GameObjectPool.CreateObject<LabledFaceButton>(_buttonParent, _buttonPrefab);
                button.SetLabel(currentIndex.ToString());
                button.SetDisplayButtonColor(color.ToColor());
                button.AddListener(() => { SetColor(button, index); });
            }
        }

        private void SetDirection(Directions dir)
        {
            if (_activeBox == null) return;
            _activeBox.SetDirection(dir);
        }

        public void Load(Box box)
        {
            if (box is not ContainerBox cBox) return;
            _activeBox = cBox;
            GameObjectPool.ClearManagedChild(_buttonParent.gameObject);
            for (var i = 0; i < _activeBox.Datas.Count; i++)
            {
                int index = i;
                var colorType = _activeBox.Datas[i];
                var button = GameObjectPool.CreateObject<LabledFaceButton>(_buttonParent, _buttonPrefab);
                button.SetLabel(i.ToString());
                button.SetDisplayButtonColor(colorType.ToColor());
                button.AddListener(() => { SetColor(button, index); });
            }
        }

        private void SetColor(LabledFaceButton button, int index)
        {
            Debug.Log("click");
            ColorType color = ColorManager.activeColor;
            Debug.Log($"color {color}");
            button.SetDisplayButtonColor(color.ToColor());
            _activeBox.Datas[index] = color;
        }
    }
}