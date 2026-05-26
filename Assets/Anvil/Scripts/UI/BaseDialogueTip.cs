// using System;
// using Gametamin.Core;
// using UnityEngine;
//
// namespace Gametamin.Utility
// {
//     public class BaseDialogueTip : MonoBehaviour
//     {
//         public static float defaultFontSize = -1;
//         public static float ScreenPaddingL = 40;
//         public static float ScreenPaddingR = 40;
//
//         public static float TextPaddingLR = 30;
//         public static float TextPaddingUD = 30;
//         public static float MinHeigh = 150;
//         protected GameObjectReference _iref;
//         protected GameObjectReference Iref
//         {
//             get
//             {
//                 if (_iref == null)
//                 {
//                     _iref = GetComponent<GameObjectReference>();
//                 }
//                 return _iref;
//             }
//         }
//
//         protected RectTransform _arrowRTF;
//         Vector3 _rootPos;
//         [SerializeField] private bool _tapToClose = true;
//
//         private void Awake()
//         {
//
//         }
//         public virtual void Show(Vector3 position, Direction direction = Direction.Up, Action<GameObject, ItemData> onElementCreated = null)
//         {
//
//             position.z = 0;
//             _rootPos = position;
//
//             RectTransform rectTransform = transform as RectTransform;
//
//             _arrowRTF = SetDirection(direction);
//             rectTransform.position = position;
//
//             LoadContent(onElementCreated);
//             UpdateOffset();
//         }
//
//         public virtual void UpdateOffset()
//         {
//             RectTransform rectTransform = transform as RectTransform;
//
//             RectTransform canvasRTF = PoolHelper.ParentPopup.GetComponent<RectTransform>();
//             float availableW = canvasRTF.rect.width - (ScreenPaddingL + ScreenPaddingR);
//
//             GameObject rootObj = Iref.GetGameObjectReference(GameObjectReferenceID.Root);
//             RectTransform rootRTF = rootObj.GetComponentSafe<RectTransform>();
//
//             Vector2 preferedSize = new Vector2(rootRTF.rect.width, rootRTF.rect.height);
//             // Debug.Log($"prefered size {preferedSize}");
//
//             //Offset calculation
//             float offSetX = TextTip.CalculateHorizontalOffset(canvasRTF.rect, rootRTF.rect, rectTransform.localPosition,
//                 new Vector4(ScreenPaddingL, 0, ScreenPaddingR, 0));
//             // Debug.Log($"offset {offSetX}");
//
//             Vector3 rootPos = rootRTF.anchoredPosition;
//             rootPos.x = offSetX;
//             rootRTF.anchoredPosition = rootPos;
//             // _iref.PlayShowAnimation();
//
//             if (_tapToClose)
//             {
//                 UserInput.OnPointerDown += OnPointerDown;
//             }
//
//             void OnPointerDown(Vector2 position)
//             {
//                 UserInput.OnPointerDown -= OnPointerDown;
//                 Iref.PlayHideAnimation(() => { GeneralPoolManager.RemoveObject(gameObject); });
//             }
//
//
//             if (_arrowRTF)
//             {
//                 Vector3 arrowPos = new Vector3(-rootRTF.anchoredPosition.x, _arrowRTF.localPosition.y, 0);
//                 _arrowRTF.localPosition = arrowPos;
//             }
//         }
//
//         protected virtual void LoadContent(Action<GameObject, ItemData> onElementCreated = null)
//         {
//             UpdateOffset();
//         }
//
//         private RectTransform SetDirection(Direction direction)
//         {
//             RectTransform ret = null;
//             // RectTransform rectTransform = _iref.GetComponent<RectTransform>();
//             GameObject rootObj = Iref.GetGameObjectReference(GameObjectReferenceID.Root);
//             RectTransform rootRTF = rootObj.GetComponentSafe<RectTransform>();
//             Vector2 originalPivot = rootRTF.pivot;
//             if (direction != Direction.Down && direction != Direction.Up)
//             {
//                 direction = Direction.Up;
//                 Debug.LogWarning($"[TextTip] Direction {direction} is not supported. default to UP");
//             }
//
//             if (direction == Direction.Down)
//             {
//                 rootRTF.pivot = new Vector2(originalPivot.x, 1);
//                 Iref.SetActiveGameObjectReference(GameObjectReferenceID.Bottom, false);
//                 Iref.SetActiveGameObjectReference(GameObjectReferenceID.Top, true);
//                 ret = Iref.GetComponentReference<RectTransform>(GameObjectReferenceID.Top);
//             }
//
//             if (direction == Direction.Up)
//             {
//                 rootRTF.pivot = new Vector2(originalPivot.x, 0);
//                 Iref.SetActiveGameObjectReference(GameObjectReferenceID.Bottom, true);
//                 Iref.SetActiveGameObjectReference(GameObjectReferenceID.Top, false);
//                 ret = Iref.GetComponentReference<RectTransform>(GameObjectReferenceID.Bottom);
//             }
//
//             rootRTF.anchoredPosition = Vector2.zero;
//
//             return ret;
//         }
//     }
// }
