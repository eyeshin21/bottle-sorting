namespace Anvil
{
    public class ValueController<T>
    {
        protected T _value;
        protected bool _isFinished = true;

        public virtual T Value => _value;
        public virtual bool Finished => _isFinished;

        protected virtual T DefaultValue => default;
        public virtual float Duration => 0;

        protected void _Construct(T value)
        {
            _value = value;
            _isFinished = false;
        }

        public virtual bool GetEnd(out T value)
        {
            value = DefaultValue;
            return false;
        }

        public virtual void SetEnd(T end)
        {

        }

        public virtual void Stop()
        {
            _isFinished = true;
        }

        public virtual void Reset()
        {
            _isFinished = false;
        }

        public virtual T Update(float deltaTime)
        {
            if (!_isFinished)
            {
                OnUpdate(deltaTime);
            }

            return _value;
        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }
    }
}