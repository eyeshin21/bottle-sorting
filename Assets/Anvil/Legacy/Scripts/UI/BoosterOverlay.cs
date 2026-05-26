using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using System;

namespace Anvil.Legacy
{
    public class BoosterOverlay : UIMeshBehaviour, IPointerClickHandler
    {
        static readonly float OverlayExtra = 0.02f; // Out of screen

        enum State
        {
            None,
            Showing,
            Show,
            Hidding,
            Hide,
        }

        [SerializeField] float _boardBorder = 32; // Sprite border in pixels
        [SerializeField, Tooltip("Left, Bottom, Right, Top")] Vector4 _boardMargin = new Vector4(0.3f, 0.3f, 0.3f, 0.3f);
        [SerializeField] float _endAlpha = 0.75f;
        [SerializeField] float _showDuration = 0.2f;
        [SerializeField] float _hideDuration = 0.2f;

        Vector3 _boardPos;
        float _boardWidth, _boardHeight;
        Vector3 _boosterPos;
        float _boosterWidth, _boosterHeight;
        Callback _showCallback, _hideCallback;

        float _boardLeft, _boardTop, _boardRight, _boardBottom;
        float _boosterLeft, _boosterTop, _boosterRight, _boosterBottom;
        State _state = State.None;
        float _startAlpha, _deltaAlpha;
        float _time;

        public bool Showing => _state == State.Showing || _state == State.Show;
        public Callback OnClickHandler { get; set; }

        /// <summary>
        /// In units.
        /// </summary>
        public void Show(float boardLeft, float boardTop, float boardRight, float boardBottom, Vector3 boosterPos, float boosterWidth, float boosterHeight, Callback callback = null)
        {
            Show(new Vector3((boardLeft + boardRight) * 0.5f, (boardTop + boardBottom) * 0.5f), boardRight - boardLeft, boardTop - boardBottom, boosterPos, boosterWidth, boosterHeight, callback);
        }

        /// <summary>
        /// In units.
        /// </summary>
        public void Show(Vector3 boardPos, float boardWidth, float boardHeight, Vector3 boosterPos, float boosterWidth, float boosterHeight, Callback callback = null)
        {
            _boardPos = boardPos;
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;

            _boosterPos = boosterPos;
            _boosterWidth = boosterWidth;
            _boosterHeight = boosterHeight;

            _showCallback = callback;

            _boardLeft = boardPos.x - boardWidth * 0.5f;
            _boardRight = _boardLeft + boardWidth;
            _boardBottom = boardPos.y - boardHeight * 0.5f;
            _boardTop = _boardBottom + boardHeight;

            _boardLeft -= _boardMargin.x;
            _boardRight += _boardMargin.z;
            _boardBottom -= _boardMargin.y;
            _boardTop += _boardMargin.w;

            _boosterLeft = boosterPos.x - boosterWidth * 0.5f;
            _boosterRight = _boosterLeft + boosterWidth;
            _boosterBottom = boosterPos.y - boosterHeight * 0.5f;
            _boosterTop = _boosterBottom + boosterHeight;

            _state = State.Showing;
            _startAlpha = 0;
            _deltaAlpha = _endAlpha - _startAlpha;
            _time = 0;
            Alpha = _startAlpha;

            gameObject.SetActive(true);
        }

        public void Hide(Callback callback = null)
        {
            _hideCallback = callback;

            _state = State.Hidding;
            _startAlpha = Alpha;
            _deltaAlpha = -_startAlpha;
            _time = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke();
        }

        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            if (_state == State.Show || _state == State.Hidding)
            {
                var pos = eventCamera.ScreenToWorldPoint(sp);
                float x = pos.x;
                float y = pos.y;

                // Check inside board
                if (x > _boardLeft && x < _boardRight && y > _boardBottom && y < _boardTop)
                {
                    return false;
                }

                // Check inside booster
                //if (x > _boosterLeft && x < _boosterRight && y > _boosterBottom && y < _boosterTop)
                //{
                //    return false;
                //}
            }

            return true;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            var sprite = activeSprite;
            if (sprite == null) return;
            sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

            float uvScaleX = 1f / sprite.texture.width;
            float uvScaleY = 1f / sprite.texture.height;

            float boardUVLeft = uvLeft;
            float boardUVTop = uvTop;
            float boardUVRight = uvRight;
            float boardUVBottom = (uvTop + uvBottom) * 0.5f;
            float boardUVX = boardUVLeft + _boardBorder * uvScaleX;
            float boardUVZ = boardUVRight - _boardBorder * uvScaleX;
            float boardUVY = boardUVBottom + _boardBorder * uvScaleY;
            float boardUVW = boardUVTop - _boardBorder * uvScaleY;

            float boosterUVLeft = uvLeft;
            float boosterUVTop = boardUVBottom;
            float boosterUVRight = uvRight;
            float boosterUVBottom = uvBottom;

            float pixelUVLeft = uvLeft;
            float pixelUVTop = uvTop;
            float pixelUVRight = pixelUVLeft + uvScaleX;
            float pixelUVBottom = pixelUVTop - uvScaleY;

