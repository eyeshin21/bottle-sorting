using UnityEngine;

namespace Anvil.Legacy
{
    public interface IValue<T>
    {
        T Value { get; set; }
    }
}