using System;
using Anvil.Legacy;
using Drawing;
using UnityEngine;

namespace MarbleMania.Scripts.Game
{
    public class CameraController : MonoBehaviour, ITouchHandler
    {
        private static CameraController _instance;
        public static CameraController Instance => _instance;
        private Camera _camera;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            _touchController = new TouchController();
            _touchController.Handler = this;
            _camera = GetComponent<Camera>();
        }
        TouchController _touchController;
        private void Update()
        {
            _touchController.Update();   
        }
        [SerializeField] LayerMask _raycastLayerMask;
        public bool OnTouchPressed(Vector2 pos)
        {
            Ray ray = _camera.ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _raycastLayerMask, QueryTriggerInteraction.Collide))
            {
                GameObject hitObject = hit.collider.gameObject;
                Crate  crate = hitObject.GetComponent<Crate>();
                if (crate == null) return false;
                crate.OnSelect();
            }
            return false;
        }

        public bool OnTouchMoved(Vector2 pos)
        {
            return false;
        }

        public void OnTouchReleased(Vector2 pos)
        {
        }
    }
}