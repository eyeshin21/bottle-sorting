using UnityEngine;

namespace Anvil.Legacy
{
    public interface IController
    {
        GameObject GameObject { get; }
        void ReturnToPool();
    }
}