using UnityEngine;

namespace Anvil.Legacy
{
    public interface IItem<TCell>
    {
        TCell Cell { get; set; }
    }
}