using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationEventController : MonoBehaviour
    {
        //public void OnStartShow()
        //{
        //   LegacyLog.Debug($"[{name}][AnimationEvent]: OnStartShow");
        //    gameObject.SetShow(true);
        //}

        //public void OnEndHide()
        //{
        //   LegacyLog.Debug($"[{name}][AnimationEvent]: OnEndHide");
        //    gameObject.SetShow(false);
        //}

        public void SetShow(int show)
        {
            //Log.Debug($"[{name}][AnimationEvent]: SetShow({show})");
            gameObject.SetShow(show > 0);
        }
    }
}