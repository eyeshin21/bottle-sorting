using Anvil.Legacy;
using System.Collections.Generic;
using Anvil;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
    public class GroupedUIElement : MonoBehaviour,IPoolData
    {
        public static OneDimensionGroup CreateHorizontalGroup(Transform parent)
        {
            return CreateGroup<OneDimensionGroup>(parent,AlligmentAxis.Horizontal);
        }

        public static OneDimensionGroup CreateVerticalGroup(Transform parent)
        {
            return CreateGroup<OneDimensionGroup>(parent,AlligmentAxis.Vertical);
        }

        public static T CreateGroup<T>(Transform parent,AlligmentAxis axis) where T : GroupedUIElement
        {
            GameObject groupObj = null;
            if (parent == null)
            {
                parent = PoolHelperAdapter.ParentPopup;
            }

            if (!GameObjectPool.TryGetObject(typeof(T).ToString(),out groupObj))
            {
                groupObj = new GameObject("ElementGroup");
                groupObj.AddComponent<RectTransform>();
            }

            groupObj.transform.SetParent(parent,false);
            groupObj.transform.localScale = Vector3.one;
            T ret = groupObj.GetOrAddComponent<T>();
            ret._poolId = typeof(T).ToString();

            ret.axis = axis;
            return ret;
        }

        // public void OnCreate(GroupAlligmentType type)
        // {
        //
        // }

        [FormerlySerializedAs("_type")] [SerializeField]
        protected AlligmentAxis axis;

        [SerializeField] protected Vector2 _anchorPos;
        protected RectTransform _rectTransform;
        protected Rect _elementRect;

        [FormerlySerializedAs("_elementParents")] [SerializeField]
        protected List<RectTransform> _elements = new List<RectTransform>();

        public List<RectTransform> Elements=>_elements;
        public Rect rect=>_rectTransform == null ? Rect.zero : _rectTransform.rect;
        protected List<GameObject> _inbetwenObjects = new List<GameObject>();
        public List<GameObject> InbetwenObjects=>_inbetwenObjects;
        public Vector2 AnchorPos=>_anchorPos;
        protected string _poolId = "GroupUIElement";
        public string PoolId=>_poolId;
        public bool IsPoolIgnore => false;

        public virtual void Construct(GameObject prefab,int elementCount,bool placeHolderOnly)
        {
            Debug.LogError($"[ElementGroup] Group Not Defined");
        }

        public virtual void ConstructAndRebuildLayout(GameObject prefab,int elementCount,bool placeholderOnly)
        {
            Construct(prefab,elementCount,placeholderOnly);
            RebuildLayout();
        }

        public virtual void RebuildLayout()
        {
        }

        public virtual void WrapContent()
        {
        }

        public List<RectTransform> FindAllParent()
        {
            List<RectTransform> ret = new List<RectTransform>();
            foreach (RectTransform parent in _elements)
            {
                if (parent.childCount == 0)
                {
                    ret.Add(parent);
                    continue;
                }

                Transform childObj = parent.GetChild(0);
                GroupedUIElement childGroup = childObj.gameObject.TryGetComponent<GroupedUIElement>();
                if (childGroup != null)
                {
                    ret.AddRange(childGroup.FindAllParent());
                }
            }

            return ret;
        }

        //Extension
        public virtual Rect CalculateContentDeltaSize()
        {
            throw new System.NotImplementedException();
        }

        public GameObject InsertInbetwen(GameObject insertObj,int elementIndex)
        {
            if (insertObj == null)
            {
                return null;
            }

            if (elementIndex < 0 || elementIndex >= _elements.Count - 1)
            {
                Debug.LogError("groupedUI gameobject insertion failed, element index must be >0 && <lastIndex");
                return null;
            }

            RectTransform elementParent = _elements[elementIndex];
            RectTransform nextElementParent = _elements[elementIndex + 1];
            insertObj.transform.SetParent(_rectTransform,false);
            insertObj.transform.position = (elementParent.position + nextElementParent.position) / 2;
            insertObj.transform.SetAsFirstSibling();
            _inbetwenObjects.Add(insertObj);
            // Debug.Log($"inbetwen inserted at {elementIndex}, position info: {elementParent.gameObject.name}:{elementParent.position}-{nextElementParent.gameObject.name}{nextElementParent.position} =>  {insertObj.transform.position}");
            return insertObj;
        }

        public GameObject InsertInbetwenIcon(int index,Sprite icon = null)
        {
            GameObject insertObj = GameObjectPool.CreateObject(_rectTransform,CommonUIConfig.SmallIconIndicatorPrefab);
            return InsertInbetwen(insertObj,index);
        }

        public GameObject InsertInbetwenText(int index,string text = "+")
        {
            GameObject insertObj = GameObjectPool.CreateObject(_rectTransform,CommonUIConfig.SmallTextIndicatorPrefab);
            return InsertInbetwen(insertObj,index);
        }

        public List<GameObject> ShowInbetwen(GameObject prefab)
        {
            for (int i = 0; i < _elements.Count - 1; i++)
            {
                GameObject insertObj = GameObjectPool.CreateObject(_rectTransform,prefab);
                InsertInbetwen(insertObj,i);
            }

            return _inbetwenObjects;
        }

        public virtual void OnReturnPool()
        {
        }

        // public List<GameObject> ShowInbetwenIcon()
        // {
        //     Debug.Log("showing indicator");
        //     for (int i = 0; i < _elementParents.Count - 1; i++)
        //     {
        //         GameObject insertObj = UIElementManager.CreateObject(_rectTransform, prefab);
        //         InsertInbetwen(insertObj, i);
        //     }
        //
        //     return _inbetwenObjects;
        // }
    }

    public static partial class ExtensionMethod
    {
    }
}
