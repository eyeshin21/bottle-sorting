using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy.Actions
{
    public class Fade : ActionX
    {
        IColorController _colorController;
        FloatController _controller;

        public override float Duration => _controller.Duration;

        public void Construct(IColorController colorController, FloatController controller)
        {
            _colorController = colorController;
            _controller = controller;
            _Construct();
        }

        void SetAlpha(float alpha)
        {
            var color = _colorController.Color;
            color.a = alpha;
            _colorController.Color = color;
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override bool OnPlay()
        {
            SetAlpha(_controller.Value);
            return _controller.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out float end))
                {
                    SetAlpha(end);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            SetAlpha(_controller.Update(deltaTime));
            return _controller.Finished;
        }

        public static Fade Create(IColorController colorController, FloatController controller)
        {
            var action = new Fade();
            action.Construct(colorController, controller);
            return action;
        }

        public static Fade Create(SpriteRenderer spriteRenderer, FloatController controller)
        {
            return Create(SpriteRendererSpriteController.Create(spriteRenderer), controller);
        }

        public static Fade CreateLerp(IColorController colorController, float startAlpha, float endAlpha, float duration)
        {
            return Create(colorController, FloatController.CreateLerp(startAlpha, endAlpha, duration));
        }

        public static Fade CreateLerp(IColorController colorController, float startAlpha, float endAlpha, float duration, EaseType easeType)
        {
            return Create(colorController, FloatController.CreateLerp(startAlpha, endAlpha, duration, easeType));
        }

        public static Fade CreateLerp(IColorController colorController, float startAlpha, float endAlpha, float duration, AnimationCurve curve)
        {
            return Create(colorController, FloatController.CreateLerp(startAlpha, endAlpha, duration, curve));
        }

        public static Fade CreateAnimationCurve(IColorController colorController, AnimationCurve curve, float duration = -1)
        {
            return Create(colorController, FloatController.CreateAnimationCurve(curve, duration));
        }

        public static Fade CreateLerp(SpriteRenderer spriteRenderer, float startAlpha, float endAlpha, float duration)
        {
            return Create(SpriteRendererSpriteController.Create(spriteRenderer), FloatController.CreateLerp(startAlpha, endAlpha, duration));
        }

        //public static Fade Create(ParticleSystem particleSystem, FloatController controller)
        //{
        //    return Create(colorController.Create(particleSystem), controller);
        //}

        //public static Fade CreateLerp(ParticleSystem particleSystem, float startAlpha, float endAlpha, float duration)
        //{
        //    return Create(colorController.Create(particleSystem), FloatController.CreateLerp(startAlpha, endAlpha, duration));
        //}

        public static Fade Create(Image image, FloatController controller)
        {
            return Create(ImageSpriteController.Create(image), controller);
        }

        public static Fade CreateLerp(Image image, float startAlpha, float endAlpha, float duration)
        {
            return Create(ImageSpriteController.Create(image), FloatController.CreateLerp(startAlpha, endAlpha, duration));
        }

        public static Fade Create(GameObject gameObject, FloatController controller)
        {
            return Create(SpriteController.Create(gameObject) as IColorController, controller);
        }

        public static Fade CreateLerp(GameObject gameObject, float startAlpha, float endAlpha, float duration)
        {
            return Create(SpriteController.Create(gameObject) as IColorController, FloatController.CreateLerp(startAlpha, endAlpha, duration));
        }
    }
}