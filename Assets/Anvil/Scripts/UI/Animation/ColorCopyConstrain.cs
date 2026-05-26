using System;
using System.Collections;
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class ColorCopyConstrain : MonoBehaviour
    {
        [SerializeField] private GameObject colorSourceObj;
        [SerializeField] private IColorAdapter _parentColorAdapter;
        [SerializeField] private IColorAdapter _childColorAdapter;

        private void Awake()
        {
            if (colorSourceObj == null)
            {
                colorSourceObj = gameObject;
            }

            _parentColorAdapter = ColorAdapter.Create(colorSourceObj, false);
            _childColorAdapter = ColorAdapter.Create(gameObject);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _childColorAdapter.Color = _parentColorAdapter.Color;
        }
    }
}
