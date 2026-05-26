using System.Collections.Generic;
using UnityEngine;
//using Spine.Unity;

namespace Anvil.Legacy
{
    public interface IMaskAdapter
    {
        GameObject GameObject { get; }
        SpriteMaskInteraction MaskInteraction { get; set; }

        /// <summary>
        /// Set VisibleInsideMask if enabled.
        /// </summary>
        void SetMaskEnabled(bool enabled);
    }

    public abstract class MaskAdapter : IMaskAdapter
    {
        public abstract GameObject GameObject { get; }
        public abstract SpriteMaskInteraction MaskInteraction { get; set; }

        /// <summary>
        /// Set VisibleInsideMask if enabled.
        /// </summary>
        public virtual void SetMaskEnabled(bool enabled)
        {
            if (enabled)
            {
                MaskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            else
            {
                MaskInteraction = SpriteMaskInteraction.None;
            }
        }

        public static MaskAdapter Create(SpriteRenderer spriteRenderer)
        {
            return new SpriteRendererMaskAdapter(spriteRenderer);
        }

        //public static MaskAdapter Create(SkeletonAnimation skeletonAnimation)
        //{
        //    return new SkeletonAnimationMaskAdapter(skeletonAnimation);
        //}

        public static MaskAdapter Create(ParticleSystemRenderer particleSystemRenderer)
        {
            return new ParticleSystemRendererMaskAdapter(particleSystemRenderer);
        }

        public static IMaskAdapter Create(Component component)
        {
            return Create(component.gameObject);
        }

        static List<IMaskAdapter> _maskAdapters = new List<IMaskAdapter>();
        public static IMaskAdapter Create(GameObject gameObject)
        {
            _maskAdapters.Clear();
            gameObject.BrowseChildrenBFS(go =>
            {
                var maskAdapter = CreateMaskAdapter(go, out bool continueChildren);
                if (maskAdapter != null)
                {
                    _maskAdapters.Add(maskAdapter);
                }
                return continueChildren;
            });

            int count = _maskAdapters.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create mask adapter for {gameObject}!");
                return new DefaultMaskAdapter(gameObject);
            }

            if (count == 1)
            {
                var maskAdapter = _maskAdapters[0];
                _maskAdapters.Clear();
                return maskAdapter;
            }

            var maskAdapter2 = new CompositeMaskAdapter(_maskAdapters);
            _maskAdapters.Clear();
            return maskAdapter2;
        }

        static IMaskAdapter CreateMaskAdapter(GameObject gameObject, out bool continueChildren)
        {
            continueChildren = true;

            var maskAdapter = gameObject.GetComponent<IMaskAdapter>();
            if (maskAdapter != null)
            {
                continueChildren = false;
                return maskAdapter;
            }

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                return Create(spriteRenderer);
            }

            //var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
            //if (skeletonAnimation != null)
            //{
            //    return Create(skeletonAnimation);
            //}

            var particleSystemRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
            if (particleSystemRenderer != null)
            {
                return Create(particleSystemRenderer);
            }

            return null;
        }

        class SpriteRendererMaskAdapter : MaskAdapter
        {
            private SpriteRenderer _spriteRenderer;

            public SpriteRendererMaskAdapter(SpriteRenderer spriteRenderer)
            {
                _spriteRenderer = spriteRenderer;
            }

            public override GameObject GameObject => _spriteRenderer.gameObject;

            public override SpriteMaskInteraction MaskInteraction
            {
                get => _spriteRenderer.maskInteraction;
                set => _spriteRenderer.maskInteraction = value;
            }
        }

        //class SkeletonAnimationMaskAdapter : MaskAdapter
        //{
        //    private SkeletonAnimation _skeletonAnimation;

        //    public SkeletonAnimationMaskAdapter(SkeletonAnimation skeletonAnimation)
        //    {
        //        _skeletonAnimation = skeletonAnimation;
        //    }

        //    public override GameObject GameObject => _skeletonAnimation.gameObject;

        //    public override SpriteMaskInteraction MaskInteraction
        //    {
        //        get => _skeletonAnimation.maskInteraction;
        //        set => _skeletonAnimation.maskInteraction = value;
        //    }
        //}

        class ParticleSystemRendererMaskAdapter : MaskAdapter
        {
            private ParticleSystemRenderer _particleSystemRenderer;

            public ParticleSystemRendererMaskAdapter(ParticleSystemRenderer particleSystemRenderer)
            {
                _particleSystemRenderer = particleSystemRenderer;
            }

            public override GameObject GameObject => _particleSystemRenderer.gameObject;

            public override SpriteMaskInteraction MaskInteraction
            {
                get => _particleSystemRenderer.maskInteraction;
                set => _particleSystemRenderer.maskInteraction = value;
            }
        }

        class DefaultMaskAdapter : MaskAdapter
        {
            private GameObject _gameObject;
            private SpriteMaskInteraction _maskInteraction;

            public DefaultMaskAdapter(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public override GameObject GameObject => _gameObject;

            public override SpriteMaskInteraction MaskInteraction
            {
                get => _maskInteraction;
                set => _maskInteraction = value;
            }
        }

        class CompositeMaskAdapter : MaskAdapter
        {
            private List<IMaskAdapter> _adapters;
            private int _count;
            private SpriteMaskInteraction _maskInteraction;

            public CompositeMaskAdapter(List<IMaskAdapter> adapters)
            {
                _count = adapters.Count;
                _adapters = new List<IMaskAdapter>(_count);
                _adapters.AddRange(adapters);
            }

            public override GameObject GameObject => _count > 0 ? _adapters[0].GameObject : null;

            public override SpriteMaskInteraction MaskInteraction
            {
                get => _count > 0 ? _adapters[0].MaskInteraction : _maskInteraction;
                set
                {
                    for (int i = 0; i < _count; i++)
                    {
                        _adapters[i].MaskInteraction = value;
                    }
                    _maskInteraction = value;
                }
            }
        }
    }
}