            Context.MainCamera.GetAABB(out float left, out float top, out float right, out float bottom);
            float scale = CanvasScale;
            if (!Mathf.Approximately(scale, 1))
            {
                float width = (right - left) * scale;
                float height = (top - bottom) * scale;
                left = (left + right - width) * 0.5f;
                right = left + width;
                bottom = (bottom + top - height) * 0.5f;
                top = bottom + height;
            }
            left -= OverlayExtra;
            right += OverlayExtra;
            bottom -= OverlayExtra;
            top += OverlayExtra;

            float boardWidth = (_boardWidth + _boardMargin.x + _boardMargin.z) * scale;
            float boardHeight = (_boardHeight + _boardMargin.y + _boardMargin.w) * scale;
            float boardPosX = _boardPos.x * scale;
            float boardPosY = _boardPos.y * scale;
            float boardLeft = (boardPosX - boardWidth * 0.5f + (_boardMargin.z - _boardMargin.x) * 0.5f) * PixelsPerUnit;
            float boardTop = (boardPosY + boardHeight * 0.5f + (_boardMargin.w - _boardMargin.y) * 0.5f) * PixelsPerUnit;
            float boardRight = boardLeft + boardWidth * PixelsPerUnit;
            float boardBottom = boardTop - boardHeight * PixelsPerUnit;

            float boosterWidth = _boosterWidth;// * scale;
            float boosterHeight = _boosterHeight;// * scale;
            float boosterPosX = _boosterPos.x * scale;
            float boosterPosY = _boosterPos.y * scale;
            float boosterLeft = (boosterPosX - boosterWidth * 0.5f) * PixelsPerUnit;
            float boosterTop = (boosterPosY + boosterHeight * 0.5f) * PixelsPerUnit;
            float boosterRight = boosterLeft + boosterWidth * PixelsPerUnit;
            float boosterBottom = boosterTop - boosterHeight * PixelsPerUnit;

            left = Mathf.Min(left * PixelsPerUnit, boardLeft);
            top = Mathf.Max(top * PixelsPerUnit, boardTop);
            right = Mathf.Max(right * PixelsPerUnit, boardRight);
            bottom = Mathf.Min(bottom * PixelsPerUnit, boosterBottom);

            // Board-Top
            AddQuad(vh, left, top, right, boardTop, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);

            // Board-Middle
            {
                // Left
                AddQuad(vh, left, boardTop, boardLeft, boardBottom, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);

                float x1 = boardLeft + _boardBorder;
                float x2 = boardRight - _boardBorder;
                float y1 = boardTop - _boardBorder;
                float y2 = boardBottom + _boardBorder;
                // Top
                AddQuad(vh, boardLeft, boardTop, x1, y1, boardUVLeft, boardUVTop, boardUVX, boardUVW);
                AddQuad(vh, x1, boardTop, x2, y1, boardUVX, boardUVTop, boardUVZ, boardUVW);
                AddQuad(vh, x2, boardTop, boardRight, y1, boardUVZ, boardUVTop, boardUVRight, boardUVW);
                // Middle
                AddQuad(vh, boardLeft, y1, x1, y2, boardUVLeft, boardUVW, boardUVX, boardUVY);
                AddQuad(vh, x2, y1, boardRight, y2, boardUVZ, boardUVW, boardUVRight, boardUVY);
                // Bottom
                AddQuad(vh, boardLeft, y2, x1, boardBottom, boardUVLeft, boardUVY, boardUVX, boardUVBottom);
                AddQuad(vh, x1, y2, x2, boardBottom, boardUVX, boardUVY, boardUVZ, boardUVBottom);
                AddQuad(vh, x2, y2, boardRight, boardBottom, boardUVZ, boardUVY, boardUVRight, boardUVBottom);

                // Right
                AddQuad(vh, boardRight, boardTop, right, boardBottom, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);
            }

            // Board-Bottom
            AddQuad(vh, left, boardBottom, right, boosterTop, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);

            // Booster-Middle
            {
                // Left
                AddQuad(vh, left, boosterTop, boosterLeft, boosterBottom, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);
                // Middle
                AddQuad(vh, boosterLeft, boosterTop, boosterRight, boosterBottom, boosterUVLeft, boosterUVTop, boosterUVRight, boosterUVBottom);
                // Right
                AddQuad(vh, boosterRight, boosterTop, right, boosterBottom, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);
            }

            // Booster-Bottom
            AddQuad(vh, left, boosterBottom, right, bottom, pixelUVLeft, pixelUVTop, pixelUVRight, pixelUVBottom);
        }

        void Update()
        {
            if (_state == State.Showing)
            {
                _time += Time.deltaTime;
                if (_time < _showDuration)
                {
                    Alpha = _startAlpha + _deltaAlpha * _time / _showDuration;
                }
                else
                {
                    Alpha = _startAlpha + _deltaAlpha;
                    _state = State.Show;
                    _showCallback?.Invoke();
                }
            }
            else if (_state == State.Hidding)
            {
                _time += Time.deltaTime;
                if (_time < _hideDuration)
                {
                    Alpha = _startAlpha + _deltaAlpha * _time / _hideDuration;
                }
                else
                {
                    Alpha = _startAlpha + _deltaAlpha;
                    _state = State.Hide;
                    gameObject.SetActive(false);
                    _hideCallback?.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (raycastTarget)
            {
                GizmosHelper.DrawAABB(_boardLeft, _boardTop, _boardRight, _boardBottom, Color.green);
                GizmosHelper.DrawAABB(_boosterLeft, _boosterTop, _boosterRight, _boosterBottom, Color.green);
            }
        }
#endif
    }
}