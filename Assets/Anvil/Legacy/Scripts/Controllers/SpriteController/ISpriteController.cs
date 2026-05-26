using UnityEngine;

namespace Anvil.Legacy
{
    public interface ISpriteController : IController
    {
        Sprite Sprite { get; set; }
        bool FlipX { get; set; }
    }
}