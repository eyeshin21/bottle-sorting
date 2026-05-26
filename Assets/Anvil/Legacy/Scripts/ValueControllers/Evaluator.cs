using UnityEngine;

namespace Anvil.Legacy
{
    public class Evaluator
    {
        public static readonly Evaluator Default = new Evaluator();

        /// <summary>
        /// Evaluates value at the specified t (t within [0, 1])
        /// </summary>
        public virtual float Evaluate(float t)
        {
            return t;
        }

        #region Create
        public static LerpEvaluator CreateLerp(float start, float end)
        {
            return new LerpEvaluator(start, end);
        }

        public static SmoothStepEvaluator CreateSmoothStep()
        {
            return new SmoothStepEvaluator();
        }

        public static SmoothStepEvaluator CreateSmoothStep(float start, float end)
        {
            return new SmoothStepEvaluator(start, end);
        }

        public static EasingEvaluator Create(EaseType easeType)
        {
            return new EasingEvaluator(easeType);
        }

        public static EasingEvaluator Create(Easer easer)
        {
            return easer != null ? new EasingEvaluator(easer) : default;
        }

        public static DiagramEvaluator Create(FloatDiagram diagram)
        {
            return new DiagramEvaluator(diagram);
        }

        public static AnimationCurveEvaluator Create(AnimationCurve curve)
        {
            return new AnimationCurveEvaluator(curve);
        }

        //public static AnimationCurve2Evaluator Create(AnimationCurve curve, AnimationCurve timeCurve)
        //{
        //    return new AnimationCurve2Evaluator(curve, timeCurve);
        //}
        #endregion
    }

    public class LerpEvaluator : Evaluator
    {
        float _start;
        float _end = 1;

        public LerpEvaluator()
        {

        }

        public LerpEvaluator(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public void Construct(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public override float Evaluate(float t)
        {
            //return t <= 0 ? _start : (t < 1 ? _start + t * (_end - _start) : _end);
            return _start + t * (_end - _start);
        }
    }

    public class SmoothStepEvaluator : Evaluator
    {
        float _start;
        float _end = 1;

        public SmoothStepEvaluator()
        {

        }

        public SmoothStepEvaluator(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public void Construct(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public override float Evaluate(float t)
        {
            // This function interpolates between min and max in a similar way to Lerp.
            // However, the interpolation will gradually speed up from the start and slow down toward the end.
            // This is useful for creating natural-looking animation, fading and other transitions.
            return Mathf.SmoothStep(_start, _end, t);
        }
    }

    public class EasingEvaluator : Evaluator
    {
        Easer _easer;

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

    public class DiagramEvaluator : Evaluator
    {
        FloatDiagram _diagram;

        public DiagramEvaluator(FloatDiagram diagram)
        {
            _diagram = diagram;
        }

        public void Construct(FloatDiagram diagram)
        {
            _diagram = diagram;
        }

        public override float Evaluate(float t)
        {
            return _diagram.GetValue(t);
        }
    }

    public class AnimationCurveEvaluator : Evaluator
    {
        AnimationCurve _curve;

        public AnimationCurveEvaluator(AnimationCurve curve)
        {
            _curve = curve;
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

    //public class AnimationCurve2Evaluator : Evaluator
    //{
    //    AnimationCurve _curve;
    //    AnimationCurve _timeCurve;

    //    public AnimationCurve2Evaluator(AnimationCurve curve, AnimationCurve timeCurve)
    //    {
    //        _curve = curve;
    //        _timeCurve = timeCurve;
    //    }

    //    public void Construct(AnimationCurve curve, AnimationCurve timeCurve)
    //    {
    //        _curve = curve;
    //        _timeCurve = timeCurve;
    //    }

    //    public override float Evaluate(float t)
    //    {
    //        return _curve.Evaluate(_timeCurve.Evaluate(t));
    //    }
    //}
}