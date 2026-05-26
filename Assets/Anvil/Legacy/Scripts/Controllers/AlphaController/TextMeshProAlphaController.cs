using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public class TextMeshProAlphaController : IAlphaController
    {
        TMP_Text _tmpText;

        void Construct(TMP_Text tmpText)
        {
            _tmpText = tmpText;
        }

        public GameObject GameObject => _tmpText?.gameObject;

        public float Alpha
        {
            get => _tmpText.color.a;
            set
            {
                var color = _tmpText.color;
                color.a = value;
                _tmpText.color = color;
            }
        }

        public void ReturnToPool()
        {
            _tmpText = null;
            Pool.Return(this);
        }

        static Pool<TextMeshProAlphaController> _pool;
        static Pool<TextMeshProAlphaController> Pool => _pool ??= new();

        public static TextMeshProAlphaController Create(TMP_Text tmpText)
        {
            var controller = Pool.Get();
            controller.Construct(tmpText);
            return controller;
        }
    }
}