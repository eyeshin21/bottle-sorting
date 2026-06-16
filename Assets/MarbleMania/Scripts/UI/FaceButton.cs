using UnityEngine;

namespace Anvil
{
    public class FaceButton : UIButton
    {
        [SerializeField]protected GameObject _displayImageObject;
        protected SpriteAdapter _buttonImageAdapter = null;
        protected override void Awake()
        {
            base.Awake();

            if (_displayImageObject != null)
            {
                _buttonImageAdapter = SpriteAdapter.Create(_displayImageObject);
 
            }

        }
        
        public void SetDisplayButton(Sprite sprite)
        {
            if (sprite == null)
            {
                return;
            }
            if (_buttonImageAdapter != null)
            {
                SetDisplayButton(true);
                _buttonImageAdapter.Sprite = sprite;
                return;
            }
        }

        public void SetDisplayButton(bool show)
        {
            if (_displayImageObject != null)
            {
                _displayImageObject.SetActive(show);
            }
        }
        public void SetDisplayButtonColor(Color color)
        {
            if (_buttonImageAdapter != null)
            {
                _buttonImageAdapter.Color = color;
            }
        }
    }
}