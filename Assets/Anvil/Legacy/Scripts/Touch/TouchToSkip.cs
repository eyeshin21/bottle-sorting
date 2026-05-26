using UnityEngine;
using UnityEngine.Events;

namespace Anvil.Legacy
{
    public class TouchToSkip : MonoBehaviour
    {
        [SerializeField] float _touchDelay = 1f;
        [SerializeField] float _skipDelay = 1f;
        [SerializeField] UnityEvent _onSkip;

        bool _isWaiting;
        bool _isSkip;
        float _time;

        void OnEnable()
        {
            _isWaiting = true;
            _isSkip = false;
            _time = 0;
        }

        void Update()
        {
            if (_isWaiting)
            {
                _time += Time.deltaTime;
                if (_isSkip)
                {
                    if (_time >= _skipDelay || Helper.HasInput())
                    {
                        _isWaiting = false;
                        _onSkip?.Invoke();
                    }
                }
                else
                {
                    if (_time >= _touchDelay)
                    {
                        _time -= _touchDelay;
                        _isSkip = true;
                    }
                }
            }

        }
    }
}