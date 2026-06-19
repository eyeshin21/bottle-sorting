using System;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace MarbleMania
{
    public class IceBox : Box
    {
        [SerializeField] private GameObject _iceObject;
        [SerializeField] private TMP_Text _hpText;

        private int _hp;
        public int Hp
        {
            get => _hp;
            set
            {
                _hp  = value;
                Debug.Log($"hpset {value}");
                _hpText.text = Hp.ToString();
            }
        }
        public override void Init(BoxData boxData)
        {
            base.Init(boxData);
            _grid?.AddOnBoxRemoved(OnBoxRemove);
            Hp = int.Parse(boxData.customData);
            _hpText.text = Hp.ToString();
            CheckUnlock();
        }

        private void OnDestroy()
        {
            _grid.RemoveOnBoxRemoved(OnBoxRemove);
        }

        private void OnBoxRemove(Box box)
        {
            OnDamaged(1);
        }

        private void OnDamaged(int i)
        {
            Hp -= i;
            CheckUnlock();
        }

        private void CheckUnlock()
        {
            if (_hp <= 0)
            {
                _iceObject.SetActive(false);
            }
        }

        public override void OnSelect()
        {
            if (_hp > 0)
            {
                return;
            }
            base.OnSelect();
        }

        public override BoxData CreateData()
        {
            var data = base.CreateData();
            data.customData = Hp.ToString();
            return data;
        }
    }
}