using UnityEngine;

namespace Anvil.Legacy
{
    public struct Range
    {
        float? _min;
        float? _max;

        public void Add(float value)
        {
            if (!_min.HasValue || value < _min.Value)
            {
                _min = value;
            }
            if (!_max.HasValue || value > _max.Value)
            {
                _max = value;
            }
        }

        public override string ToString()
        {
            if (_min.HasValue)
            {
                if (_max.HasValue)
                {
                    return $"[{_min.Value}, {_max.Value}]";
                }
                return $"[{_min.Value}, N/A]";
            }
            if (_max.HasValue)
            {
                return $"[N/A, {_max.Value}]";
            }
            return $"N/A";
        }
    }
}