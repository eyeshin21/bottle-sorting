using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class CopyTransform : MonoBehaviour
    {

        [SerializeField] bool _copyPosition = true;
        
        //NOT IMPLEMENTED YET
        // [SerializeField] bool _copyRotation = true;
        // [SerializeField] bool _copyScale = true;

        private ITargetDesignator _targetDesignator;
        // private Transform _target;
        //
        // public Transform Target
        // {
        //     get { return _target; }
        //     set { _target = value; }
        // }

        public void CopyPosition(ITargetDesignator targetDesignator)
        {
            _targetDesignator = targetDesignator;
            _copyPosition = true;
            // _copyRotation = false;
            // _copyScale = false;
        }

        public void CopyRotation(ITargetDesignator targetDesignator)
        {
            _targetDesignator = targetDesignator;
            _copyPosition = false;
            // _copyRotation = true;
            // _copyScale = false;
        }

        public void CopyScale(ITargetDesignator targetDesignator)
        {
            _targetDesignator = targetDesignator;
            _copyPosition = false;
            // _copyRotation = false;
            // _copyScale = true;
        }

        private void Update()
        {
            if (_targetDesignator == null)
            {
                return;
            }

            if (_copyPosition)
            {
                transform.position = _targetDesignator.GetTargetPosition();
            }

            //NOT IMPLEMENTED YET
            // if (_copyRotation)
            // {
            //     transform.rotation = _target.rotation;
            // }
            //
            // if (_copyScale)
            // {
            //     transform.localScale = _target.localScale;
            // }
        }
    }
    public static partial class ExtensionMethod
    {
        public static CopyTransform CopyPosition(this GameObject self,Transform target)
        {
            if (self == null || target == null)
            {
                return null;
            }

            var constraint = self.GetOrAddComponent<CopyTransform>();
            constraint.CopyPosition(new StaticTargetReference(target.gameObject));
            return constraint;
        }
        public static CopyTransform CopyPosition(this GameObject self,ITargetDesignator targetDesignator)
        {
            if (self == null || targetDesignator == null)
            {
                Debug.Log("null");
                return null;
            }

            var constraint = self.GetOrAddComponent<CopyTransform>();
            constraint.CopyPosition(targetDesignator);
            return constraint;
        }
    }
}
