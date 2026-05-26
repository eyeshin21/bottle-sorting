using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITestCell<TBoard, TCell>
        where TBoard : ITestBoard<TBoard, TCell>
        where TCell : ITestCell<TBoard, TCell>
    {
        void Construct(TBoard board, int row, int column);
        void Get(out int row, out int column);
        //bool Empty { get; }
        //void Clear();
    }
}