using UnityEngine;

namespace Anvil.Legacy
{
    public interface ICoreCell
    {
        void Get(out int row, out int column);
    }
}