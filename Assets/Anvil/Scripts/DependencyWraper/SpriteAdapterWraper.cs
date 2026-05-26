using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class SpriteAdapterWraper : MonoBehaviour, ISpriteAdapter
    {
        private ISpriteAdapter _adapter;

        private void Awake()
        {
            _adapter = SpriteAdapter.Create(gameObject);
        }

        public Sprite Sprite { get => _adapter.Sprite; set => _adapter.Sprite = value; }
        public Material Material { get => _adapter.Material; set => _adapter.Material = value; }
        public void SetShow(bool show)
        {
            _adapter.SetShow(show);
        }
    }
}
