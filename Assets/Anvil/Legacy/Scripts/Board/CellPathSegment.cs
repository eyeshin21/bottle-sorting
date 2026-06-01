using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CellPathSegment
    {
        Direction4 _direction;
        int _length;

        public Direction4 Direction => _direction;
        public int Length
        {
            get => _length;
            set => _length = value;
        }

        //public CellPathSegment Next { get; set; }

        public void Construct(Direction4 direction, int length)
        {
            Assert.IsGreaterThan(length, 1);
            _direction = direction;
            _length = length;
        }

        public void Get(out Direction4 direction, out int length)
        {
            direction = _direction;
            length = _length;
        }

        public override string ToString()
        {
            return $"{_direction}-{_length}";
        }

        #region Pool
        static Pool<CellPathSegment> _pool = new();

        public static CellPathSegment Create(Direction4 direction, int length)
        {
            var segment = _pool.Get();
            segment.Construct(direction, length);
            return segment;
        }

        public static void Return(CellPathSegment segment)
        {
            Assert.IsNotNull(segment);
            _pool.Return(segment);
        }

        public static void Return(List<CellPathSegment> segments)
        {
            _pool.Return(segments);
        }
        #endregion
    }
}