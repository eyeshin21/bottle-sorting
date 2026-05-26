using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil
{
    public interface IUIAnimationController
    {
        public void PlayAnimation(string name, Action callback);
        public void PlayAnimation(string name);
        public void PlayShowAnimation(Action callback = null);
        public void PlayHideAnimation(Action callback = null);
    }
}
