using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Extension;
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
    public interface IBoxCollector
    {
        public bool TryCollect(Box box);
    }
    [Serializable]
    public class CrateGridData
    {
        public int row;
        public int col;
        public List<CrateCellData> _gridData;
    }

    [Serializable]
    public class CrateCellData
    {
        public int row;
        public int col;
        public BoxData crate;
    }

    [Serializable]
    public class BoxData
    {
        public BoxType type;
        public Directions direction;
        public List<ColorType> colorData;
        public ColorType colorType;
        public string customData;
    }

    // public class CrateGridCell
    public class CrateGrid : MonoBehaviourGizmos
    {
        [SerializeField] private CrateGridData _testData;

        [SerializeField] private bool drawCoord = false;
        [SerializeField] private float textSize = 0.3f;
        [SerializeField] private int _rowCount;

        [SerializeField] private int _colCount;

        // [SerializeField] private float _rowWidth;
        [SerializeField] private float _spacing;
        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _maxSize = new Vector2(100, 100);

        private List<IBoxCollector> _boxCollectors;
        public int RowCount => _rowCount;
        public int ColCount => _colCount;


        private Box[,] _crates;
        // [SerializeField] private CrateRow _rowPrefab;
        [SerializeField, ReadOnly] private float _width;
        [SerializeField, ReadOnly] private float _height;
        private Action<Box> _onBoxRemove;
        public Box[,] Crates => _crates;
        // [Button]
        
        public void AddOnBoxRemoved(Action<Box> onBoxRemove)
        {
            _onBoxRemove += onBoxRemove;
        }

        public void RemoveOnBoxRemoved(Action<Box> onBoxRemove)
        {
            _onBoxRemove -= onBoxRemove;
        }

        public void RegisterBoxCollector(IBoxCollector collector)
        {
            if (_boxCollectors == null)
            {
                _boxCollectors = new List<IBoxCollector>();
            }
            if (!_boxCollectors.Contains(collector))
            {
                _boxCollectors.Add(collector);
            }
        }
        public void UnRegisterBoxCollector(IBoxCollector collector)
        {
            if (_boxCollectors == null) return;
            for (var i = _boxCollectors.Count - 1; i >= 0; i--)
            {
                var boxCollector = _boxCollectors[i];
                if (boxCollector == collector)
                {
                    _boxCollectors.RemoveAt(i);
                    break;
                }
            }
        }
        private void Awake()
        {
        }

        public void Init(CrateGridData gridData)
        {
            Init(gridData.row, gridData.col);
            for (var i = 0; i < gridData._gridData.Count; i++)
            {
                var data = gridData._gridData[i];
                if (!IsValid(data.row, data.col) || GetCrate(data.row, data.col) != null) continue;
                var prefab = GameConfig.GetCratePrefab(data.crate.type);
                var crate = GameObjectPool.CreateObject<Box>(transform, prefab.gameObject, resetScale: false);
                if (crate == null) continue;
                RegisterCrate(crate, data.row, data.col);
                crate.Init(this, data.crate);
            }
        }

        public void Init(int row, int col)
        {
            _rowCount = row;
            _colCount = col;
            _crates = new Box[_rowCount, _colCount];
            _height = _rowCount * _cellSize + (_rowCount - 1) * _spacing;
            _width = _colCount * _cellSize + (_colCount - 1) * _spacing;
            float scaleX = _maxSize.x / _width;
            float scaleY = _maxSize.y / _height;
            float scale = Mathf.Min(scaleX, scaleY);
            scale = Mathf.Min(scale, 1f);
            transform.localScale = Vector3.one * scale;
        }

        public void RegisterCrate(Box box, int row, int col)
        {
            Debug.Log($"registered {row}-{col}");
            _crates[row, col] = box;
            box.col = col;
            box.row = row;
            if (box.transform.parent != transform)
            {
                box.transform.SetParent(transform, false);
            }

            box.transform.localPosition = GetCellLocalPosition(row, col);
        }

        private bool IsValid(int row, int col)
        {
            if (row >= _rowCount || col >= _colCount || row < 0 || col < 0)
            {
                return false;
            }

            return true;
        }

        public void RemoveCrate(Box box)
        {
            RemoveCrate(box.row, box.col);
        }

        public void RemoveCrate(int row, int col)
        {
            Debug.Log("remove");
            if (!IsValid(row, col) || _crates[row, col] == null) return;
            var box = _crates[row, col];
            _crates[row, col] = null;
            _onBoxRemove?.Invoke(box);
            GameObjectPool.RemoveObject(box.gameObject);
        }

        public Box GetCrate(int row, int col)
        {
            if (!IsValid(row, col)) return null;
            return _crates[row, col];
        }

        private Box FirstCrateOfCol(int col)
        {
            if (col >= _colCount || col < 0) return null;
            for (int i = 0; i < _rowCount; i++)
            {
                var crate = _crates[i, col];
                if (crate != null) return crate;
            }

            return null;
        }

        public Vector3 GetCellLocalPosition(int row, int col)
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
        public int GetCrateColIndex(Box box)
        {
            return box.col;
        }

        public void OnCrateSelected(Box box)
        {
            if (box == null) return;
            if (box != FirstCrateOfCol(box.col)) return;
            if (!CanRemoveCrate(box)) return;
            int col = box.col;
            box.OnSelected();
            RemoveCrate(box);
            GameObjectPool.RemoveObject(box.gameObject);
            var next = FirstCrateOfCol(col);
            next?.OnGridActive();
            // gameObject.DelayCall(0.7f, () => { UpdateCratePosition(); });
        }

        public bool CanRemoveCrate(Box box)
        {
            return true;
        }


        public bool IsPointInside(Vector3 worldPoint)
        {
            Vector3 localPoint = ConvertToLocalSpace(worldPoint);
            return localPoint.x.IsInRange(-_width / 2, _width / 2)
                   && localPoint.z.IsInRange(-_height, 0)
                ;
        }

        private Vector3 ConvertToLocalSpace(Vector3 worldPoint)
        {
            return transform.InverseTransformPoint(worldPoint);
        }

        /// <summary>
        /// Row,Col
        /// </summary>
        public Vector2Int ConvertToGridCoordinates(Vector3 worldPoint)
        {
            Vector3 localPoint = ConvertToLocalSpace(worldPoint);
            int col = Mathf.FloorToInt((localPoint.x + _width / 2) / (_cellSize + _spacing));
            int row = Mathf.FloorToInt((-localPoint.z) / (_cellSize + _spacing));
            return new Vector2Int(row, col);
        }

        public bool IsValidForNewCrate(Vector2Int coord)
        {
            if (!IsValid(coord.x, coord.y)) return false;
            if (GetCrate(coord.x, coord.y) != null) return false;
            return true;
        }

        public override void DrawGizmos()
        {
            // draw grid
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _colCount; j++)
                {
                    Draw.WireRectangleXZ(transform.TransformPoint(GetCellLocalPosition(i, j)),
                        Vector2.one * _cellSize * transform.lossyScale.x, Color.white);
                }
            }

            float x = transform.position.x;
            float z = transform.position.z;
            Vector3 topLeftMax = new Vector3(x - _maxSize.x / 2, 0, z);
            Vector3 bottomRightMax = new Vector3(x + _maxSize.x / 2, 0, z - _maxSize.y);
            // Draw.SolidBox(topLeftMax, Vector3.one * 0.2f, Color.red);
            // Draw.SolidBox(bottomRightMax, Vector3.one * 0.2f, Color.red);
        }

        public CrateGridData CreateData()
        {
            CrateGridData data = new CrateGridData();
            data.row = _rowCount;
            data.col = _colCount;
            data._gridData = new List<CrateCellData>();
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _colCount; j++)
                {
                    var crate = GetCrate(i, j);
                    if (crate == null) continue;
                    CrateCellData cellData = new CrateCellData();
                    cellData.row = i;
                    cellData.col = j;
                    cellData.crate = crate.CreateData();
                    data._gridData.Add(cellData);
                }
            }

            return data;
        }

        public void Clear()
        {
            GameObjectPool.ClearManagedChild(gameObject);
            _crates = new Box[_rowCount, _colCount];
        }

    }
}