using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Wait : ActionX
    {
        public override float Duration => -1;

        public void Construct()
        {
            _Construct();
        }

        protected override bool OnPlay()
        {
            return false;
        }

        protected override bool OnUpdate(float deltaTime)
        {
            return false;
        }

        public static Wait Create()
        {
            var action = new Wait();
            action.Construct();
            return action;
        }
    }
}