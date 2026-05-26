using System.Collections.Generic;
using UnityEngine;
using Anvil.Legacy;

namespace Anvil
{
    public class ParticleColorController : ColorController
    {
        public List<ParticleSystem> colorElement = new List<ParticleSystem>();
        public override void ChangeColor(Color color)
        {
            if (colorElement == null || colorElement.Count <= 0)
            {
                Debug.LogWarning($"NoColor Element to change");
                return;
            }
            foreach (var particleSystem in colorElement)
            {
                //ParticleSystem.MainModule main = particleSystem.main;
                //main.startColor = color;
                particleSystem.SetStartColor(color);
            }
        }

        public override void ChangeColor(Gradient gradient)
        {
            if (colorElement == null || colorElement.Count <= 0)
            {
                Debug.LogWarning($"NoColor Element to change");
                return;
            }
            foreach (var particleSystem in colorElement)
            {
                particleSystem.SetStartColor(gradient);
            }
        }

        public override void ChangeColor(Color minColor, Color maxColor)
        {
            if (colorElement == null || colorElement.Count <= 0)
            {
                Debug.LogWarning($"NoColor Element to change");
                return;
            }
            foreach (var particleSystem in colorElement)
            {
                particleSystem.SetStartColorGradient(minColor, maxColor);
            }
        }
    }
}
