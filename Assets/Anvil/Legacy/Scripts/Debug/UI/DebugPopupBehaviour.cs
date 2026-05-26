#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class DebugPopupBehaviour : MonoBehaviour
    {
        protected IAnimationController _animationController;
        protected float _delay;
        protected Callback _delayCallback;

        protected Transform _popup;
        protected Transform Popup => _popup ??= transform.GetChild("Popup");

        protected Transform _top;
        protected Transform Top => _top ??= Popup.GetChild("Top");

        protected Transform _content;
        protected virtual Transform Content => _content ??= Popup.GetChild("Content");

        protected Transform _bottom;
        protected Transform Bottom => _bottom ??= Popup.GetChild("Bottom");

        protected Transform _titleParent;
        protected Transform TitleParent => _titleParent ??= Top.GetChild("Container");

        protected static DebugUIConfig _config;
        protected static DebugUIConfig Config => _config ??= DebugUIConfig.Instance;

        protected void AddTitle(string text)
        {
            var title = Config.Title.Create(TitleParent);
            title.SetText(text);
        }

        protected void AddButtonClose(Listener onClick)
        {
            var button = Config.ButtonClose.Create<Button>(Top);
            button.AddListener(onClick);
        }

        protected Button AddButton(string text, Listener onClick)
        {
            var button = Config.Button.Create<Button>(Bottom);
            var textController = TextController.Create(button);
            if (textController != null)
            {
                textController.Text = text;
                //textController.ForceUpdateText();
                var textSize = textController.TextSize;
                var buttonRectTransform = button.transform as RectTransform;
                var buttonSize = buttonRectTransform.sizeDelta;
                //Log.Debug($"textSize={textSize}, buttonSize={buttonSize}");
                if (textSize.x > buttonSize.x)
                {
                    buttonSize.x = textSize.x;
                    buttonRectTransform.sizeDelta = buttonSize;
                }
            }
            else
            {
                button.SetText(text);
            }
            button.AddListener(onClick);
            return button;
        }

        //protected Button AddButton(string text, Callback callback)
        //{
        //    var button = Config.Button.Create<Button>(Bottom);
        //    button.SetText(text);
        //    button.AddCallback(callback);
        //    return button;
        //}

        protected void AddButtons(string text1, string text2, Listener onClick1, Listener onClick2, Callback<Button, Button> callback = null)
        {
            var buttons = _bottom.CreateChild("Buttons");
            var layoutGroup = buttons.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 50;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            var buttonPrefab = Config.Button;
            var button1 = buttonPrefab.Create<Button>(buttons);
            button1.SetText(text1);
            button1.AddListener(onClick1);
            var button2 = buttonPrefab.Create<Button>(buttons);
            button2.SetText(text2);
            button2.AddListener(onClick2);
            callback?.Invoke(button1, button2);
        }

        protected virtual void PlayAnimation(string name)
        {
            if (_animationController == null)
            {
                _animationController = AnimationController.Create(gameObject, true);
                if (_animationController == null)
                {
                   LegacyLog.Warning($"Can't play animation \"{name}\"!");
                    return;
                }
            }
            _animationController.PlayAnimation(name);
        }

        protected virtual void Show(Callback doneCallback = null)
        {
            DebugManager.AddKeyBlocker(name);
            PlayAnimation("Show");
            DelayCall(0.2f, () =>
            {
                SoundManager.PlaySoundPopup();
                doneCallback?.Invoke();
            });
        }

        protected virtual void Hide()
        {
            Hide(null);
        }

        protected virtual void Hide(Callback<int> buttonCallback, int buttonId)
        {
            SoundManager.PlaySoundButton();
            Hide(() => buttonCallback?.Invoke(buttonId));
        }

        protected virtual void Hide(Callback doneCallback)
        {
            DebugManager.RemoveKeyBlocker(name);
            PlayAnimation("Hide");
            DelayCall(0.1f, () =>
            {
                doneCallback?.Invoke();
                DelayCall(0.1f, () =>
                {
                    Destroy(gameObject);
                });
            });
        }

        protected void DelayCall(float delay, Callback callback)
        {
            if (callback != null)
            {
                if (delay > 0)
                {
                    _delay = delay;
                    _delayCallback = callback;
                }
                else
                {
                    callback();
                }
            }
        }

        protected virtual void Update()
        {
            if (_delay > 0)
            {
                _delay -= Time.deltaTime;
                if (_delay <= 0)
                {
                    if (_delayCallback != null)
                    {
                        var callback = _delayCallback;
                        _delayCallback = null;
                        callback();
                    }
                }
            }
        }
    }
}
#endif