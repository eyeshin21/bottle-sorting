using System.Collections;
using System.Collections.Generic;
using Anvil;
using MarbleMania.LevelEditor;
using UnityEngine;

namespace MarbleMania
{
    public class ContainerBox : Box
    {
        [SerializeField] private Transform _mesh;
        private List<ColorType> _datas = new();
        [SerializeField]private Directions _direction = Directions.Up;
        private Vector2Int _targetCellCoord;
        public List<ColorType> Datas => _datas;
        protected override void Awake()
        {
            base.Awake();
            _direction = Directions.Up;
        }

        public override void Init(BoxData data)
        {
            if (data.direction == default)
            {
                data.direction = Directions.Up;
            }
            _datas = data.colorData;
            Debug.Log($"data direction {data.direction}");
            SetDirection(data.direction);
            _grid.AddOnBoxRemoved(OnBoxRemove);
            if (!Editor.IsActive)
            {
                CheckSpawn();
            }
        }

        private void OnBoxRemove(Box box)
        {
            if (row == _targetCellCoord.y && col == _targetCellCoord.x)
            {
                StartCoroutine(DelaySpawn());
            }
        }

        private IEnumerator DelaySpawn()
        {
            yield return new WaitForSeconds(0.5f);
            CheckSpawn();
        }
        private void CheckSpawn()
        {
            if (_grid.GetCrate(_targetCellCoord.y, _targetCellCoord.x) != null)
                return;
            if (_datas.Count > 0)
            {
                ColorType colorType = _datas[0];
                _datas.RemoveAt(0);
                var box = GameObjectPool.CreateObject<Box>(transform.parent,
                    GameConfig.GetCratePrefab(BoxType.ThreeByThree).gameObject, resetScale: false);
                box.SetColor(colorType);
                box.Grid = _grid;
                _grid.RegisterCrate(box, _targetCellCoord.y, _targetCellCoord.x);
            }

            if (_datas.Count == 0)
            {
                RemoveFromGrid();
            }
        }

        private void RemoveFromGrid()
        {
            _grid.RemoveCrate(this);
        }

        public void SetDirection(Directions direction)
        {
            _direction = direction;
            int targetRow = row;
            int targetCol = col;
            Debug.Log(direction);
            switch (direction)
            {
                case Directions.Up:
                    _mesh.localRotation = Quaternion.Euler(0, 0, 0);
                    targetRow = row - 1;
                    Debug.Log($"target row {targetRow}");
                    break;
                case Directions.Down:
                    _mesh.localRotation = Quaternion.Euler(0, 180, 0);
                    targetRow = row + 1;
                    break;
                case Directions.Left:
                    _mesh.localRotation = Quaternion.Euler(0, -90, 0);
                    targetCol = col - 1;
                    break;
                case Directions.Right:
                    _mesh.localRotation = Quaternion.Euler(0, 90, 0);
                    targetCol = col + 1;
                    break;
            }

            _targetCellCoord = new Vector2Int(targetCol, targetRow);
            Debug.Log($"targeting {_targetCellCoord.y}, {_targetCellCoord.x}");
        }

        public override BoxData CreateData()
        {
            var data = new  BoxData();
            data.type = Type;
            data.colorData = _datas;
            data.direction = _direction;
            return data;
        }
    }
}