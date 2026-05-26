using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseTouchBoard<TBoard, TCell> : BaseBoard<TBoard, TCell>, ITouchHandler
        where TBoard : IBoard<TBoard, TCell>
        where TCell : ICell<TBoard, TCell>
    {
        protected TouchController _touchController = new();

        public bool Interactable
        {
            get => _touchController.Interactable;
            set => _touchController.Interactable = value;
        }

        protected ITouchHandler _touchHandler;
        public ITouchHandler TouchHandler
        {
            get => _touchHandler;
            set
            {
                _touchHandler = value;
                _touchController.Handler = value ?? this;
            }
        }

        //public GameObject RaycastObject
        //{
        //    get => _touchController.RaycastObject;
        //    set => _touchController.RaycastObject = value;
        //}

        protected virtual void Awake()
        {
            _touchController.Handler = this;
        }

        public virtual bool OnTouchPressed(Vector3 pos)
        {
            return false;
        }

        public virtual bool OnTouchMoved(Vector3 pos)
        {
            return false;
        }

        public virtual void OnTouchReleased(Vector3 pos)
        {

        }

        /// <summary>
        /// Update touch controller.
        /// </summary>
        protected virtual void Update()
        {
            _touchController.Update();
        }

        protected virtual void OnApplicationPause(bool pause)
        {
            _touchController?.OnApplicationPaused();
        }
    }
}