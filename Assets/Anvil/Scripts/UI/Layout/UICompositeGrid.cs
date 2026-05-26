using System;
#if NaughtyAttributes
using NaughtyAttributes;
#endif
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Anvil
{
    public enum GridAlligmentMode
    {
        PaddingAlligment,
        SpacingAlligment,
        SelfAlligment,
    }

    public class UICompositeGrid : MonoBehaviour , ILayoutGroup
    {
      
        public void SetLayoutHorizontal()
        {
            UpdateUnmanaged();
            // Debug.Log("horizontal");
        }

        public void SetLayoutVertical()
        {
            UpdateUnmanaged();
            // Debug.Log("vertical ");
        }

        private void UpdateUnmanaged()
        {
            if (!_inited)
            {
                gridItems.Clear();
                Init();
            }

            for (int i = gridItems.Count - 1; i >= 0; i--)
            {
                if (gridItems[i] == null)
                {
                    gridItems.RemoveAt(i);
                }
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject childObj = transform.GetChild(i).gameObject;
                if (childObj == null)
                {
                    continue;
                }

                if (gridItems.Contains(childObj))
                {
                    continue;
                }

                if (_rowObjs.Contains(childObj))
                {
                    continue;
                }

                AddObject(childObj);
            }
        }

        [FormerlySerializedAs("_slotAvailable")]
        public int slotAvailable;

        [FormerlySerializedAs("slotPerRow")] public int _slotPerRow = 5;


        public int _spacing = 5;

        public int _paddingLeft = 5;
        public int _paddingRight = 5;
        public int _paddingTop = 5;
        public int _paddingBottom = 5;

        public TextAnchor rowAligment = TextAnchor.MiddleLeft;


        [SerializeField]private VerticalLayoutGroup _layoutGroup;
        public LayoutGroup LayoutGroup => _layoutGroup;

        // [SerializeField] private int _padding = 5;
        // [SerializeField] private int _slotPerRow = 5;
#if NaughtyAttributes
        [NaughtyAttributes.HorizontalLine]
#endif
        public GridAlligmentMode alligmentMode;
        
        // public bool autoCenter = true;
        // public bool matchPadding = true;
        public bool expandToFitParent = false;

        [SerializeField]private List<GameObject> _rowObjs = new List<GameObject>();
        [FormerlySerializedAs("gridCells")] public List<GameObject> gridItems;
        public int Count => gridItems.GetCount();

        public int RowCount
        {
            get
            {
                int ret = 0;
                // Debug.Log($"child count {transform.childCount} statuc {gameObject.IsActiveInHierarchySafe()}/{gameObject.IsActiveSafe()}");
                foreach (GameObject rowObj in _rowObjs)
                {
                    if (rowObj != null && rowObj.IsActiveSafe())
                    {
                        // Debug.Log("row ++");
                        ret++;
                    }
                    // else
                    // {
                    //     Debug.Log("row --");
                    // }
                }

                return ret;
            }
        }

        private RectTransform _rectTransform;

        public Rect Rect => _rectTransform.rect;
        public float Width => Rect.width;
        public float Height => Rect.height;

        private GameObject _gridItemPrefab = null;
        // private IPoolableGameObject _gridItemPrefabPool;

        private bool _inited = false;
        public bool Inited => _inited;

        private Rect _gridItemRect = new Rect();

        public void Awake()
        {
        }

        private void SetGridItemPrefab(GameObject prefab)
        {
            if (prefab == null)
            {
                _gridItemPrefab = CommonUIConfig.UIItemSlotPrefab;
            }
            else
            {
                _gridItemPrefab = prefab;
            }

            _gridItemRect = _gridItemPrefab.GetComponent<RectTransform>().rect;
        }

        private GameObject CreateGridItem(Transform parent)
        {
            return GameObjectPool.CreateObject(parent, _gridItemPrefab);
        }

        public void ReturnGridItem(GameObject gridItemObj)
        {
            gridItems.Remove(gridItemObj);

            GameObjectPool.RemoveObject(gridItemObj);
        }

        public void Clear()
        {
            for (int i = gridItems.Count - 1; i >= 0; i--)
            {
                GameObject item = gridItems[i];
                if (item == null)
                {
                    continue;
                }

                ReturnGridItem(item);
            }
            gridItems.Clear();

            for (int i = _rowObjs.Count - 1; i >= 0; i--)
            {
                GameObject rowObj = _rowObjs[i];
                if (rowObj == null)
                {
                    _rowObjs.RemoveAt(i);
                    continue;
                }
                rowObj.SetShow(false);
            }

        }


        public void Init(GameObject gridItemPrefab = null, int slotCount = 0)
        {
            slotAvailable = slotCount;
            SetGridItemPrefab(gridItemPrefab);
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            if (expandToFitParent)
            {
                RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
                if (parentRect == null)
                {
                    Debug.LogError("[UIItemSlotGrid] need a parent to operate");
                }
                else
                {
                    float width = parentRect.rect.width;
                    float heigh = parentRect.rect.height;
                }
            }

            _inited = true;

            // _rowObj = new List<GameObject>();

            // ConstructGrid();
        }

        private GameObject _currentRowObj;
        private int _currentIndex;

        private GameObject GetOrAddRow(int rowIndex)
        {
            GameObject rowObj = null;
            if (_rowObjs.Count >= rowIndex + 1)
            {
                rowObj = transform.GetChild(rowIndex).gameObject;
                rowObj.SetShow(true);
                rowObj.transform.localScale = Vector3.one;
            }
            else
            {
                rowObj = new GameObject($"gridRow{rowIndex}");
                RectTransform rowRectTf = rowObj.GetOrAddComponent<RectTransform>();
                rowObj.transform.SetParent(transform);
                rowObj.transform.localScale = Vector3.one;

                HorizontalLayoutGroup rowLayoutGroup = rowObj.GetOrAddComponent<HorizontalLayoutGroup>();
                rowLayoutGroup.childControlHeight = false;
                rowLayoutGroup.childControlWidth = false;
                rowLayoutGroup.childForceExpandHeight = false;
                rowLayoutGroup.childForceExpandWidth = false;

                rowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

                rowLayoutGroup.spacing = _spacing;
                rowLayoutGroup.padding.left = (int)_paddingLeft;

                rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.rect.width);
                rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _gridItemRect.height);

                _rowObjs.CheckAdd(rowObj);
                // Debug.Log($"row added {rowIndex}");

                //SyncSize();
            }

            return rowObj;
        }

        public GameObject AddObject(GameObject obj, bool isManagedItem = true, bool allowOverflow = true)
        {
            int rowIndex = GetRowIndexOfElementIndex(gridItems.Count, out bool isNewRow);
            if (isNewRow && !allowOverflow)
            {
                rowIndex--;
                if (rowIndex < 0)
                {
                    rowIndex = 0;
                }
            }

            GameObject rowObj = GetOrAddRow(rowIndex);
            obj.transform.SetParent(rowObj.transform);
            obj.transform.SetAsLastSibling();
            if (isManagedItem)
            {
                gridItems.Add(obj);
            }

            if (isNewRow)
            {
                SyncSize();
            }else
            {
                RecalculateRowAlligment();
            }

            return obj;
        }

        public void ReIndexElement(GameObject item, int newIndex)
        {
            if (!gridItems.Contains(item) || newIndex < 0 || newIndex >= Count)
            {
                return;
            }

            // int oldIndex = gridItems.IndexOf(item);
            gridItems.Remove(item);
            if (newIndex == Count)
            {
                gridItems.Add(item);
            }
            else
            {
                gridItems.Insert(newIndex, item);
            }

            int elementIndex = 0;
            for (int i = 0; i < Count; i++)
            {
                GameObject element = gridItems[i];
                GameObject rowObj = GetOrAddRow(GetRowIndexOfElementIndex(i, out bool isNewRow));

                element.transform.SetParent(rowObj.transform);
                element.transform.SetSiblingIndex(elementIndex);

                elementIndex++;
                if (elementIndex >= _slotPerRow - 1)
                {
                    elementIndex = 0;
                }
            }
        }

        public int GetRowIndexOfElementIndex(int index, out bool isNewRow)
        {
            int rowIndex = index / _slotPerRow;
            isNewRow = index % _slotPerRow == 0 ? true : false;
            return rowIndex;
        }

        public GameObject AddSlot()
        {
            int nextSlotIndex = gridItems.Count; //0 based
            int rowIndex = GetRowIndexOfElementIndex(gridItems.Count, out bool isNewRow);
            // if (_currentRowObj == null)
            GameObject rowObj = GetOrAddRow(rowIndex);
            // _slotRows.Add(slotRow);
            GameObject slot = CreateGridItem(rowObj.transform);
            gridItems.Add(slot);
            // if (isNewRow)
            // {
            //     HorizontalLayoutGroup rowLayoutGroup = rowObj.GetOrAddComponent<HorizontalLayoutGroup>();
            //     rowLayoutGroup.childControlHeight = false;
            //     rowLayoutGroup.childControlWidth = false;
            //     rowLayoutGroup.childForceExpandHeight = false;
            //     rowLayoutGroup.childForceExpandWidth = false;
            //     rowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            //     rowLayoutGroup.spacing = _spacing;
            //     rowLayoutGroup.padding.left = (int)_paddingLeft;
            //
            //
            // }
            // RectTransform rowRectTf = rowObj.GetOrAddComponent<RectTransform>();
            // rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.rect.width);
            // rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _gridItemRect.height);

            return slot;
        }

        public void SyncSize()
        {
            int rowCount = RowCount;
            float calculatedHeight = rowCount * _gridItemRect.height;
            calculatedHeight += ((rowCount - 1) * _spacing) + _paddingBottom + _paddingTop;
            // Debug.Log($"calculated heigh = {rowCount} x {_gridItemRect.height}");
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);

            RecalculateRowAlligment();
        }

        private void RecalculateRowAlligment()
        {
            if (rowAligment == TextAnchor.MiddleCenter)
            {
                foreach (GameObject rowObj in _rowObjs)
                {
                    HorizontalLayoutGroup layoutGroup = rowObj.GetComponent<HorizontalLayoutGroup>();
                    int elementCount = rowObj.transform.childCount;
                    if (elementCount == _slotPerRow)
                    {
                        layoutGroup.padding.left = _paddingLeft;
                        continue;
                    }

                    int missing = _slotPerRow - elementCount;
                    layoutGroup.padding.left = (int)(_paddingLeft + ((missing * _spacing + missing * _gridItemRect.width) / 2));
                }
            }
        }

        public void ConstructGrid(Callback<GameObject> onSlotCreate = null)
        {
            // Debug.Log("constructing grid");
            Clear();
            float availableW = _rectTransform.rect.width;
            // Rect gridSlotRect = UIConfig.UIItemSlotPrefab.GetComponent<RectTransform>().rect;
            Rect gridSlotRect = _gridItemPrefab.GetComponent<RectTransform>().rect;

            float occupiedW = _slotPerRow * gridSlotRect.width;

            CalculateAlligment(ref occupiedW, ref availableW, out float calculatedSpacing);

            if (availableW <= occupiedW)
            {
                Debug.LogError("[composite grid] grid will overflow with current configuration");
                // return;
            }

            VerticalLayoutGroup verticalLayoutGroup = gameObject.GetOrAddComponentSafe<VerticalLayoutGroup>();
            verticalLayoutGroup.spacing = _spacing;
            verticalLayoutGroup.padding.top = _paddingTop;
            verticalLayoutGroup.padding.bottom = _paddingBottom;
            verticalLayoutGroup.padding.right = 0;
            verticalLayoutGroup.padding.left = 0;
            _layoutGroup = verticalLayoutGroup;


            if (_slotPerRow == 0)
            {
                _slotPerRow = 1;
            }

            int rowCount = slotAvailable / _slotPerRow;
            rowCount += slotAvailable % _slotPerRow == 0 ? 0 : 1;

            if (rowCount < 1)
            {
                return;
            }

            float calculatedHeight = rowCount * _gridItemRect.height;
            calculatedHeight += ((rowCount - 1) * _spacing) + _paddingBottom + _paddingTop;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);

            int slotRemain = slotAvailable;
            gridItems.Clear();
            foreach (Transform childTF in transform)
            {
                childTF.gameObject.SetShow(false);
            }

            for (int i = 0; i < slotRemain; i++)
            {
                GameObject slot = AddSlot();
                onSlotCreate?.Invoke(slot);
            }


            SyncSize();
            // for (int i = 0; i < rowCount; i++)
            // {
            //     GameObject rowObj = null;
            //
            //     if (transform.childCount >= i + 1)
            //     {
            //         rowObj = transform.GetChild(i).gameObject;
            //     }
            //     else
            //     {
            //         rowObj =  new GameObject($"gridRow{i}");
            //     }
            //
            //     rowObj.SetShow(true);
            //     rowObj.transform.SetParent(transform);
            //     rowObj.transform.localScale = Vector3.one;
            //
            //     HorizontalLayoutGroup rowLayoutGroup = rowObj.GetOrAddComponent<HorizontalLayoutGroup>();
            //     rowLayoutGroup.childControlHeight = false;
            //     rowLayoutGroup.childControlWidth = false;
            //     rowLayoutGroup.spacing = _spacing;
            //     rowLayoutGroup.childForceExpandHeight = false;
            //     rowLayoutGroup.childForceExpandWidth = false;
            //     rowLayoutGroup.padding.left = (int)_paddingLeft;
            //     rowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            //     // _slotRows.Add(slotRow);
            //     for (int j = 0; j < _slotPerRow; j++)
            //     {
            //         // GameObject slot = Instantiate(_gridItemPrefab,rowObj.transform);
            //         GameObject slot = CreateGridItem(rowObj.transform);
            //         gridItems.Add(slot);
            //         // slotRow.AddSlot(slot);
            //         slotRemain--;
            //         if (slotRemain <= 0)
            //         {
            //             break;
            //         }
            //     }
            //
            //     RectTransform rowRectTf = rowObj.GetComponent<RectTransform>();
            //     rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, availableW);
            //     rowRectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _gridItemRect.height);
            // }
        }

        public GameObject GetGridItem(int atIndex, bool autoClamp = false)
        {
            if (atIndex >= gridItems.Count || atIndex < 0)
            {
                if (autoClamp)
                {
                    atIndex = Math.Clamp(atIndex, 0, gridItems.Count - 1);
                    Debug.LogWarning($"grid item at index {atIndex} is out of range clamping to {atIndex}");
                    return gridItems[atIndex];
                }

                Debug.LogError($"grid item at index {atIndex} is out of range");
                return null;
            }

            return gridItems.TryGet(atIndex);
        }

        private void CalculateAlligment(ref float occupiedW, ref float availableW, out float calculatedSpacing)
        {
            calculatedSpacing = 0;
            if (alligmentMode == GridAlligmentMode.SpacingAlligment)
            {
                occupiedW += _spacing * (_slotPerRow - 1);
                float remainingSpace = availableW - occupiedW;
                float extendedPadding = remainingSpace / 2;
                _paddingLeft += (int)extendedPadding;
            }
            else if (alligmentMode == GridAlligmentMode.PaddingAlligment)
            {
                float remainingSpace = availableW - occupiedW;
                remainingSpace -= (_paddingLeft + _paddingRight);
                calculatedSpacing = remainingSpace / (_slotPerRow - 1);
                calculatedSpacing = Mathf.Clamp(calculatedSpacing, 0, remainingSpace);
                _spacing = (int)calculatedSpacing;
            }
            else if (alligmentMode == GridAlligmentMode.SelfAlligment)
            {
                // calculatedSpacing = _spacing;
            }
        }

        /// <summary>
        /// Recalculate spacing for each row so the excess space is distributed evenly
        /// </summary>
        public void RecalculateRowSpacing()
        {
            //if (!expandToFitParent)
            //{
            //    return;
            //}
            Transform row = _rectTransform.GetChild(0);
            RectTransform gridItemRTF = _gridItemPrefab.GetComponent<RectTransform>();
            float occupiedSpace = 0;
            RectTransform rectTF = row.GetComponent<RectTransform>();
            foreach (RectTransform childRTF in rectTF)
            {
                occupiedSpace += childRTF.rect.width;
            }

            if (slotAvailable < _slotPerRow)
            {
                int slotMissing = _slotPerRow - slotAvailable;
                occupiedSpace += slotMissing * gridItemRTF.rect.width;
            }

            float remainingSpace = _rectTransform.rect.width - occupiedSpace;

            HorizontalLayoutGroup layoutGroup = row.gameObject.TryGetComponent<HorizontalLayoutGroup>();
            remainingSpace = remainingSpace - layoutGroup.padding.left - layoutGroup.padding.right;
            float calculatedSpace = 0;
            if (remainingSpace <= 0)
            {
                calculatedSpace = 0;
            }
            else
            {
                calculatedSpace = remainingSpace / (row.childCount - 1);
            }

            foreach (Transform child in transform)
            {
                HorizontalLayoutGroup childlayoutGroup = child.gameObject.TryGetComponent<HorizontalLayoutGroup>();
                if (childlayoutGroup != null)
                {
                    childlayoutGroup.spacing = calculatedSpace;
                }
            }
        }
        //
        // public void SetLayoutHorizontal()
        // {
        //     CheckRegisterUnmanaged();
        // }
        //
        // public void SetLayoutVertical()
        // {
        //     CheckRegisterUnmanaged();
        // }
        //
        // private void CheckRegisterUnmanaged()
        // {
        //     //TODO
        //
        //     // if (!_inited)
        //     // {
        //     //     Init();
        //     //     Debug.Log("inted");
        //     // }
        //     //
        //     // foreach (Transform child in transform)
        //     // {
        //     //     if (_rowObjs.Contains(child.gameObject))
        //     //     {
        //     //         continue;
        //     //     }
        //     //     Debug.Log("adding");
        //     //     AddObject(child.gameObject);
        //     // }
        // }
    }
}
