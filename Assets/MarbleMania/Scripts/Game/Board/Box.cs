using System;
using System.Collections.Generic;
using Anvil;
using Drawing;
using MarbleMania.EditorPanel;
using MarbleMania.LevelEditor;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania
{
    public enum BoxType
    {
        ThreeByThree,
        TwoByTwo,
        Mystery3x3,
        Container,
        Ice,
        Lock,
        Key,
    }

    public class Box : MonoBehaviourGizmos, IEditorProperty
    {
        [SerializeField] private BoxType _type;
        [SerializeField] private List<Transform> _slots;
        [SerializeField] private float _throwForce;
        [SerializeField] private float _throwAngle;

        public int row;
        public int col;
        
        protected CrateGrid _grid;
        public BoxType Type => _type;
        protected List<Bottle> _bottles = new List<Bottle>();
        protected Vector3 _throwVector;
        
        public int SlotCount => _slots.Count;
        public List<Bottle> Bottles => _bottles;

        public CrateGrid Grid
        {
            get => _grid;
            set => _grid = value;
        }
        protected virtual void Awake()
        {
            _throwVector = Vector3.forward;
            // rotate
            _throwVector = Quaternion.Euler(-_throwAngle, 0, 0) * _throwVector;
            _throwVector = _throwVector.normalized * _throwForce;
        }

        public void Init(CrateGrid grid, BoxData data)
        {
            _grid = grid;
            Init(data);
        }
        public virtual void Init(BoxData boxData)
        {
            foreach (Bottle bottle in _bottles)
            {
                GameObjectPool.RemoveObject(bottle.gameObject);
            }
            _bottles.Clear();
            for (var i = 0; i < _slots.Count; i++)
            {
                if (i >= boxData.colorData.Count) break;
                var slot = _slots[i];
                var prefab = GameConfig.BottlePrefab;
                var bottle = GameObjectPool.CreateObject<Bottle>(slot, prefab.gameObject);
                bottle.SetColor(boxData.colorData[i]);
                _bottles.Add(bottle);
            }
        }

        public virtual void OnGridActive()
        {
            
        }
        public virtual void OnSelected()
        {
            foreach (Bottle bottle in _bottles)
            {
                bottle.transform.SetParent(null, true);
                bottle.ActivePhysicDynamic(true);
                bottle.Throw(_throwVector);
            }
        }

        [Button]
        public virtual void OnSelect()
        {
            _grid.OnCrateSelected(this);
        }
        
        [Button]
        private void GenerateSlot()
        {
            RectTransformGridSpawner spawner = GetComponentInChildren<RectTransformGridSpawner>();
            Debug.Log($"child {spawner.transform.childCount}");
            for (int i = spawner.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = spawner.transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
            spawner.Spawn();
            _slots.Clear();
            foreach (Transform child in spawner.transform)
            {
                _slots.Add(child);
            }
        }

        public virtual BoxData CreateData()
        {
            List<ColorType> colorData = new List<ColorType>();
            foreach (Bottle bottle in _bottles)
            {
                colorData.Add(bottle.ColorType);
            }
            BoxData data = new();
            data.type = _type;
            data.colorData = colorData;
            return data;
        }

#region EditorProperty
        [SerializeField] private GameObject _propertyPanelPrefab;

        public void CreatePropertyPanel(Transform parent)
        {
            var panel = GameObjectPool.CreateObject(parent, _propertyPanelPrefab).GetComponent<IBoxPropertyPanel>();
            panel.Load(this);
            // return panel;
        }

#endregion

        public void SetBottleColorAt(int i, ColorType colorType)
        {
            if (i < 0 || i >= _bottles.Count) return;
            var slot = _slots[i];
            var prefab = GameConfig.BottlePrefab;
            var bottle = _bottles[i];
            GameObjectPool.RemoveObject(bottle.gameObject);
            bottle = GameObjectPool.CreateObject<Bottle>(slot, prefab.gameObject);
            bottle.SetColor(colorType);
            _bottles[i] = bottle;
        }

        public virtual void SetColor(ColorType activeColor)
        {
            foreach (Bottle bottle in _bottles)
            {
                GameObjectPool.RemoveObject(bottle.gameObject);
            }
            _bottles.Clear();
            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var prefab = GameConfig.BottlePrefab;
                var bottle = GameObjectPool.CreateObject<Bottle>(slot, prefab.gameObject);
                bottle.SetColor(activeColor);
                _bottles.Add(bottle);
            }
        }
    }
}