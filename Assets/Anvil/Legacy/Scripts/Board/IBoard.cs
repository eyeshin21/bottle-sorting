using UnityEngine;

namespace Anvil.Legacy
{
    public interface IBoard<TBoard, TCell>
        where TBoard : IBoard<TBoard, TCell>
        where TCell : ICell<TBoard, TCell>
    {
        TCell GetCell(int row, int column);
        Vector3 GetCellPosition(int row, int column);
    }
}