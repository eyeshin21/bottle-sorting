using System;
using Anvil;
using UnityEditor;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class AuxilaryPanel : DestroyableSingletonBehaviour<AuxilaryPanel>
    {
        [SerializeField] private GameObject _container;

        protected override void Awake()
        {
            base.Awake();
            LevelEditor.ReloadSignal += Reload;
        }

        private void Reload()
        {
            GameObjectPool.ClearManagedChild(_container);
        }

        public static void Load(IEditorProperty property)
        {
            Instance.Reload();
            property.CreatePropertyPanel(Instance._container.transform);
        }

        [SerializeField] private LayerMask _layerMask;
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var mousePos = Input.mousePosition;
                var ray = CameraController.Camera.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    var hitObject = hit.collider.gameObject;
                    EditorGUIUtility.PingObject(hitObject);
                    var editorProperty = hitObject.GetComponent<IEditorProperty>();
                    if (editorProperty == null) return;
                    Load(editorProperty);
                }
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
            }
        }
    }
}