using UnityEngine;

namespace Anvil
{
    public interface ITargetDesignator
    {
        public bool IsActive { get; set; }
        public bool Validate();
        public void SetTarget(GameObject targetObj);
        public void SetTarget(Vector3 position);

        public GameObject GetTargetObject();
        public Vector3 CalculateTargetPosition();
        public Vector3 GetTargetPosition();
    }
}
