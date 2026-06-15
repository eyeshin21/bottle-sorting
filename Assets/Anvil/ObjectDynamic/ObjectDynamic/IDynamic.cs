using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Anvil
{
    public interface IDynamic
    {
        public void OnUpdate(float deltaTime);

    }
    public interface IContinuousObjectDynamic : IDynamic
    {
        public Vector3 Velocity { get; }
        public void ApplyVelocity(Vector3 movement);
        public void ResetDynamics();
        public void FollowTrack(List<Vector3> positions, Action callback = null);
    }
    public interface IDiscreteObjectDynamic : IDynamic
    {
        public void ForceCompleteMovement();
        public void SetMoveTarget(Vector3 position);
        public void MoveTo(Vector3 position);
        public void MoveTo(Vector3 position, Action callback);
        public void MoveTo(List<Vector3> positions, Action callback);
    }
}