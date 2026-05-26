using UnityEngine;

namespace Anvil.Legacy
{
    public class FloatController : ValueController<float>
    {
        #region Create
        public static LerpFloatController CreateLerp(float start, float end, float duration)
        {
            return new LerpFloatController(start, end, duration);
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, EaseType easeType)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(easeType));
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, Easer easer)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(easer));
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, FloatDiagram timeDiagram)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(timeDiagram));
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, AnimationCurve timeCurve)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(timeCurve));
        }

        public static DiagramFloatController CreateDiagram(FloatDiagram diagram, float duration = -1)
        {
            return new DiagramFloatController(diagram, duration);
        }

        public static AnimationCurveFloatController CreateAnimationCurve(AnimationCurve curve, float duration = -1)
        {
            return new AnimationCurveFloatController(curve, duration);
        }

        public static AnimationCurveFloatController CreateAnimationCurve(AnimationCurveTime curveTime)
        {
            return CreateAnimationCurve(curveTime.Curve, curveTime.Time);
        }

        public static FloatControllerConverter CreateConverter(FloatController controller, Func<float, float> converter)
        {
            return new FloatControllerConverter(controller, converter);
        }
        #endregion
    }
}