using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITestItem<TCell>
    {
        TCell Cell { get; set; }
    }
}