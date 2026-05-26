using UnityEngine;

namespace Anvil.Legacy
{
    public class InputNumber
    {
        static readonly float DelayTime = 0.25f;

        Callback<int> _callback;
        int _digitCapacity;
        int[] _digits;
        int _count;
        float _delayTime;

        public InputNumber(Callback<int> callback, int digitCapacity = 3)
        {
            _callback = callback;
            _digitCapacity = digitCapacity;
            _digits = new int[digitCapacity];
            _count = 0;
            _delayTime = 0;
        }

        void DoCallback()
        {
            if (_count > 0)
            {
                int number = _digits[0];
                for (int i = 1; i < _count; i++)
                {
                    number = number * 10 + _digits[i];
                }
                _count = 0;
                _callback(number);
            }
            _delayTime = 0;
        }

        void Push(int digit)
        {
            if (_count == 1 && _digits[0] == 0)
            {
                _count = 0;
            }

            _digits[_count++] = digit;

            if (_count == _digitCapacity)
            {
                DoCallback();
            }
            else
            {
                _delayTime = DelayTime;
            }
        }

        public bool Update()
        {
            if (_delayTime > 0)
            {
                _delayTime -= Time.deltaTime;
                if (_delayTime <= 0)
                {
                    DoCallback();
                }
            }

            if (Input.anyKeyDown)
            {
                for (int digit = 0; digit < 10; digit++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + digit))
                    {
                        Push(digit);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Update(Event evt)
        {
            if (_delayTime > 0)
            {
                _delayTime -= Time.deltaTime;
                if (_delayTime <= 0)
                {
                    DoCallback();
                }
            }

            if (evt.type == EventType.KeyDown)
            {
                var keyCode = evt.keyCode;
                if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
                {
                    Push(keyCode - KeyCode.Alpha0);
                    evt.Use();
                    return true;
                }
            }

            return false;
        }
    }
}