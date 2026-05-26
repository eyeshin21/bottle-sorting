using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public class TextMeshProColorController : IColorController
    {
        TMP_Text _tmpText;

        void Construct(TMP_Text tmpText)
        {
            _tmpText = tmpText;
        }

        public GameObject GameObject => _tmpText?.gameObject;

        public Color Color
        {
            get => _tmpText.color;
            set => _tmpText.color = value;
        }

        public void ReturnToPool()
        {
            _tmpText = null;
            Pool.Return(this);
        }

        static Pool<TextMeshProColorController> _pool;
        static Pool<TextMeshProColorController> Pool => _pool ??= new();

        public static TextMeshProColorController Create(TMP_Text tmpText)
        {
            var controller = Pool.Get();
            controller.Construct(tmpText);
            return controller;
        }
    }
}