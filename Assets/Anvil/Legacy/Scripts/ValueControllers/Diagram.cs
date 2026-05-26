using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public abstract class Diagram<T>
    {
        public abstract class Segment<TValue>
        {
            protected float _startTime;
            protected TValue _startValue;
            protected float _endTime;
            protected TValue _endValue;

            public float StartTime => _startTime;
            public TValue StartValue => _startValue;
            public float EndTime => _endTime;
            public TValue EndValue => _endValue;

            protected void Construct(float startTime, TValue startValue, float endTime, TValue endValue)
            {
                _startTime = startTime;
                _startValue = startValue;
                _endTime = endTime;
                _endValue = endValue;
            }

            public TValue GetValue(float time)
            {
                if (time <= _startTime) return _startValue;
                if (time >= _endTime) return _endValue;
                return GetValueAt((time - _startTime) / (_endTime - _startTime));
            }

            /// <summary>
            /// Returns value at t within [0,1].
            /// </summary>
            protected abstract TValue GetValueAt(float t);

            public bool Equals(Segment<TValue> segment)
            {
                return _startTime == segment._startTime && _startValue.Equals(segment._startValue) && _endTime == segment._endTime && _endValue.Equals(segment._endValue);
            }

            public override string ToString()
            {
                return $"[({_startTime:0.0}:{_startValue}), ({_endTime:0.0}:{_endValue})]";
            }
        }

        protected List<Segment<T>> _segments = new List<Segment<T>>();

        public T StartValue => _segments[0].StartValue;
        public float EndTime => _segments[_segments.Count - 1].EndTime;
        public T EndValue => _segments[_segments.Count - 1].EndValue;

        protected void Construct(float startTime, T startValue, float endTime, T endValue)
        {
            Assert.IsGreaterThanOrEquals(endTime, startTime);
            Assert.IsEmpty(_segments);
            _segments.Add(CreateSegment(startTime, startValue, endTime, endValue));
        }

        protected abstract Segment<T> CreateSegment(float startTime, T startValue, float endTime, T endValue);

        public void Add(float time, T value)
        {
            int count = _segments.Count;
            Assert.IsPositive(count);
            var last = _segments[count - 1];
            Assert.IsGreaterThanOrEquals(time, last.EndTime);
            _segments.Add(CreateSegment(last.EndTime, last.EndValue, time, value));
        }

        public T GetValue(float time)
        {
            int count = _segments.Count;
            for (int i = 0; i < count - 1; i++)
            {
                var segment = _segments[i];
                if (time <= segment.EndTime)
                {
                    return segment.GetValue(time);
                }
            }
            return _segments[count - 1].GetValue(time);
        }

        public T GetValue(ref int segmentIndex, float time)
        {
            int count = _segments.Count;
            if (segmentIndex < count - 1)
            {
                var segment = _segments[segmentIndex];
                if (time < segment.EndTime)
                {
                    return segment.GetValue(time);
                }

                segmentIndex++;
                return segment.EndValue;
            }

            return _segments[count - 1].GetValue(time);
        }

        public bool IsTimeWithin01()
        {
            int count = _segments.Count;
            float time;
            for (int i = 0; i < count; i++)
            {
                time = _segments[i].StartTime;
                if (time < 0 || time > 1)
                {
                    return false;
                }
            }
            time = _segments[count - 1].EndTime;
            return time >= 0 && time <= 1;
        }

        public bool Equals(Diagram<T> diagram)
        {
            int count = _segments.Count;
            var segments2 = diagram._segments;
            int count2 = segments2.Count;
            if (count != count2) return false;

            for (int i = 0; i < count; i++)
            {
                if (!_segments[i].Equals(segments2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            int count = _segments.Count;
            if (count == 0) return "[]";

            var segment = _segments[0];
            if (count == 1) return segment.ToString();

            return Helper.CreateString(sb =>
            {
                sb.Append($"[({segment.StartTime:0.0}:{segment.StartValue})");
                for (int i = 1; i < count; i++)
                {
                    segment = _segments[i];
                    sb.Append($", ({segment.StartTime:0.0}:{segment.StartValue})");
                }
                sb.Append($", ({segment.EndTime:0.0}:{segment.EndValue})]");
            });
        }
    }
}