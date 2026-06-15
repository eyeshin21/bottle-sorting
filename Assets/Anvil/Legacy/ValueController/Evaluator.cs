using UnityEngine;

namespace Anvil
{
    public class Evaluator
    {
        public static readonly Evaluator Default = new Evaluator();

        /// <summary>
        /// Gets value at the specified t (t in [0, 1])
        /// </summary>
        public virtual float Evaluate(float t)
        {
            return t;
        }

        public static EasingEvaluator Create(EaseType easeType, bool reverse = false)
        {
            return new EasingEvaluator(reverse ? easeType.GetReverse() : easeType);
        }

        public static EasingEvaluator Create(Easer easer)
        {
            return new EasingEvaluator(easer);
        }

        public static AnimationCurveEvaluator Create(AnimationCurve curve)
        {
            return new AnimationCurveEvaluator(curve);
        }

        public static AnimationCurve2Evaluator Create(AnimationCurve curve, AnimationCurve timeCurve)
        {
            return new AnimationCurve2Evaluator(curve, timeCurve);
        }
    }

    public class LerpEvaluator : Evaluator
    {
        private float _start;
        private float _end = 1;

        public void Construct(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public override float Evaluate(float t)
        {
            return _start + t * (_end - _start);
        }
    }

    public class SmoothStepEvaluator : Evaluator
    {
        private float _start;
        private float _end = 1;

        public void Construct(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public override float Evaluate(float t)
        {
            return Mathf.SmoothStep(_start, _end, t);
        }
    }

    public class AnimationCurveEvaluator : Evaluator
    {
        private AnimationCurve _curve;

        public AnimationCurveEvaluator(AnimationCurve curve)
        {
            Construct(curve);
        }

        public void Construct(AnimationCurve curve)
        {
            _curve = curve;
        }

        public override float Evaluate(float t)
        {
            return _curve.Evaluate(t);
        }
    }

    public class AnimationCurve2Evaluator : Evaluator
    {
        private AnimationCurve _curve;
        private AnimationCurve _timeCurve;

        public AnimationCurve2Evaluator(AnimationCurve curve, AnimationCurve timeCurve)
        {
            Construct(curve, timeCurve);
        }

        public void Construct(AnimationCurve curve, AnimationCurve timeCurve)
        {
            _curve = curve;
            _timeCurve = timeCurve;
        }

        public override float Evaluate(float t)
        {
            return _curve.Evaluate(_timeCurve.Evaluate(t));
        }
    }

    public class EasingEvaluator : Evaluator
    {
        private Easer _easer;

        public EasingEvaluator()
        {
            Construct(EaseType.Linear);
        }

        public EasingEvaluator(EaseType easeType)
        {
            Construct(easeType);
        }

        public EasingEvaluator(Easer easer)
        {
            _easer = easer;
        }

        public void Construct(EaseType easeType)
        {
            _easer = Easers.GetEaser(easeType);
        }

        public void Construct(Easer easer)
        {
            _easer = easer;
        }

        public override float Evaluate(float t)
        {
            return _easer(t);
        }
    }
}