using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public class EasingSpeedProfile : SpeedProfile
    {
        [SerializeField] EaseType easeType = EaseType.Linear;
        Easer _easer = null;
        private Easer easer
        {
            get
            {
                if (_easer == null)
                {
                    _easer = Easers.GetEaser(easeType);
                }
                return _easer;
            }
        }

        private void OnValidate()
        {
            _easer = null;
        }

        public override float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime)
        {
            
            // return elapsedTimeSec / moveTime;
            
            float t = elapsedTimeSec / desiredMoveTime;
            t = Mathf.Clamp01(t);
            return easer(t);
        }
    }
}