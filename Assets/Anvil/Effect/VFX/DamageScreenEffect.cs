using System;
using Anvil;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class DamageScreenEffect : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _alphaA = 0;
        [SerializeField] private float _alphaB = 1;
        [SerializeField] private float _duration = 1f;

        // [SerializeField] private EaseType _easeType = EaseType.Linear;
        [SerializeField] private AnimationCurve _easeCurve;
        private float timer = 0f;
        // private bool forward = true;
        private Easer _easer;

        // private void Awake()
        // {
        //     _easer = Easers.GetEaser(_easeType);
        // }

        private void Update()
        {
            if (_duration <= 0f) return;

            timer += Time.deltaTime;
            if (timer >= _duration)
            {
                timer = _duration;
                timer = 0f;
                StopEffect();
                return;
            }

            float t = timer / _duration;
            t = _easeCurve.Evaluate(t);
            Color color = _image.color;
            _image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(_alphaA, _alphaB, t));
        }
        public void StartEffect(float alpha = 1)
        {
            _alphaB = alpha;
            timer = 0f;
            gameObject.SetActive(true);
        }
        public void StopEffect()
        {
            gameObject.SetActive(false);
        }
    }
}