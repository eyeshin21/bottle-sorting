using UnityEngine;

namespace Anvil.Legacy
{
    public interface IColorController : IController
    {
        Color Color { get; set; }
    }
}