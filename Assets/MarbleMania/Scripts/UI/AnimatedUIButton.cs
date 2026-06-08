using Anvil.Legacy;

namespace Anvil
{
    public class AnimatedUIButton : UIButton, IAnimatedUIButton
    {
        private Legacy.IAnimationController _animationController;

        protected override void Awake()
        {
            base.Awake();
            _animationController = gameObject.CreateAnimationController();
        }

        protected override bool OnClick()
        {
            PlayAnimation(AnimationNames.ButtonPress);
            return base.OnClick();
        }

        public void PlayAnimation(string AnimationNames)
        {
            _animationController.PlayAnimation(AnimationNames);
        }
    }
}