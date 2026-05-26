using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static AnimationCurve NewAnimationCurve() => new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        public static AnimationCurveTime NewAnimationCurveTime() => new AnimationCurveTime(NewAnimationCurve(), 1);

        public static bool[] CreateTrues(int count)
        {
            var a = new bool[count];
            for (int i = 0; i < count; i++)
            {
                a[i] = true;
            }
            return a;
        }

        public static int[] CreateInts(int start, int end, bool random = false)
        {
            if (start > end)
            {
                return CreateInts(end, start);
            }

            int count = end - start + 1;
            var values = new int[count];
            int value = start;
            for (int i = 0; i < count; i++, value++)
            {
                values[i] = value;
            }
            if (random)
            {
                values.Swap();
            }
            return values;
        }

        public static int[] CreateRandomInts(int start, int end)
        {
            var values = CreateInts(start, end);
            values.Swap();
            return values;
        }

        public static Sprite[] CreateSprites<T>() where T : struct
        {
            return new Sprite[GetEnumCount<T>()];
        }

        public static TArray[] CreateArray<TArray, TType>() where TType : struct
        {
            return new TArray[GetEnumCount<TType>()];
        }
    }
}