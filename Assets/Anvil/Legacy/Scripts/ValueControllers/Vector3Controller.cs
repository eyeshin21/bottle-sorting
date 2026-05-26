using UnityEngine;

namespace Anvil.Legacy
{
    public class Vector3Controller : ValueController<Vector3>
    {
        #region Create
        public static LerpVector3Controller CreateLerp(Vector3 start, Vector3 end, float duration)
        {
            return new LerpVector3Controller(start, end, duration);
        }

        public static LerpVector3Controller CreateLerp(Vector3 start, Vector3 end, float duration, EaseType easeType)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(easeType));
        }

        public static LerpVector3Controller CreateLerp(Vector3 start, Vector3 end, float duration, Easer easer)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(easer));
        }

        public static LerpVector3Controller CreateLerp(Vector3 start, Vector3 end, float duration, FloatDiagram timeDiagram)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(timeDiagram));
        }

        public static LerpVector3Controller CreateLerp(Vector3 start, Vector3 end, float duration, AnimationCurve timeCurve)
        {
            return CreateLerp(start, end, duration).SetEvaluator(Evaluator.Create(timeCurve));
        }

        public static DiagramVector3Controller CreateDiagram(Vector3Diagram diagram, float duration = -1)
        {
            return new DiagramVector3Controller(diagram, duration);
        }

        public static QuadBezierVector3Controller CreateQuadBezier(Vector3 start, Vector3 control, Vector3 end, float duration)
        {
            return new QuadBezierVector3Controller(start, control, end, duration);
        }

        public static CubicBezierVector3Controller CreateCubicBezier(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float duration)
        {
            return new CubicBezierVector3Controller(start, control1, control2, end, duration);
        }

        public static AnimationCurveVector3Controller CreateAnimationCurve(Vector3 start, Vector3 end, AnimationCurve curve, float duration)
        {
            return new AnimationCurveVector3Controller(start, end, curve, duration);
        }
        #endregion
    }
}