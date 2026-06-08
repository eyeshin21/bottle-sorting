//using FarmJamUI;
//using Anvil;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Anvil
//{
//    public abstract class BaseUITransformController : IUITransformController
//    {

//        protected RectTransform _rectTransform;
//        protected List<Vector3> wayPoints = new List<Vector3>();
//        protected Vector3 _endPos;
//        protected Vector3 _originPos;

//        protected Vector3 _endSize;
//        protected Vector3 _originSize;

//        protected Action _callback;
//        protected bool _isMoveFinished = true;
//        protected bool _isSizeFinished = true;
//        protected bool _isFinished = true;
//        protected float moveTimeStamp = 0;
//        protected float sizeTimeStamp = 0;

//        public Vector3 CurrentPos
//        {
//            get => _rectTransform.anchoredPosition;
//            set => _rectTransform.anchoredPosition = value;
//        }
//        public Vector3 OriginPos
//        {
//            get => _originPos;
//        }
//        public Vector3 Distance
//        {
//            get => EndPos - OriginPos;
//        }
//        public Vector2 CurrentSize
//        {
//            get => _rectTransform.sizeDelta;
//            set => _rectTransform.sizeDelta = value;
//        }
//        public Vector2 OriginSize
//        {
//            get => _originSize;
//        }
//        public Vector2 EndSize
//        {
//            get => _endSize;
//        }

//        public Vector3 CurrentScale
//        {
//            get => _rectTransform.localScale;
//            set => _rectTransform.localScale = value;
//        }

//        public Vector3 EndPos
//        {
//            get => _endPos;
//            set
//            {
//                _endPos = value;
//                //_isFinished = false;
//            }
//        }

//        protected void _Construct(RectTransform rectTransform)
//        {
//            _rectTransform = rectTransform;
//            _endPos = CurrentPos;
//        }

//        bool IUITransformController.Finished => _isFinished;


//        void IUITransformController.MoveTo(Vector3 pos)
//        {

//            _endPos = pos; //set destination to first waypoint
//            _originPos = CurrentPos;
//            _isMoveFinished = false;
//            moveTimeStamp = 0;

//            _callback = null;
//        }
//        void IUITransformController.MoveTo(Vector3 pos, Action callback)
//        {

//            _endPos = pos; //set destination to first waypoint
//            _originPos = CurrentPos;
//            _isMoveFinished = false;
//            moveTimeStamp = 0;

//            _callback = callback;

//        }

//        void IUITransformController.ReSizeTo(Vector2 size)
//        {
//            _endSize = size;
//            _originSize = CurrentSize;
//            _isSizeFinished = false;
//            sizeTimeStamp = 0;
//        }

//        void IUITransformController.ForceFinish()
//        {
//            if (!_isMoveFinished || !_isSizeFinished)
//            {
//                CurrentPos = _endPos;
//                CurrentSize = _endSize;
//                OnFinish();
//            }
//        }

//        void IUITransformController.Update()
//        {
//            OnUpdate(Time.deltaTime);
//        }

//        void IUITransformController.Update(float deltaTime)
//        {
//            OnUpdate(deltaTime);
//        }

//        protected virtual void OnMoveFinish()
//        {
//            _isMoveFinished = true;
//            //if (_callback != null)
//            //{
//            //    var callback = _callback;
//            //    _callback = null;
//            //    callback();
//            //}
//        }
//        protected virtual void OnSizeFinish()
//        {
//            _isSizeFinished = true;
//            //if (_callback != null)
//            //{
//            //    var callback = _callback;
//            //    _callback = null;
//            //    callback();
//            //}
//        }
//        protected virtual void OnFinish()
//        {
//            OnMoveFinish();
//        }


//        protected abstract void OnUpdate(float deltaTime);


//    }
//}