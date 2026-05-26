#define NaughtyAttributes

#region
using System.Collections.Generic;
using Anvil.Legacy;
#if NaughtyAttributes
using NaughtyAttributes;
#endif

using UnityEngine;
#endregion

namespace Anvil
{
#if DEBUG_MODE
    [ExecuteInEditMode]
#endif
    public class ColorControl : MonoBehaviour,IColorChangable
    {
        [SerializeField] private List<IColorAdapter> _adapters = new List<IColorAdapter>();
        [SerializeField] private GameObject _colorObj;
        [SerializeField] bool searchChild = false;
        [SerializeField] private bool _selfControll = true;
        [SerializeField] private bool _state = true;

        [SerializeField]
#if NaughtyAttributes
         [ShowIf("_selfControll")]
#endif
        private Color _color;
        // [SerializeField, ShowIf("_selfControll")]

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (_colorObj == null)
            {
                _colorObj = gameObject;
            }

            _adapters.Clear();
            var colorAdapter = ColorAdapter.Create(_colorObj,searchChild);
            if (colorAdapter != null)
            {
                _adapters.Add(colorAdapter);
            }
        }

#if DEBUG_MODE
        [Button]
        private void UseThisToPreview()
        {
            _selfControll = true;
            Awake();
            Debug.Log("found " + _adapters.Count + " adapter");
        }
#endif
        Color? _overrideColor = null;

        private void Update()
        {
            if (!_selfControll)
            {
                if (_state)
                {
                    //Update one last time
                    _state = false;
                }
                else
                {
                    if (_overrideColor != null)
                    {
                        SetColorToAllElement(_overrideColor.Value);
                        _overrideColor = null;
                    }

                    return;
                }
            }
            else
            {
                _state = true;
            }

            if (_overrideColor != null)
            {
                SetColorToAllElement(_overrideColor.Value);
                _overrideColor = null;
                return;
            }

            SetColorToAllElement(_color);
        }

        private void SetColorToAllElement(Color color)
        {
            foreach (IColorAdapter colorAdapter in _adapters)
            {
                colorAdapter.Color = color;
            }
        }

        public void ChangeColor(Color color)
        {
            _selfControll = false;
            _overrideColor = color;
            // SetColorToAllElement(color);
        }

        public void ChangeColor(Gradient gradient)
        {
            _selfControll = false;
            SetColorToAllElement(gradient.Evaluate(0));
        }

        public void ChangeColor(Color minColor,Color maxColor)
        {
            _selfControll = false;
            SetColorToAllElement(minColor);
        }
    }
}