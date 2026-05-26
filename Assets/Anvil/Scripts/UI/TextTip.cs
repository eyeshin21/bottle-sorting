#region
using System;
using Anvil.Legacy;
using TMPro;
using UnityEngine;
#endregion

namespace Anvil
{
    public static class TextTip
    {
        public static float defaultFontSize = -1;
        public static float ScreenPaddingL = 40;
        public static float ScreenPaddingR = 40;

        public static float TextPaddingLR = 30;
        public static float TextPaddingUD = 30;
        public static float MinHeigh = 150;

        public static GameObject ShowTextTip(string text,RectTransform targetRTF,Direction direction = Direction.Up,GameObject prefab = null, Action onShow = null,Action onHide = null)
        {
            Vector3 worldPos = targetRTF.position;

            float horizonLine = worldPos.y;
            Vector3[] corners = new Vector3[4];
            targetRTF.GetWorldCorners(corners);
            if (direction.IsDown())
            {
                horizonLine = (corners[0].y + corners[3].y) / 2;
            }
            else
            {
                horizonLine = (corners[1].y + corners[2].y) / 2;
            }
            worldPos.y = horizonLine;
            GameObject textTipObj = ShowTextTip(text,worldPos,direction, prefab,onShow,onHide);
            return textTipObj;
        }

        public static GameObject ShowTextTip(string text,Vector3 position,Direction direction = Direction.Up, GameObject prefab = null, Action onShow = null,Action onHide = null)
        {
            position.z = 0;
            if (prefab == null)
            {
                return null;
            }
            GameObject textTipObj = GameObjectPool.CreateObject(PoolHelperAdapter.ParentPopup,prefab);
            IGameObjectReferenceAdapter iref = textTipObj.CreateGameObjectReferenceAdapter();
            RectTransform rectTransform = textTipObj.transform as RectTransform;
            TextMeshProUGUI textComponent = iref.GetComponentReference<TextMeshProUGUI>(GameObjectReferenceIDAdapter.Text);
            if (defaultFontSize <= 0)
            {
                defaultFontSize = textComponent.fontSize;
            }

            GameObject rootObj = iref.GetGameObjectReference(GameObjectReferenceIDAdapter.Root);
            RectTransform rootRTF = rootObj.TryGetComponent<RectTransform>();

            RectTransform arrowRTF = iref.SetDirection(direction);
            rectTransform.transform.position = position;

            RectTransform canvasRTF = PoolHelperAdapter.ParentPopup.GetComponent<RectTransform>();
            float availableW = canvasRTF.rect.width - (ScreenPaddingL + ScreenPaddingR);

            textComponent.SetText(text);
            Vector2 preferedSize = new Vector2(textComponent.preferredWidth,textComponent.preferredHeight);
            // Debug.Log($"prefered size {preferedSize}");

            rootRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,Mathf.Min(preferedSize.x, availableW));
            Action updateHeigh = ()=>
            {
                rootObj.SetActive(true);
                preferedSize.y = Mathf.Max(MinHeigh,textComponent.preferredHeight);
                rootRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,preferedSize.y);
            };
            rootObj.SetActive(false);
            updateHeigh.ExecuteNextFrame();

            //Offset calculation
            float offSetX = CalculateHorizontalOffset(canvasRTF.rect,rootRTF.rect,rectTransform.localPosition,new Vector4(ScreenPaddingL,0,ScreenPaddingR,0));

            Vector3 rootPos = rootRTF.anchoredPosition;
            rootPos.x += offSetX;
            rootRTF.anchoredPosition = rootPos;
            arrowRTF.position = position;
            iref.PlayShowAnimation(()=>{ onShow?.Invoke(); });

            CallOnInput(OnPointerDown);

            void OnPointerDown()
            {
                iref.PlayHideAnimation(()=>
                {
                    onHide?.Invoke();
                    GameObjectPool.RemoveObject(textTipObj);
                });
            }

            return textTipObj;
        }

        private static void CallOnInput(Action action)
        {
            //TODO  
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentRect">parent rect or the bounding box</param>
        /// <param name="childRect">child rect</param>
        /// <param name="childLocalPos">local position of the child rect inside</param>
        /// <param name="margin">left, top, right, down</param>
        /// <returns></returns>
        public static float CalculateHorizontalOffset(Rect parentRect,Rect childRect,Vector3 childLocalPos,Vector4 margin)
        {
            float halfW = childRect.width / 2;
            float spaceAvailable_L = parentRect.width * 0.5f + childLocalPos.x - ScreenPaddingL;
            float spaceAvailable_R = parentRect.width - (spaceAvailable_L + ScreenPaddingL) - ScreenPaddingR;
            float deltaL = spaceAvailable_L - halfW;
            float deltaR = spaceAvailable_R - halfW;
            float offSetX = 0;
            if (deltaL <= 0)
            {
                offSetX = Mathf.Abs(deltaL);
            }
            else if (deltaR <= 0)
            {
                offSetX = -Mathf.Abs(deltaR);
            }

            return offSetX;
        }

        // private static Vector2 ClampRectInsideScreen(Rect screenRect, RectTransform rectTransform, float paddingL, float paddingR)
        // {
        //     Vector2 ret = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        //
        // }
        private static RectTransform SetDirection(this IGameObjectReferenceAdapter iref,Direction direction)
        {
            RectTransform ret = null;
            RectTransform rectTransform = iref.GetComponent<RectTransform>();
            GameObject rootObj = iref.GetGameObjectReference(GameObjectReferenceIDAdapter.Root);
            RectTransform rootRTF = rootObj.TryGetComponent<RectTransform>();
            if (direction != Direction.Down && direction != Direction.Up)
            {
                direction = Direction.Up;
                Debug.LogWarning($"[TextTip] Direction {direction} is not supported. default to UP");
            }

            if (direction == Direction.Down)
            {
                rootRTF.pivot = new Vector2(0.5f,1);
                iref.SetActiveGameObjectReference(GameObjectReferenceIDAdapter.Bottom,false);
                iref.SetActiveGameObjectReference(GameObjectReferenceIDAdapter.Top,true);
                ret = iref.GetComponentReference<RectTransform>(GameObjectReferenceIDAdapter.Top);
            }

            if (direction == Direction.Up)
            {
                rootRTF.pivot = new Vector2(0.5f,0);
                iref.SetActiveGameObjectReference(GameObjectReferenceIDAdapter.Bottom,true);
                iref.SetActiveGameObjectReference(GameObjectReferenceIDAdapter.Top,false);
                ret = iref.GetComponentReference<RectTransform>(GameObjectReferenceIDAdapter.Bottom);
            }

            rootRTF.anchoredPosition = Vector2.zero;

            return ret;
        }
    }
}
