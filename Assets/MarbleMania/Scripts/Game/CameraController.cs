using Anvil.Legacy;
using UnityEngine;

namespace MarbleMania
{
    public class CameraController : MonoBehaviour, ITouchHandler
    {
        private static CameraController _instance;
        public static CameraController Instance => _instance;
        
        
        [SerializeField] private bool _zoomEnabled = true;
        private Camera _camera;
        public static Camera Camera => Instance._camera;
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
            if (!_zoomEnabled) return;
            _touchController.Update();
            var scrollDelta = Input.mouseScrollDelta;
            if (scrollDelta.y == 0) return;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scrollDelta.y * 0.15f, 2, 20);
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