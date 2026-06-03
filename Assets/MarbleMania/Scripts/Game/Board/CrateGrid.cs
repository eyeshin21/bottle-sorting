using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania
{
    // public class CrateCell : GridCell
    // {
    //     private Crate _crate;
    //
    //     public Crate Crate => _crate;
    //     public bool IsEmpty => _crate == null;
    //     public void AddCrate(Tray tray)
    //     {
    //         _crate = tray;
    //     }
    //
    //     public void RemoveTray()
    //     {
    //         _crate = null;
    //     }
    // }

    [Serializable]
    public class CrateGridData
    {
        public List<CrateCellData> _gridData;
    }

    [Serializable]
    public class CrateCellData
    {
        public int row;
        public int col;
        public CrateData crate;
    }

    [Serializable]
    public class CrateData
    {
        public CrateType type;
        public List<ColorType> colorData;
    }

    // public class CrateGridCell
    public class CrateGrid : MonoBehaviourGizmos
    {
        [SerializeField] private CrateGridData _testData;

        [SerializeField] private Transform _rowContainer;
        [SerializeField] private bool drawCoord = false;
        [SerializeField] private float textSize = 0.3f;
        [SerializeField] private int _rowCount;

        [SerializeField] private int _colCount;

        // [SerializeField] private float _rowWidth;
        [SerializeField] private float _spacing;
        [SerializeField] private float _cellSize;
        private Crate[,] _crates;

        // [SerializeField] private CrateRow _rowPrefab;

        [SerializeField, ReadOnly]private float _width;
        [SerializeField, ReadOnly]private float _height;

        [Button]
        private void Awake()
        {
            Generate(_testData);
        }

        private void Generate(CrateGridData gridData)
        {
            _crates = new Crate[_rowCount, _colCount];

            GameObjectPool.ClearManagedChild(_rowContainer.gameObject);
            _height = _rowCount * _cellSize + (_rowCount - 1) * _spacing;
            _width = _colCount * _cellSize + (_colCount - 1) * _spacing;

            for (var i = 0; i < gridData._gridData.Count; i++)
            {
                var data = gridData._gridData[i];
                if (!IsValid(data.row, data.col) || GetCrate(data.row, data.col) != null) continue;
                var prefab = GameConfig.GetCratePrefab(data.crate.type);
                var crate = GameObjectPool.CreateObject<Crate>(transform, prefab.gameObject);
                if (crate == null) continue;
                _crates[data.row, data.col] = crate;
                crate.col = data.col;
                crate.row = data.row;
                crate.Init(this, data.crate.colorData);
                crate.transform.localPosition = GetCellLocalPosition(data.row, data.col);
            }
        }

        private bool IsValid(int row, int col)
        {
            if (row >= _rowCount || col >= _colCount || row < 0 || col < 0)
            {
                return false;
            }

            return true;
        }

        private void RemoveCrate(Crate crate)
        {
            _crates[crate.row, crate.col] = null;
        }
        private Crate GetCrate(int row, int col)
        {
            if (!IsValid(row, col)) return null;
            return _crates[row, col];
        }

        private Crate FirstCrateOfCol(int col)
        {
            if (col >= _colCount || col < 0) return null;
            for (int i = 0; i < _rowCount; i++)
            {
                var crate = _crates[i, col];
                if (crate != null) return crate;
            }

            return null;
        }

        private Vector3 GetCellLocalPosition(int row, int col)
        {
            float x = -_width / 2 + _cellSize / 2 + col * (_cellSize + _spacing);
            float z = 0 - _cellSize / 2 - row * (_cellSize + _spacing);
            return new Vector3(x, 0, z);
        }

        private Vector3 GetRowLocalPosition(int i)
        {
            float x = -_width / 2 + _cellSize / 2 + i * (_cellSize + _spacing);
            return new Vector3(x, 0, 0);
        }


        /// <summary>
        /// Unsafe. check for null first
        /// </summary>
        public int GetCrateColIndex(Crate crate)
        {
            return crate.col;
        }

        public void OnCrateSelected(Crate crate)
        {
            if (crate == null) return;
            if (crate != FirstCrateOfCol(crate.col)) return;
            if (!CanRemoveCrate(crate)) return;
            crate.OnSelected();
            RemoveCrate(crate);
            GameObjectPool.RemoveObject(crate.gameObject);
            // gameObject.DelayCall(0.7f, () => { UpdateCratePosition(); });
        }

        public bool CanRemoveCrate(Crate crate)
        {
            return true;
        }
        
        
        public override void DrawGizmos()
        {
            // draw grid
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _colCount; j++)
                {
                    Draw.WireRectangleXZ(transform.TransformPoint(GetCellLocalPosition(i, j)), Vector2.one * _cellSize, Color.white);
                }
            }
        }


    }
}