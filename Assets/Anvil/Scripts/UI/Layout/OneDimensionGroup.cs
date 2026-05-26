using System;
using System.Linq;
using Anvil.Legacy;

#if NaughtyAttributes
using NaughtyAttributes;
#endif

using UnityEngine;

namespace Anvil
{
    public class OneDimensionGroup : GroupedUIElement
    {
        //all calculation are assuming root position of a rect is its bottoml left pos

        public float paddingTop;
        public float paddingBottom;
        public float paddingLeft;
        public float paddingRight;
        public float spacing;

        public bool _wrapContent = true;


        private float _anchorIndex = -1;
        private Vector2 _centerPos;

        GameObject _baseElementPrefab;

        private bool _isPlaceHolderOnly = true;
        // private bool _inited = false;
        // public bool _childAutoExpand;

        public override void Construct(GameObject prefab, int elementCount, bool placeHolderOnly)
        {
            // _inited = true;
            _baseElementPrefab = prefab;
            _isPlaceHolderOnly = placeHolderOnly;
            RectTransform rectTF = prefab.GetComponent<RectTransform>();
            _elementRect = rectTF.rect;

            _rectTransform = gameObject.GetOrAddComponentSafe<RectTransform>();

            for (int i = 0; i < elementCount; i++)
            {
                if (placeHolderOnly)
                {
                    AddElementContainer();
                }
                else
                {
                    AddElement();
                }
            }

            //UpdateSize();
            // RebuildLayout();
        }

        private void Awake()
        {
            _rectTransform = gameObject.GetOrAddComponentSafe<RectTransform>();
            if (_rectTransform.childCount == 0)
            {
                return;
            }

            foreach (RectTransform childRTF in _rectTransform)
            {
                _elements.Add(childRTF);
            }

            UpdateLayout();
        }

        public void UpdateLayout()
        {
            RebuildLayout();
            if (_wrapContent)
            {
                WrapContent();
            }
        }

        public void SetPadding(PaddingInfo padding)
        {
            paddingTop = padding._top;
            paddingBottom = padding._bottom;
            paddingLeft = padding._left;
            paddingRight = padding._right;
            spacing = padding._spacing;
        }

        public GameObject AddElement(Action<GameObject> action = null)
        {
            GameObject elementObj = GameObjectPool.CreateObject(_rectTransform, _baseElementPrefab);
            RectTransform elementRTF = elementObj.GetOrAddComponentSafe<RectTransform>();
            elementRTF.SetParent(_rectTransform, false);
            elementRTF.localScale = Vector2.one;
            _elements.Add(elementRTF);

            action?.Invoke(elementObj);

            return elementObj;
        }

