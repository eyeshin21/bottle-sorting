using UnityEngine;

namespace Anvil.Legacy
{
    public interface ISortingController : IController
    {
        int SortingLayerID { get; set; }
        string SortingLayerName { get; set; }
        int SortingOrder { get; set; }
        void AddSortingOrder(int deltaSortingOrder);
    }
}