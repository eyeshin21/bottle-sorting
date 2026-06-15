using UnityEngine;

namespace Anvil
{
    public delegate float Easer(float t);

    public enum EaseType
    {
        Linear,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubeIn,
        CubeOut,
        CubeInOut,
        BackIn,
        BackOut,
        BackInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        SineIn,
        SineOut,
        SineInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BounceIn,
        BounceOut,
        BounceInOut
    }

    public static class Easers
    {
        static float BounceTime(float time)
        {
            if (time < 0.36363636363636f)
            {
                return 7.5625f * time * time;
            }

            if (time < 0.72727272727272f)
            {
                time -= 0.54545454545454f;
                return 7.5625f * time * time + 0.75f;
            }

            if (time < 0.90909090909091f)
            {
                time -= 0.81818181818181f;
                return 7.5625f * time * time + 0.9375f;
            }

            time -= 0.95454545454545f;
            return 7.5625f * time * time + 0.984375f;
        }

        public static readonly Easer Linear = (t) => { return t; };
        public static readonly Easer QuadIn = (t) => { return t * t; };
        public static readonly Easer QuadOut = (t) => { return 1 - QuadIn(1 - t); };
        public static readonly Easer QuadInOut = (t) => { return (t <= 0.5f) ? QuadIn(t * 2) / 2 : QuadOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer CubeIn = (t) => { return t * t * t; };
        public static readonly Easer CubeOut = (t) => { return 1 - CubeIn(1 - t); };
        public static readonly Easer CubeInOut = (t) => { return (t <= 0.5f) ? CubeIn(t * 2) / 2 : CubeOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer BackIn = (t) => { return t * t * (2.70158f * t - 1.70158f); };
        public static readonly Easer BackOut = (t) => { return 1 - BackIn(1 - t); };
        public static readonly Easer BackInOut = (t) => { return (t <= 0.5f) ? BackIn(t * 2) / 2 : BackOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer ExpoIn = (t) => { return Mathf.Pow(2.0f, 10 * (t - 1)); };
        public static readonly Easer ExpoOut = (t) => { return 1.0f - Mathf.Pow(2.0f, -10 * t); };
        public static readonly Easer ExpoInOut = (t) =>
        {
            if (t > 0.0f && t < 1.0f)
            {
                t *= 2.0f;

                if (t < 1.0f)
                {
                    return Mathf.Pow(2.0f, 10 * (t - 1.0f)) * 0.5f;
                }

                return 1.0f - Mathf.Pow(2.0f, 10 * (1.0f - t)) * 0.5f;
            }

            return t;
        };
        public static readonly Easer SineIn = (t) => { return -Mathf.Cos(Mathf.PI / 2 * t) + 1; };
        public static readonly Easer SineOut = (t) => { return Mathf.Sin(Mathf.PI / 2 * t); };
        public static readonly Easer SineInOut = (t) => { return -Mathf.Cos(Mathf.PI * t) / 2f + .5f; };
        public static readonly Easer ElasticIn = (t) => { return 1 - ElasticOut(1 - t); };
        public static readonly Easer ElasticOut = (t) => { return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.075f) * (2 * Mathf.PI) / 0.3f) + 1; };
        public static readonly Easer ElasticInOut = (t) => { return (t <= 0.5f) ? ElasticIn(t * 2) / 2 : ElasticOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer BounceIn = (t) => { return 1 - BounceTime(1 - t); };
        public static readonly Easer BounceOut = (t) => { return BounceTime(t); };
        public static readonly Easer BounceInOut = (t) => { return (t < 0.5f) ? (1 - BounceTime(1 - t * 2)) * 0.5f : BounceTime(t * 2 - 1) * 0.5f + 0.5f; };

        public static Easer GetEaser(EaseType easeType)
        {
            switch (easeType)
            {
                case EaseType.SineIn: return SineIn;
                case EaseType.SineOut: return SineOut;
                case EaseType.SineInOut: return SineInOut;

                case EaseType.QuadIn: return QuadIn;
                case EaseType.QuadOut: return QuadOut;
                case EaseType.QuadInOut: return QuadInOut;

                case EaseType.CubeIn: return CubeIn;
                case EaseType.CubeOut: return CubeOut;
                case EaseType.CubeInOut: return CubeInOut;

                case EaseType.BackIn: return BackIn;
                case EaseType.BackOut: return BackOut;
                case EaseType.BackInOut: return BackInOut;

                case EaseType.ExpoIn: return ExpoIn;
                case EaseType.ExpoOut: return ExpoOut;
                case EaseType.ExpoInOut: return ExpoInOut;

                case EaseType.ElasticIn: return ElasticIn;
                case EaseType.ElasticOut: return ElasticOut;
                case EaseType.ElasticInOut: return ElasticInOut;

                case EaseType.BounceIn: return BounceIn;
                case EaseType.BounceOut: return BounceOut;
                case EaseType.BounceInOut: return BounceInOut;
            }

            return Linear;
        }

        public static EaseType GetEaseType(Easer easer)
        {
            if (easer == SineIn) return EaseType.SineIn;
            if (easer == SineOut) return EaseType.SineOut;
            if (easer == SineInOut) return EaseType.SineInOut;

            if (easer == QuadIn) return EaseType.QuadIn;
            if (easer == QuadOut) return EaseType.QuadOut;
            if (easer == QuadInOut) return EaseType.QuadInOut;

            if (easer == CubeIn) return EaseType.CubeIn;
            if (easer == CubeOut) return EaseType.CubeOut;
            if (easer == CubeInOut) return EaseType.CubeInOut;

            if (easer == BackIn) return EaseType.BackIn;
            if (easer == BackOut) return EaseType.BackOut;
            if (easer == BackInOut) return EaseType.BackInOut;

            if (easer == ExpoIn) return EaseType.ExpoIn;
            if (easer == ExpoOut) return EaseType.ExpoOut;
            if (easer == ExpoInOut) return EaseType.ExpoInOut;

            if (easer == ElasticIn) return EaseType.ElasticIn;
            if (easer == ElasticOut) return EaseType.ElasticOut;
            if (easer == ElasticInOut) return EaseType.ElasticInOut;

            if (easer == BounceIn) return EaseType.BounceIn;
            if (easer == BounceOut) return EaseType.BounceOut;
            if (easer == BounceInOut) return EaseType.BounceInOut;

            return EaseType.Linear;
        }

        public static EaseType GetReverse(this EaseType easeType)
        {
            if (easeType == EaseType.SineIn) return EaseType.SineOut;
            if (easeType == EaseType.SineOut) return EaseType.SineIn;
            if (easeType == EaseType.SineInOut) return easeType;

            if (easeType == EaseType.QuadIn) return EaseType.QuadOut;
            if (easeType == EaseType.QuadOut) return EaseType.QuadIn;
            if (easeType == EaseType.QuadInOut) return easeType;

            if (easeType == EaseType.CubeIn) return EaseType.CubeOut;
            if (easeType == EaseType.CubeOut) return EaseType.CubeIn;
            if (easeType == EaseType.CubeInOut) return easeType;

            if (easeType == EaseType.BackIn) return EaseType.BackOut;
            if (easeType == EaseType.BackOut) return EaseType.BackIn;
            if (easeType == EaseType.BackInOut) return easeType;

            if (easeType == EaseType.ExpoIn) return EaseType.ExpoOut;
            if (easeType == EaseType.ExpoOut) return EaseType.ExpoIn;
            if (easeType == EaseType.ExpoInOut) return easeType;

            if (easeType == EaseType.ElasticIn) return EaseType.ElasticOut;
            if (easeType == EaseType.ElasticOut) return EaseType.ElasticIn;
            if (easeType == EaseType.ElasticInOut) return easeType;

            if (easeType == EaseType.BounceIn) return EaseType.BounceOut;
            if (easeType == EaseType.BounceOut) return EaseType.BounceIn;
            if (easeType == EaseType.BounceInOut) return easeType;

            return EaseType.Linear;
        }
    }
}