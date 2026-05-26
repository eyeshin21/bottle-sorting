using UnityEngine;

namespace Anvil.Legacy
{
    public class BorderNode
    {
        float _x, _y;
        CornerType _cornerType;
        bool _isOuter;
        BorderNode _hLink;
        BorderNode _vLink;

        public float X
        {
            get => _x;
            set => _x = value;
        }
        public float Y
        {
            get => _y;
            set => _y = value;
        }
        public CornerType CornerType => _cornerType;
        public bool Inner => !_isOuter;
        public bool Outer => _isOuter;

        public CornerType CornerTypeWithoutOuter
        {
            get
            {
                if (_isOuter)
                {
                    if (_cornerType == CornerType.TopLeft) return CornerType.TopRight;
                    if (_cornerType == CornerType.TopRight) return CornerType.BottomRight;
                    if (_cornerType == CornerType.BottomLeft) return CornerType.TopLeft;
                    if (_cornerType == CornerType.BottomRight) return CornerType.BottomLeft;
                }
                return _cornerType;
            }
        }

        public BorderNode HLink
        {
            get => _hLink;
            set
            {
                Assert.IsNull(_hLink);
                Assert.IsNull(value._hLink);
                _hLink = value;
            }
        }

        public BorderNode VLink => _vLink;

        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        public float UVLeft { get; set; }
        public float UVTop { get; set; }
        public float UVRight { get; set; }
        public float UVBottom { get; set; }

        public bool Dark { get; set; }

        public BorderNode(CornerType cornerType, float x, float y, bool outer = false)
        {
            //Log.Debug($"{cornerType}: x={x}, y={y}, outer={outer}");
            _x = x;
            _y = y;
            _cornerType = cornerType;
            _isOuter = outer;
        }

        public BorderNode(float x, float y, CornerType cornerType, bool outer = false)
        {
            _x = x;
            _y = y;
            _cornerType = cornerType;
            _isOuter = outer;
        }

        public void LinkHorizontal(BorderNode other)
        {
            Assert.IsNull(_hLink);
            Assert.IsNull(other._hLink);
            _hLink = other;
            other._hLink = this;
            //Log.Debug($"{this} link horizontal {other}");
        }

        public void LinkVertical(BorderNode other)
        {
            Assert.IsNull(_vLink);
            Assert.IsNull(other._vLink);
            _vLink = other;
            other._vLink = this;
            //Log.Debug($"{this} link vertical {other}");
        }

#if UNITY_EDITOR
        float _prevX, _prevY;
        public void BackupXY()
        {
            _prevX = _x;
            _prevY = _y;
        }

        public void RestoreXY()
        {
            _x = _prevX;
            _y = _prevY;
        }

        public override string ToString()
        {
            return $"[{_cornerType}: x={_x}, y={_y}, outer={_isOuter}]";
        }
#endif
    }
}