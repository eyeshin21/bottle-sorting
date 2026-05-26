using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITestBoard<TBoard, TCell>
        where TBoard : ITestBoard<TBoard, TCell>
        where TCell : ITestCell<TBoard, TCell>
    {
        TCell GetCell(int row, int column);
    }
}