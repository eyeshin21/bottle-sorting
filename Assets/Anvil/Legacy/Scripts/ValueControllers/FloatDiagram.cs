using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class FloatDiagram : Diagram<float>
    {
        public class FloatSegment : Segment<float>
        {
            public FloatSegment(float startTime, float startValue, float endTime, float endValue)
            {
                Construct(startTime, startValue, endTime, endValue);
            }

            protected override float GetValueAt(float t)
            {
                return _startValue + (_endValue - _startValue) * t;
            }
        }

        public FloatDiagram(float startTime, float startValue, float endTime, float endValue)
        {
            Construct(startTime, startValue, endTime, endValue);
        }

        public FloatDiagram(params float[] timeValues)
        {
            Construct(timeValues);
        }

        public FloatDiagram(List<float> timeValues)
        {
            Construct(timeValues);
        }

        public FloatDiagram(string s)
        {
            var timeValues = Helper.GetFloats(s);
            Construct(timeValues);
            Pools.Return(timeValues);
        }

        void Construct(float[] timeValues)
        {
            int count = timeValues.Length;
            Assert.IsTrue(count >= 4 && count.IsEven());
            Construct(timeValues[0], timeValues[1], timeValues[2], timeValues[3]);

            for (int i = 4; i < count; i += 2)
            {
                Add(timeValues[i], timeValues[i + 1]);
            }
        }

        void Construct(List<float> timeValues)
        {
            int count = timeValues.Count;
            Assert.IsTrue(count >= 4 && count.IsEven());
            Construct(timeValues[0], timeValues[1], timeValues[2], timeValues[3]);

            for (int i = 4; i < count; i += 2)
            {
                Add(timeValues[i], timeValues[i + 1]);
            }
        }

        protected override Segment<float> CreateSegment(float startTime, float startValue, float endTime, float endValue)
        {
            return new FloatSegment(startTime, startValue, endTime, endValue);
        }

#if UNITY_EDITOR
        //[UnityEditor.MenuItem("Test/Float Diagram")]
        static void Test()
        {
            Helper.ClearLog();

            float time1 = 0;
            float time2 = 1;
            float time3 = 2;
            float time4 = 3;
            float value1 = 0.5f;
            float value2 = 1;
            float value3 = 1;
            float value4 = 0;

            var diagram = new FloatDiagram(time1, value1, time2, value2);
            diagram.Add(time3, value3);
            diagram.Add(time4, value4);
            Test(diagram);

            var diagram2 = new FloatDiagram(time1, value1, time2, value2, time3, value3, time4, value4);
            Assert.IsTrue(diagram2.Equals(diagram));

            var diagram3 = new FloatDiagram($"{time1},{value1},{time2},{value2},{time3},{value3},{time4},{value4}");
            Assert.IsTrue(diagram3.Equals(diagram));
        }

        static void Test(FloatDiagram diagram)
        {
            LegacyLog.Debug(diagram);
            float time = -1f;
            for (int i = 0; i < 10; i++)
            {
                time += 0.5f;
                LegacyLog.Debug($"{time:0.0}: {diagram.GetValue(time):0.00}");
            }
        }
#endif
    }
}