        public GameObject AddElementContainer()
        {
            GameObject elementObj = new GameObject($"elementContainer{_elements.Count}");
            RectTransform elementRTF = elementObj.GetOrAddComponentSafe<RectTransform>();
            elementRTF.SetParent(_rectTransform, false);
            elementRTF.localScale = Vector2.one;
            elementRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _elementRect.width);
            elementRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _elementRect.height);
            _elements.Add(elementRTF);
            return elementObj;
        }

        public GameObject AddElementContainer(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddElementContainer();
            }

            return Elements.Last().gameObject;
        }

        public override void RebuildLayout()
        {
            // Debug.Log($"Rebuilding layout");
            if (_wrapContent)
            {
                UpdateSize();
            }
            else
            {
                UpdateAnchorPos();
            }

            float cellSize = 0;
            if (axis == AlligmentAxis.Horizontal)
                cellSize = _elementRect.width + spacing;
            else
                cellSize = _elementRect.height + spacing;
            Vector2 offsetVector = _anchorPos - _centerPos;
            //Debug.Log($"raing layout with anchor index {_anchorIndex}");
            for (int i = 0; i < _elements.Count; i++)
            {
                RectTransform parent = _elements[i];
                float offsetIndex = i - _anchorIndex;
                float offset = offsetIndex * cellSize;
                if (axis == AlligmentAxis.Vertical)
                {
                    Vector2 newPos = new Vector2(0, offset);
                    parent.anchoredPosition = newPos + offsetVector;
                }
                else if (axis == AlligmentAxis.Horizontal)
                {
                    Vector2 newPos = new Vector2(offset, 0);
                    parent.anchoredPosition = newPos + offsetVector;
                }
            }
        }

        public override void WrapContent()
        {
            if (!_wrapContent)
            {
                return;
            }

            Rect newSize = CalculateContentDeltaSize(out float extendTop, out float extendBottom, out float extendLeft,
                out float extendRight);
            Rect oldSize = rect;

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.width);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.height);

            Vector2 topleft = new Vector2(0 - extendLeft, 0 - extendTop);
            Vector2 bottomRight = new Vector2(oldSize.width + extendRight, oldSize.height + extendBottom);
            Vector2 newCenter = (topleft + bottomRight) / 2;
            Vector2 oldCenter = new Vector2(oldSize.width / 2, oldSize.height / 2);

            Vector2 offsetVector = newCenter - oldCenter;
            float cellSize = 0;
            if (axis == AlligmentAxis.Horizontal)
                cellSize = _elementRect.width;
            else
                cellSize = _elementRect.height;
            foreach (RectTransform rectTransform in _elements)
            {
                Vector2 newPos = rectTransform.anchoredPosition + offsetVector;
                // if (axis == AlligmentAxis.Horizontal)
                // {
                //     newPos.x += (cellSize / 2);
                // }
                // else
                // {
                //     newPos.y += (cellSize / 2);
                // }
                rectTransform.anchoredPosition = newPos;
            }
        }

        public override Rect CalculateContentDeltaSize()
        {
            return CalculateContentDeltaSize(out float throwaway1, out float throwaway2, out float throwaway3,
                out float throwaway4);
        }

        public Rect CalculateContentDeltaSize(out float extendTop, out float extendBottom, out float extendLeft,
            out float extendRight)
        {
            extendBottom = 0;
            extendTop = 0;
            extendLeft = 0;
            extendRight = 0;

            int elementIndex = 0;
            bool dirty = false;
            foreach (RectTransform elementParentRTF in _elements)
            {
                elementIndex++;
                if (elementParentRTF.childCount == 0)
                {
                    continue;
                }

                if (!elementParentRTF.GetChild(0).TryGetComponent<GroupedUIElement>(out GroupedUIElement childGroup))
                {
                    //TODO: Calculation for iregular element size
                    continue;
                }
                // RectTransform parentRTF = parent.parent as RectTransform;

                Rect childRect = childGroup.CalculateContentDeltaSize();
                float bottomSpace = childGroup.rect.height - childGroup.AnchorPos.y;
                float topSpace = childGroup.AnchorPos.y;
                float rightSpace = childGroup.rect.width - childGroup.AnchorPos.x;
                float leftSpace = childGroup.AnchorPos.x;

                Vector2 elementParentPos = elementParentRTF.anchoredPosition;
                topSpace = topSpace - (rect.height - (-elementParentPos.y + rect.height / 2));
                bottomSpace = bottomSpace - (rect.height - (elementParentPos.y + rect.height / 2));
                leftSpace = leftSpace - (rect.width - (elementParentPos.x + rect.width / 2));
                rightSpace = rightSpace - (rect.width - (-elementParentPos.x + rect.width / 2));

                extendBottom = bottomSpace > extendBottom ? bottomSpace : extendBottom;
                extendTop = topSpace > extendTop ? topSpace : extendTop;
                extendLeft = leftSpace > extendLeft ? leftSpace : extendLeft;
                extendRight = rightSpace > extendRight ? rightSpace : extendRight;


                dirty = true;
            }

            if (!dirty)
            {
                return rect;
            }

            Debug.Log($"wrapContetn Detail: extension: l{extendLeft} r{extendRight} u{extendTop} d{extendBottom}");
            float finalW = rect.width + extendLeft + extendRight;
            float finalH = rect.height + extendTop + extendBottom;

            return new Rect(rect.position, new Vector2(finalW, finalH));
        }

        public void UpdateSize()
        {
            // Debug.Log($"Updating container size");
            float sizeX = 0;
            float sizeY = 0;
            sizeX += paddingRight + paddingLeft;
            sizeY += paddingTop + paddingBottom;
            int count = _elements.Count;
            if (axis == AlligmentAxis.Vertical)
            {
                sizeY += spacing * (count - 1);
                //sizeX = elementRect.width;
                float maxW = 0;
                foreach (RectTransform rtf in Elements)
                {
                    sizeY += rtf.rect.height;
                    maxW = Mathf.Max(maxW, rtf.rect.width);
                }

                sizeX = maxW;
            }
            else
            {
                sizeX += spacing * (count - 1);
                //sizeX += elementRect.width * count;
                //sizeY = elementRect.height;
                float maxH = 0;
                foreach (RectTransform rtf in Elements)
                {
                    sizeX += rtf.rect.width;
                    maxH = Mathf.Max(maxH, rtf.rect.height);
                }

                sizeY = maxH;
            }

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY);
            // Debug.Log($"size updated {sizeX}:{sizeY}");


            UpdateAnchorPos();
        }

        public void SetAnchorIndex(float index)
        {
            _anchorIndex = index;
        }

        public void UpdateAnchorPos()
        {
            if (_anchorIndex == -1)
            {
                _anchorIndex = _elements.Count == 1 ? 0 : (_elements.Count - 1) / 2f;
                // Debug.Log($"anchor set default");
            }

            // Debug.Log($"updating anchor index {_anchorIndex}");
            _anchorIndex = Mathf.Clamp(_anchorIndex, 0, _elements.Count - 1);

            float x = 0;
            float y = 0;
            Rect rect = _rectTransform.rect;
            float yPos = 0;
            float xPos = 0;

            int floorIndex = Mathf.FloorToInt(_anchorIndex);
            int ceilIndex = Mathf.CeilToInt(_anchorIndex);
            // Debug.Log($"floor {floorIndex} celi {ceilIndex}");
            if (axis == AlligmentAxis.Vertical)
            {
                xPos = rect.width / 2;
                yPos += paddingTop;
                Rect preRect = new Rect();
                for (int i = 0; i < floorIndex; i++)
                {
                    Rect elementRect = _elements[i].rect;
                    xPos += preRect.width / 2;
                    yPos += elementRect.height / 2;
                    preRect = elementRect;
                }

                yPos += spacing * floorIndex;
                float fraction = _anchorIndex - floorIndex;
                if (fraction > 0)
                {
                    Rect floorRect = _elements[floorIndex].rect;
                    Rect ceilRect = _elements[ceilIndex].rect;
                    yPos += ((floorRect.height + ceilRect.height) / 2) + spacing * fraction;
                }

                y = 1 - (yPos / _rectTransform.rect.height);
                x = 0.5f;
            }
            else if (axis == AlligmentAxis.Horizontal)
            {
                yPos = rect.height / 2;

                //xPos += paddingLeft + _elementRect.width / 2 + (_elementRect.width + spacing) * _anchorIndex;
                xPos += paddingLeft;
                Rect preRect = new Rect();
                for (int i = 0; i <= _anchorIndex; i++)
                {
                    Rect elementRect = _elements[i].rect;
                    if (i == 0)
                    {
                        xPos += elementRect.width / 2;
                        preRect = elementRect;
                    }
                    else
                    {
                        xPos += preRect.width / 2;
                        xPos += elementRect.width / 2;
                        preRect = elementRect;
                    }
                }

                xPos += spacing * floorIndex;
                float fraction = _anchorIndex - floorIndex;
                // Debug.Log($"fraction {fraction}");
                if (fraction > 0)
                {
                    Rect floorRect = _elements[floorIndex].rect;
                    Rect ceilRect = _elements[ceilIndex].rect;
                    xPos += (((floorRect.width + ceilRect.width) / 2) + spacing) * fraction;
                }

                x = xPos / _rectTransform.rect.width;
                y = 0.5f;
            }

            // Debug.Log($"xpos{xPos}");
            _centerPos = new Vector2(rect.width / 2, rect.height / 2);
            _anchorPos = new Vector2(xPos, yPos);
            // Debug.Log($"calculated pivot {x}:{y}, anchorpos{_anchorPos}, centerpos{_centerPos}");
            Vector2 offsetVector = _anchorPos - _centerPos;
            _rectTransform.anchoredPosition += offsetVector;
            _rectTransform.pivot = new Vector2(x, y);
        }

        private void CleanSelf()
        {
            GameObjectPool.ClearChild(gameObject);

            //UIElementManager.ClearChild(gameObject);
            Elements.Clear();
            _inbetwenObjects.Clear();
        }

        public override void OnReturnPool()
        {
            if (_isPlaceHolderOnly)
            {
                foreach (RectTransform parent in _elements)
                {
                    if (parent.childCount == 0)
                    {
                        continue;
                    }

                    GameObject childObj = parent.GetChild(0).gameObject;
                    GameObjectPool.RemoveObject(childObj);
                }
            }

            CleanSelf();
        }
    }
}
