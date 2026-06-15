using System;
using UnityEngine;

namespace Anvil
{
    public class SciptedFx : IScriptedFX
    {
        public Action<float> updateCallback;
        public Action<Action> placeHolderAction = null;

        public SciptedFx()
        {
        }

        public SciptedFx(Action<Action> temporaryAciton)
        {
            placeHolderAction = temporaryAciton;
        }
        public void Update()
        {
            updateCallback?.Invoke(Time.deltaTime);
        }

        public IScriptedFX Construct(IScriptedFxParam param)
        {
            return this;
        }

        public virtual GameObject gameObject=>null;

        public virtual void Play(Action callback)
        {
            if (placeHolderAction != null)
            {
                placeHolderAction(callback);
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
