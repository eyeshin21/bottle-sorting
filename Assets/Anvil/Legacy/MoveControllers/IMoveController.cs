using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public interface IMoveController
    {
        Transform transform { get; set; }
        bool Local { get; set; }
        Vector3 Current { get; set; }
        Vector3 End { get; set; }
        bool Finished { get; }

        void MoveTo(Vector3 endPos, Action callback = null);
        void Stop(bool forceEnd = false);
        void ForceFinish();
        void Update(float deltaTime);

        void UpdateConfig(MoveConfig moveConfig);
        void ReturnPool();
    }
}