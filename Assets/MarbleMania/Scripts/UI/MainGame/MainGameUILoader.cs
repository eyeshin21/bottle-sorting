using System;
using Anvil;
using Anvil.Legacy;
using UnityEngine;

namespace MarbleMania
{
    public enum MainGameUIType
    {
        ConfirmPopup,
    }
    public class MainGameUILoader : LocalAssetTable<MainGameUIType>
    {
        private Canvas  _canvas;

        private Canvas _Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = CanvasUIUtility.CreateUICanvas("MainGameCanvas", 10);
                }
                return _canvas;
            }
        }
        public ConfirmationPopup _confirmPopup;

        public void ShowConfirmPopup(Action<ConfirmationPopup> onLoad,Action callback)
        {
            if (_confirmPopup == null)
            {
                LoadAsset<GameObject>(nameof(MainGameUIType.ConfirmPopup), (prefab) =>
                {
                    _confirmPopup = GameObjectPool.CreateObject<ConfirmationPopup>(_Canvas.transform, prefab);
                    onLoad?.Invoke(_confirmPopup);
                    _confirmPopup.Show(callback);
                });
                return;
            }
            _confirmPopup.Show(callback);
        }
    }

    
}