using UnityEngine;

namespace Anvil.Legacy
{
    public interface IMaterialController : IController
    {
        Material Material { get; set; }
    }
}