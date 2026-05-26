using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class Vector3Diagram : Diagram<Vector3>
    {
        public class Vector3Segment : Segment<Vector3>
        {
            public Vector3Segment(float startTime, Vector3 startValue, float endTime, Vector3 endValue)
            {
                Construct(startTime, startValue, endTime, endValue);
            }

            protected override Vector3 GetValueAt(float t)
            {
                return _startValue + (_endValue - _startValue) * t;
            }
        }

        public Vector3Diagram(float startTime, Vector3 startValue, float endTime, Vector3 endValue)
        {
            Construct(startTime, startValue, endTime, endValue);
        }

        public Vector3Diagram(params (float, Vector3)[] timeValues)
        {
            Construct(timeValues);
        }

        public Vector3Diagram(List<(float, Vector3)> timeValues)
        {
            Construct(timeValues);
        }

        void Construct((float, Vector3)[] timeValues)
        {
            int count = timeValues.Length;
            Assert.IsTrue(count >= 2 && count.IsEven());
            var p1 = timeValues[0];
            var p2 = timeValues[1];
            Construct(p1.Item1, p1.Item2, p2.Item1, p2.Item2);

            for (int i = 2; i < count; i++)
            {
                var p = timeValues[i];
                Add(p.Item1, p.Item2);
            }
        }

        void Construct(List<(float, Vector3)> timeValues)
        {
            int count = timeValues.Count;
            Assert.IsTrue(count >= 2 && count.IsEven());
            var p1 = timeValues[0];
            var p2 = timeValues[1];
            Construct(p1.Item1, p1.Item2, p2.Item1, p2.Item2);

            for (int i = 2; i < count; i++)
            {
                var p = timeValues[i];
                Add(p.Item1, p.Item2);
            }
        }

        protected override Segment<Vector3> CreateSegment(float startTime, Vector3 startValue, float endTime, Vector3 endValue)
        {
            return new Vector3Segment(startTime, startValue, endTime, endValue);
        }
    }
}