using System;
using UnityEngine;

namespace Anvil
{
    public interface IScriptedFX
    {

        public GameObject gameObject { get; }
        public void Play(Action callback);
    }

    public interface IScriptedFxParam
    {
        public IScriptedFX CreateFx();

    }
}
