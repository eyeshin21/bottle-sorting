using System.Collections.Generic;
using Anvil;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania
{
    // public class CrateRow : MonoBehaviourGizmos
    // {
    //     [SerializeField, ReadOnly] private float _spacing;
    //     [SerializeField, ReadOnly] private float _crateSize;
    //     private CrateGrid _parentGrid;
    //     private List<Crate> _crates = new List<Crate>();
    //     
    //     public void Init(CrateGrid parentGrid, rowdata data, float crateSize, float spacing)
    //     {
    //         _parentGrid = parentGrid;
    //         _crateSize = crateSize;
    //         _spacing = spacing;
    //         Generate(data);
    //     }
    //     public void Generate(CrateRowData data)
    //     {
    //         var rowData = data.rowData;
    //         
    //         for (var i = 0; i < rowData.Count; i++)
    //         {
    //             var row = rowData[i];
    //             var crate = GameObjectPool.CreateObject<Crate>(transform,
    //                 GameConfig.GetCratePrefab(CrateType.ThreeByThree)?.gameObject);
    //             crate.Init(this, row.colorData);
    //             _crates.Add(crate);
    //             crate.transform.SetParent(transform);
    //             crate.transform.localPosition = GetCrateLocalPosition(i);
    //         }
    //     }
    //     private Vector3 GetCrateLocalPosition(int index)
    //     {
    //         float z = -index * (_crateSize + _spacing);
    //         return new Vector3(0, 0, z);
    //     }
    //
    //     public void OnCrateSelected(Crate crate)
    //     {
    //         if (crate == null || _crates.IsNullOrEmpty()) return;
    //         if (crate == _crates[0])
    //         {
    //             if (!_parentGrid.CanRemoveCrate(crate)) return;
    //             crate.OnSelected();                
    //             _crates.Remove(crate);
    //             GameObjectPool.RemoveObject(crate.gameObject);
    //             gameObject.DelayCall(0.7f, () =>
    //             {
    //                 UpdateCratePosition();
    //             });
    //         }
    //     }
    //
    //     private void UpdateCratePosition()
    //     {
    //         for (var i = 0; i < _crates.Count; i++)
    //         {
    //             var crate = _crates[i];
    //             crate.transform.localPosition = GetCrateLocalPosition(i);
    //         }
    //     }
    // }
}