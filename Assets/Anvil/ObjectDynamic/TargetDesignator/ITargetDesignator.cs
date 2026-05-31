using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITargetDesignator
    {
        // public bool IsActive { get; set; }
        // public void SetTarget(GameObject targetObj);
        // public void SetTarget(Vector3 position);

        public GameObject GetTargetObject();
        public Vector3 CalculateTargetPosition();
        public Vector3 GetTargetPosition();
    }
}
