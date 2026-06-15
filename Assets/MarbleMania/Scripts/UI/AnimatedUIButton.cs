using Anvil;

namespace Anvil
{
    public class AnimatedUIButton : UIButton, IAnimatedUIButton
    {
        private Anvil.IAnimationController _animationController;

        protected override void Awake()
        {
            base.Awake();
            _animationController = gameObject.CreateAnimationController();
        }

        protected override bool OnClick()
        {
            PlayAnimation(AnimationNames.Press);
            return base.OnClick();
        }

        public void PlayAnimation(string AnimationNames)
        {
            _animationController.PlayAnimation(AnimationNames);
        }
    }
}