using UnityEngine;

namespace Anvil.Legacy
{
    public interface IFinishHandler
    {
        void AddOnFinish(Listener listener);
    }
}