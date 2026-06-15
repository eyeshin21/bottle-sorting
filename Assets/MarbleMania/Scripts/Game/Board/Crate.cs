using System;
using System.Collections.Generic;
using Anvil;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania
{
    public enum CrateType
    {
        ThreeByThree,
        TwoByTwo,
    }

    public class Crate : MonoBehaviourGizmos
    {
        [SerializeField] private CrateType _type;
        [SerializeField] private List<Transform> _slots;
        [SerializeField] private float _throwForce;
        [SerializeField] private float _throwAngle;

        public int row;
        public int col;
        
        private CrateGrid _grid;
        public CrateType Type => _type;
        private List<Bottle> _bottles = new List<Bottle>();
        private Vector3 _throwVector;
        
        public int SlotCount => _slots.Count;
        public List<Bottle> Bottles => _bottles;
        private void Awake()
        {
            _throwVector = Vector3.forward;
            // rotate
            _throwVector = Quaternion.Euler(-_throwAngle, 0, 0) * _throwVector;
            _throwVector = _throwVector.normalized * _throwForce;
        }

        public void Init(CrateGrid grid, List<ColorType> data)
        {
            _grid = grid;
            Generate(data);
        }
        public void Generate(List<ColorType> colorData)
        {
            foreach (Bottle bottle in _bottles)
            {
                GameObjectPool.RemoveObject(bottle.gameObject);
            }
            _bottles.Clear();
            for (var i = 0; i < _slots.Count; i++)
            {
                if (i >= colorData.Count) break;
                var slot = _slots[i];
                var prefab = GameConfig.GetBottlePrefab(colorData[i]);
                var bottle = GameObjectPool.CreateObject<Bottle>(slot, prefab.gameObject);
                _bottles.Add(bottle);
            }
        }

        public void OnSelected()
        {
            foreach (Bottle bottle in _bottles)
            {
                bottle.transform.SetParent(null, true);
                bottle.ActivePhysicDynamic(true);
                bottle.Throw(_throwVector);
            }
        }

        [Button]
        public void OnSelect()
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

        public CrateData CreateData()
        {
            List<ColorType> colorData = new List<ColorType>();
            foreach (Bottle bottle in _bottles)
            {
                colorData.Add(bottle.ColorType);
            }
            CrateData data = new  CrateData();
            data.type = _type;
            data.colorData = colorData;
            return data;
        }
    }
}