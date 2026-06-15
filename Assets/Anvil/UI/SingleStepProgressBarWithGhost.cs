using System;
using UnityEngine;

namespace Anvil
{
    public class SingleStepProgressBarWithGhost : SingleStepProgressBar
    {
        [SerializeField] private RectTransform _ghostFillBar;
        [SerializeField] private float _ghostDelay = 0.5f;
        [SerializeField] private float _ghostDuration = 2f;

        private float _ghostTimer = 0f;
        private float _ghostTime = 0f;
        private float _targetGhostFill = 0f;
        private float _currentGhostFill = 0f;

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (_ghostFillBar == null) return;
            if (Mathf.Approximately(_currentGhostFill, _targetGhostFill))
            {
                
            }
            if (_ghostTimer > 0f)
            {
                _ghostTimer -= Time.deltaTime;
                if (_ghostTimer <= 0f)
                {
                    _ghostTimer = 0f;
                }

                return;
            }
            _ghostTime += Time.deltaTime;
            float t = Mathf.Clamp01(_ghostTime / _ghostDuration);
            _easer.Invoke(t);
            float fillAmount = Mathf.Lerp(_currentGhostFill, _targetGhostFill, t);
            fillAmount /= _maxStep;
            UpdateView(fillAmount, _ghostFillBar);
        }

        public override void SetProgress(float progress, Action callback = null)
        {
            base.SetProgress(progress, callback);
            _targetGhostFill = progress;
            _currentGhostFill = _currentStep;
            _ghostTimer = _ghostDelay;
            _ghostTime = 0f;
        }

        public override void SetProgressImidiate(float progress)
        {
            base.SetProgressImidiate(progress);
            _targetGhostFill = progress;
            _currentGhostFill = progress;
            UpdateView(_currentGhostFill/_maxStep, _ghostFillBar);
        }
    }
}