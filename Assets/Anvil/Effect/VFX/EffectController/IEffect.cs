using System;

namespace Anvil
{
    public interface IEffect
    {
        public void PlayEffect();
        public void PlayEffect(Action callback);
        
        public void StopEffect();
        public void StopEffect(Action callback);
    }
}