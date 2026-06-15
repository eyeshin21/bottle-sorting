using System;
using Anvil;
using UnityEngine;

namespace MarbleMania
{
    public static class UILoader
    {
        private static MainGameUIAsset _MainGameUIAsset => GameConfig.MainGameUIAsset;
        private static Canvas  _canvas;

        private static Canvas _Canvas
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

        private static ConfirmationPopup _confirmPopup;

        public static void ShowConfirmPopup(Action<ConfirmationPopup> onLoad,Action<bool> callback)
        {
            if (_confirmPopup == null)
            {
                _MainGameUIAsset.LoadGameObject(nameof(MainGameUIType.ConfirmPopup), (prefab) =>
                {
                    _confirmPopup = GameObjectPool.CreateObject<ConfirmationPopup>(_Canvas.transform, prefab);
                    Debug.Log($"{prefab.name}");
                    onLoad?.Invoke(_confirmPopup);
                    _confirmPopup.Show(callback);
                });
                return;
            }
            onLoad?.Invoke(_confirmPopup);
            _confirmPopup.Show(callback);
        }
    }
}