// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Anvil;
// using NaughtyAttributes;
// using UnityEngine;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
// using GizmosHelper = Anvil.GizmosHelper;
// using MoveConfig = MatchThree.MoveConfig;
//
// namespace Anvil
// {
//     public class MoveControllerLayoutWraper : MonoBehaviour
//     {
//         LayoutGroup _layoutGroup;
//         [SerializeField] List<LayoutFollower> _elements = new List<LayoutFollower>();
//         [SerializeField] private GameObject _elementContainer;
//         [FormerlySerializedAs("_moveConfig")] [SerializeField] private MoveConfig _defaultMoveConfig;
//         [SerializeField] private bool _initOnAwake = true;
//
//         [SerializeField]
//         private bool _findElementOnAwake = true;
//
//         private List<int> _emptyIndex = new List<int>();
//         private bool _inited = false;
//         private bool _canUpdate = true;
//         private bool _busy = false;
//         public bool Busy=>_busy;
//         public List<int> EmptyIndex=>_emptyIndex;
//         public bool inited=>_inited;
//         public List<LayoutFollower> Elements=>_elements;
//         public MoveConfig DefaultMoveConfig => _defaultMoveConfig;
//         List<GameObject> _elementPlaceHolders = new List<GameObject>();
//         private float _moveDelayIncrement = 0;
//
//         [SerializeField]private bool debug = false;
//         private void LogDebug(string message)
//         {
//             if (debug)
//             {
//                 Debug.Log(message);
//             }
//         }
//
//         public GameObject GetElement(int index)
//         {
//             if (index < 0 || index >= _elements.Count)
//             {
//                 return null;
//             }
//
//             return _elements[index].gameObject;
//         }
//         private GameObject elementContainer
//         {
//             get
//             {
//                 if (_elementContainer == null)
//                 {
//                     Init();
//                 }
//
//                 return _elementContainer;
//             }
//         }
//
//         public void SetCanUpdate(bool value)
//         {
//             _canUpdate = value;
//             // Debug.Log($"can update => {_canUpdate}");
//         }
//
//         private void Awake()
//         {
//             if (_initOnAwake)
//             {
//                 Init();
//             }
//         }
//
//         private bool _updateLayout = false;
//         private float _updateTimer = 0f;
//         public bool AwaitUpdate=>_updateLayout;
//
//         [Button]
//         private void DebugUpdateLayout()
//         {
//             MarkUpdateLayout();
//         }
//         public void MarkUpdateLayout(Action callback = null, float delay = 0)
//         {
//             // LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
//             // LogDebug("set update layout");
//             _updateLayout = true;
//             _updateTimer = delay;
//             if (callback != null)
//             {
//                 onLayoutUdpateComplete = callback;
//             }
//         }
//
//         public void UpdateLayoutGroupImidiate()
//         {
//             LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
//         }
//
//         public void MarkUpdateLayoutGroup()
//         {
//             LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
//         }
//
//         Dictionary<int,List<LayoutFollower>> _staticIndexTable = new Dictionary<int,List<LayoutFollower>>();
//
//         public void ForceFinishUpdate()
//         {
//             // Debug.Log("force update finish");
//             _updateCallback?.Invoke();
//             foreach (LayoutFollower layoutFollower in _elements)
//             {
//                 layoutFollower.ForceFinish();
//             }
//
//             _busy = false;
//             OnLayoutUpdateComplete();
//         }
//
//         private Action _updateCallback = null;
//
//         public void UpdateLayoutImidiate(Action callback = null)
//         {
//             // LogDebug("layout update");
//             _updateCallback = callback;
//             // UpdateLayoutGroupImidiate();
//             MarkUpdateLayoutGroup();
//
//             int busyElement = 0;
//             float delay = Time.deltaTime;
//             List<LayoutFollower> elements = new List<LayoutFollower>(_elements);
//             foreach (var element in elements)
//             {
//                 element.SyncFollowTarget();
//             }
//
//             for (int i = elements.Count - 1; i >= 0; i--)
//             {
//                 if (elements[i] == null)
//                 {
//                     elements.RemoveAt(i);
//                 }
//                 else if (elements[i].target == null)
//                 {
//                     elements.RemoveAt(i);
//                 }
//                 else if (elements[i].IsFollowing)
//                 {
//                     elements.RemoveAt(i);
//                 }
//             }
//
//             UpdateSortingIndex();
//
//             elements.Sort(new Comparison<LayoutFollower>((a,b)=>
//                 a.Index.CompareTo(b.Index)));
//             bool layoutUpdateCycleReady = false;
//             _busy = true;
//             foreach (LayoutFollower element in elements)
//             {
//                 busyElement++;
//                 // element.transform.SetSiblingIndex(element.target.transform.GetSiblingIndex() - 1);
//                 Manager.DelayCall(delay,()=>
//                 {
//                     element.StartFollowTarget(()=>
//                     {
//                         element.StopFollowTarget();
//                         busyElement--;
//                         if (layoutUpdateCycleReady && busyElement == 0)
//                         {
//                             _busy = false;
//                             OnLayoutUpdateComplete();
//                             _updateCallback?.Invoke();
//                         }
//                     });
//                 });
//                 delay += _moveDelayIncrement;
//             }
//
//             if (busyElement == 0)
//             {
//                 _busy = false;
//             }
//
//             layoutUpdateCycleReady = true;
//             // Debug.Log("clear empty index");
//             _emptyIndex.Clear();
//         }
//
//         private void UpdateSortingIndex()
//         {
//         }
//
//         private void CheckRemoveEmptyPlaceHolder()
//         {
//             if (!inited)
//             {
//                 return;
//             }
//
//             for (int i = _layoutGroup.transform.childCount - 1; i >= 0; i--)
//             {
//                 GameObject child = _layoutGroup.transform.GetChild(i).gameObject;
//                 child.TryGetComponent<LayoutPlaceholderTag>(out LayoutPlaceholderTag layoutTag);
//                 if (child == elementContainer || layoutTag == null)
//                 {
//                     continue;
//                 }
//
//                 if (!_elementPlaceHolders.Contains(child))
//                 {
//                     // Debug.Log("destroyed");
//                     Destroy(child);
//                 }
//             }
//         }
//
//         private Action onLayoutUdpateComplete = null;
//
//         private void OnLayoutUpdateComplete()
//         {
//             onLayoutUdpateComplete?.Invoke();
//             onLayoutUdpateComplete = null;
//         }
//
//         public void SyncSiblingIndex()
//         {
//             foreach (LayoutFollower element in _elements)
//             {
//                 if (element.target == null)
//                 {
//                     continue;
//                 }
//
//                 element.transform.SetSiblingIndex(element.target.transform.GetSiblingIndex() - 1);
//             }
//         }
//         public void SyncSiblingIndexInverse()
//         {
//             int elementCount = _elements.Count;
//             for (int i = elementCount - 1; i >= 0; i--)
//             {
//                 LayoutFollower element = _elements[i];
//                 if (element.target == null)
//                 {
//                     continue;
//                 }
//                 int targetTrueIndex = element.target.transform.GetSiblingIndex();
//                 int inverseIndex = elementCount - targetTrueIndex - 1;
//                 element.transform.SetSiblingIndex(inverseIndex);
//             }
//         }
//
// // LateUpdate so layout group can update first
//         private void LateUpdate()
//         {
//             if (!_canUpdate)
//                 return;
//             OnUpdate();
//         }
//
//         public void OnUpdate()
//         {
//             if (_updateLayout)
//             {
//                 if (_updateTimer > 0)
//                 {
//                     _updateTimer -= Time.deltaTime;
//                     return;
//                 }
//
//                 // SyncSiblingIndex();
//                 CheckRemoveEmptyPlaceHolder();
//                 UpdateLayoutImidiate();
//                 _updateLayout = false;
//             }
//         }
//
//         private void Init()
//         {
//             if (_inited)
//             {
//                 return;
//             }
//
//             _layoutGroup = GetComponent<LayoutGroup>();
//             if (_layoutGroup == null)
//             {
//                 Debug.LogError("layout transistion controller should have at least one child of type LayoutGroup");
//                 return;
//             }
//
//             if (_findElementOnAwake)
//             {
//                 FindUnmanagedElement();
//             }
//
//             _inited = true;
//         }
//
//         private void FindUnmanagedElement()
//         {
//             List<GameObject> unmanagedElements = new List<GameObject>();
//             if (_elementContainer == null)
//             {
//                 _elementContainer = new GameObject("ElementContainer");
//                 _elementContainer.transform.SetParent(transform);
//                 _elementContainer.transform.localScale = Vector3.one;
//                 _elementContainer.AddComponent<LayoutElement>().ignoreLayout = true;
//                 _elementContainer.transform.SetSiblingIndex(0);
//
//                 unmanagedElements = GetAllElementInside(gameObject);
//             }
//             else
//             {
//                 unmanagedElements = GetAllElementInside(_elementContainer);
//             }
//
//             if (unmanagedElements.IsNullOrEmpty())
//             {
//                 return;
//             }
//
//             foreach (GameObject element in unmanagedElements)
//             {
//                 LayoutFollower layoutFollower = AddElement(element,true);
//                 layoutFollower.Init();
//                 layoutFollower.Master = this;
//             }
//             // LogDebug($"unmanaged element found {unmanagedElements.Count}");
//             UpdateLayoutGroupImidiate();
//             MarkUpdateLayoutGroup();
//         }
//
//         public List<GameObject> GetAllElementInside(GameObject parent)
//         {
//             if (parent == null)
//             {
//                 return null;
//             }
//
//             List<GameObject> ret = new List<GameObject>();
//             foreach (Transform childTF in parent.transform)
//             {
//                 childTF.gameObject.TryGetComponent<LayoutElement>(out LayoutElement layoutElement);
//                 if (layoutElement != null && layoutElement.ignoreLayout)
//                 {
//                     continue;
//                 }
//
//                 ret.Add(childTF.gameObject);
//             }
//
//             return ret;
//         }
//
//         public LayoutFollower AddElement(GameObject element,bool forceTeleport = false)
//         {
//             // LogDebug("adding element");
//             if (element == null)
//             {
//                 return null;
//             }
//
//             LayoutFollower layoutFollower = element.GetComponent<LayoutFollower>();
//             if (layoutFollower == null)
//             {
//                 layoutFollower = element.AddComponent<LayoutFollower>();
//             }
//             else
//             {
//                 if (_elements.Contains(layoutFollower))
//                 {
//                     return layoutFollower;
//                 }
//             }
//
//             element.transform.SetParent(elementContainer.transform);
//             // _elements.Add(element);
//
//
//             layoutFollower.Init(_defaultMoveConfig);
//             layoutFollower.Master = this;
//             _elements.Add(layoutFollower);
//
//           GameObject placeHolder = CreatePlaceHolder(element);
//             _elementPlaceHolders.Add(placeHolder);
//             layoutFollower.StopFollowTarget();
//             layoutFollower.SetTarget(placeHolder.transform as RectTransform);
//             SetElementSiblingIndex(layoutFollower,_elements.Count - 1, true);
//
//             // followObject.follow = true;
//             UpdateLayoutGroupImidiate();
//             MarkUpdateLayout();
//             if (forceTeleport)
//             {
//                 layoutFollower.TeleportToTarget();
//             }
//
//             return layoutFollower;
//         }
//         GameObject CreatePlaceHolder(GameObject followerObj)
//         {
//             RectTransform rectTransform = followerObj.GetComponent<RectTransform>();
//             GameObject placeHolder = new GameObject("placeHolder");
//             RectTransform placeholderRTF = Anvil.ExtensionMethods.GetOrAddComponent<RectTransform>(placeHolder);
//             placeHolder.AddComponent<LayoutPlaceholderTag>();
//             placeholderRTF.sizeDelta = rectTransform.sizeDelta;
//             placeholderRTF.SetParent(_layoutGroup.transform);
//             placeholderRTF.localScale = Vector3.one;
//             placeholderRTF.localPosition = Vector3.zero;
//
//             return placeHolder;
//         }
//
//         public void RemoveElement(GameObject element, bool blockUpdate = false)
//         {
//             element.TryGetComponent<LayoutFollower>(out LayoutFollower layoutFollower);
//             if (!_elements.Contains(layoutFollower))
//             {
//                 return;
//             }
//
//             Transform targetPlaceholderTF = layoutFollower.target;
//             _elementPlaceHolders.Remove(targetPlaceholderTF.gameObject);
//             _emptyIndex.CheckAdd(layoutFollower.Index);
//             _elements.Remove(layoutFollower);
//
//             // Transform placeholderTF = _layoutGroup.transform.GetChild(index);
//             if (targetPlaceholderTF == null)
//             {
//                 return;
//             }
//             // Debug.Log($"element removed along with placeholder at {targetPlaceholderTF.GetSiblingIndex()}");
//
//             // Destroy(targetPlaceholderTF.gameObject);
//             if (!blockUpdate)
//             {
//                 MarkUpdateLayout();
//             }
//         }
//
//         public void SetElementSiblingIndex(GameObject obj,int desiredIndex, bool blockUpdate = false)
//         {
//             obj.TryGetComponent<LayoutFollower>(out LayoutFollower layoutFollower);
//             SetElementSiblingIndex(layoutFollower,desiredIndex, blockUpdate);
//         }
//
//         public void SetElementSiblingIndex(LayoutFollower element, int desiredIndex, bool blockUpdate = false)
//         {
//             if (!_elements.Contains(element))
//             {
//                 return;
//             }
//
//             if (element.HasStaticChildIndex)
//             {
//                 // Debug.LogWarning($"setting sibling index for a static index element is not allowed ({element.gameObject.name}). disable it static index first");
//                 return;
//             }
//
//             // Debug.Log($"seting index for obj {element.gameObject.name} => {desiredIndex}");
//
//             List<int> occupiedIndex = new List<int>();
//             List<int> staticIndex = new List<int>();
//             List<LayoutFollower> floatingElements = new List<LayoutFollower>();
//             Dictionary<int,List<LayoutFollower>> staticIndexTable = new Dictionary<int,List<LayoutFollower>>();
//             int maxIndex = 0;
//             int elementCount = _elements.Count;
//             foreach (LayoutFollower layoutFollower in _elements)
//             {
//                 if (layoutFollower == element)
//                 {
//                     continue;
//                 }
//
//                 int index = layoutFollower.Index;
//                 if (index > maxIndex)
//                 {
//                     maxIndex = index;
//                 }
//
//                 occupiedIndex.Add(index);
//                 if (layoutFollower.HasStaticChildIndex)
//                 {
//                     // Debug.Log($"static index found {layoutFollower.StaticChildIndex}");
//                     staticIndex.Add(layoutFollower.StaticChildIndex);
//
//                     if (staticIndexTable.ContainsKey(layoutFollower.StaticChildIndex))
//                     {
//                         staticIndexTable[layoutFollower.StaticChildIndex].Add(layoutFollower);
//                     }
//                     else
//                     {
//                         staticIndexTable.Add(layoutFollower.StaticChildIndex,new List<LayoutFollower>() { layoutFollower });
//                     }
//                 }
//                 else
//                 {
//                     floatingElements.Add(layoutFollower);
//                 }
//             }
//
//             List<LayoutFollower> sortedElements = new List<LayoutFollower>();
//             int currentIndex = 0;
//             for (int i = 0; i < elementCount; i++)
//             {
//                 if (staticIndexTable.ContainsKey(i))
//                 {
//                     bool isDesiredIndex = currentIndex == desiredIndex;
//                     List<LayoutFollower> currentStaticElements = staticIndexTable[i];
//                     foreach (LayoutFollower staticElement in currentStaticElements)
//                     {
//                         sortedElements.Add(staticElement);
//                         currentIndex++;
//                     }
//
//                     if (isDesiredIndex)
//                     {
//                         sortedElements.Add(element);
//                         currentIndex++;
//                     }
//
//                     staticIndexTable.Remove(i);
//
//                     continue;
//                 }
//
//                 if (currentIndex == desiredIndex)
//                 {
//                     sortedElements.Add(element);
//                     currentIndex++;
//                     continue;
//                 }
//
//                 LayoutFollower floatingElement = floatingElements.First();
//                 if (floatingElement == null)
//                 {
//                     currentIndex++;
//                     continue;
//                 }
//
//                 sortedElements.Add(floatingElement);
//                 floatingElements.RemoveAt(0);
//                 currentIndex++;
//             }
//
//             if (staticIndexTable.Count > 0)
//             {
//                 List<int> staticIndexKeys = new List<int>(staticIndexTable.Keys);
//                 staticIndexKeys.Sort();
//                 foreach (int key in staticIndexKeys)
//                 {
//                     List<LayoutFollower> staticElements = staticIndexTable[key];
//                     foreach (LayoutFollower staticElement in staticElements)
//                     {
//                         sortedElements.Add(staticElement);
//                     }
//
//                     if (key == desiredIndex)
//                     {
//                         sortedElements.CheckAdd(element);
//                         currentIndex++;
//                     }
//                 }
//             }
//
//             // StringBuilder sb = new StringBuilder();
//             // sb.Append($"[{gameObject.name}] sort result for: {sortedElements.Count} element\n");
//             for (int i = 0; i < sortedElements.Count; i++)
//             {
//                 LayoutFollower sortedElement = sortedElements[i];
//                 // sb.Append($"{ sortedElement.gameObject.name} \t");
//                 sortedElement.Index = i;
//             }
//
//             _elements = sortedElements;
//
//             // Debug.Log(sb.ToString());
//             if (!blockUpdate)
//             {
//                 MarkUpdateLayout();
//             }
//         }
//
//         [SerializeField] private bool drawIndexGizmos = false;
//         [SerializeField] private bool drawPathGizmos = true;
//
//         private void OnDrawGizmos()
//         {
//
//             foreach (LayoutFollower element in _elements)
//             {
//                 if (drawIndexGizmos)
//                 {
//                     Vector3 indexDrawPos = element.transform.position;
//                     // indexDrawPos.x += 0.8f;
//                     // indexDrawPos.y += 0.8f;
//                     GizmosHelper.DrawText(element.Index.ToString(),indexDrawPos,Color.green);
//                 }
//
//                 if (element.target == null)
//                 {
//                     GizmosHelper.DrawCross(element.transform.position,1f,Color.red);
//                     continue;
//                 }
//                 if (drawPathGizmos)
//                 {
//                     GizmosHelper.DrawLine(element.transform.position,element.target.transform.position,0.15f,
//                         Color.green);
//                     // GizmosHelper.draw
//                 }
//             }
//         }
//
//         public void Reset()
//         {
//             _elements.Clear();
//         }
//
//         public void CleanUp()
//         {
//             GameObjectPool.ClearChild(_elementContainer);
//             GameObjectPool.ClearChild(gameObject);
//             
//             _elements.Clear();
//             _elementPlaceHolders.Clear();
//         }
//
//         public void SetDefaultMoveTime(float f)
//         {
//             _defaultMoveConfig.SetDuration(f);
//             foreach (LayoutFollower element in _elements)
//             {
//                 element.UpdateMoveConfig(_defaultMoveConfig);
//             }
//         }
//     }
// }
