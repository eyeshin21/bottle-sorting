using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class EffectController : MonoBehaviour , IEffect
    {
        [SerializeField] protected GameObject _parentGameObject;
        public virtual void PlayEffect()
        {
        }

        public virtual void PlayEffect(Action callback)
        {
            callback?.Invoke();
        }

        public virtual void StopEffect()
        {
        }

        public virtual void StopEffect(Action callback)
        {
            callback?.Invoke();
        }

    }
}