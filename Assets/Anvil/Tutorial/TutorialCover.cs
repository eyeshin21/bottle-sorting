using System;
using System.Collections.Generic;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public enum MaskType
    {
        Rect,
    }

    public class TutorialCover : MonoBehaviour
    {
        // [SerializeField] private ReferenceIDPoolTable _maskTable;
        [ElementName(typeof(MaskType))] [SerializeField]
        private List<GameObject> _maskPrefabs;

        private TutorialMask _mask;

        public TutorialMask ShowMask(MaskType type, Vector3 position, Vector2 worldUnitSize, Action callback = null)
        {
            if (_mask == null)
            {
                if (_maskPrefabs == null)
                {
                    Debug.LogError("Mask table is not assigned.");
                    return null;
                }

                GameObject prefab = _maskPrefabs.TryGet((int)type);
                if (prefab == null)
                {
                    Debug.LogError($"mask type not found {type}");
                    return null;
                }

                GameObject maskObj = GameObjectPool.CreateObject(transform, prefab);
                _mask = maskObj.GetComponent<TutorialMask>();
                maskObj.transform.SetAsFirstSibling();
            }

            _mask.ShowMask(position, worldUnitSize.x, worldUnitSize.y, callback);
            return _mask;
        }

        public void HideMask(Action callback = null)
        {
            if (_mask == null)
            {
                callback?.Invoke();
                return;
            }

            _mask.HideMask(callback);
        }

        public void HideMaskThenRemove(Action callback = null)
        {
            HideMask(() =>
            {
                GameObjectPool.RemoveObject(_mask.gameObject);
                callback?.Invoke();
            });
        }
    }
}