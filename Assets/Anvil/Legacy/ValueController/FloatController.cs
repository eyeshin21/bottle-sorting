using UnityEngine;
using System;

namespace Anvil
{
    public class FloatController : ValueController<float>
    {
        public static LerpFloatController CreateLerp(float start, float end, float duration)
        {
            var controller = new LerpFloatController();
            controller.Construct(start, end, duration);
            return controller;
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, EaseType easeType)
        {
            var controller = new LerpFloatController();
            controller.Construct(start, end, duration);
            controller.Evaluator = Evaluator.Create(easeType);
            return controller;
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, Easer easer)
        {
            var controller = new LerpFloatController();
            controller.Construct(start, end, duration);
            if (easer != null)
            {
                controller.Evaluator = Evaluator.Create(easer);
            }
            return controller;
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, AnimationCurve curve)
        {
            var controller = new LerpFloatController();
            controller.Construct(start, end, duration);
            controller.Evaluator = Evaluator.Create(curve);
            return controller;
        }

        public static LerpFloatController CreateLerp(float start, float end, float duration, AnimationCurve curve, AnimationCurve timeCurve)
        {
            var controller = new LerpFloatController();
            controller.Construct(start, end, duration);
            controller.Evaluator = Evaluator.Create(curve, timeCurve);
            return controller;
        }

    }
}