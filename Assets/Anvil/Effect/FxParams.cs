using System;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    [Serializable]
    public class FxParams
    {
        public float duration;
        [FormerlySerializedAs("delayBetwenSpawn")] public float delayInBetween;
        // public float delayBetwenMove;

        // public float
        [Range(0,1)] public float delayMinDecrement;
        
        public AnimationCurve delayCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public FxParams()
        {

        }
        public FxParams(float duration2, float delayBetwenSpawn2, float delayMinDecrement2)
        {
            duration = duration2;
            delayInBetween = delayBetwenSpawn2;
            // delayBetwenMove = delayBetwenMove2;
            delayMinDecrement = delayMinDecrement2;
        }

        public FxParams GetCopy()
        {
            FxParams ret = new FxParams();
            ret.SetParam(duration, delayInBetween, delayMinDecrement);
            return ret;
        }
        public void SetParam(float duration2, float delayBetwenSpawn2, float delayMinDecrement2)
        {
            duration = duration2;
            delayInBetween = delayBetwenSpawn2;
            // delayBetwenMove = delayBetwenMove2;
            delayMinDecrement = delayMinDecrement2;
        }
        public float EvaluateDelaySpawn(int index, int total)
        {
            //Linear interpolation
            float ret = delayInBetween + (delayInBetween * (delayMinDecrement - 1) * index / total);
            // Debug.Log($"eval {index}/{total}: {ret}");
            return ret;
        }
        public float EvaluateDelaySpawnInverse(int index, int total)
        {
            //Linear interpolation
            float ret = delayInBetween + (delayInBetween * (delayMinDecrement - 1) * (total - index - 1) / total);
            return ret;
        }
        public float EvaluateDelayCurve(int index, int total)
        {
            return delayCurve.Evaluate(index / (float)total) * delayInBetween;
        }
    }

}
