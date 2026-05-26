using UnityEngine;

namespace Anvil.Legacy
{
    public interface ICell<TBoard, TCell> : ICoreCell
        where TBoard : IBoard<TBoard, TCell>
        where TCell : ICell<TBoard, TCell>
    {
        bool Empty { get; }
        void Construct(TBoard board, int row, int column);
        void Clear();
    }
}