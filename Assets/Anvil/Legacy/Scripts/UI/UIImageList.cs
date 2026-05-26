//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;

//namespace Gametamin
//{
//    public class UIImageList : UIMeshBehaviour
//    {
//        [SerializeField] Vector2 _size;

//#if UNITY_EDITOR
//        protected override void OnSpriteChanged()
//        {
//            SetNativeSize(_sprite);
//        }
//#endif

//        protected override void OnPopulateMesh(VertexHelper vh)
//        {
//            vh.Clear();
//            GetAABB(out float left, out float top, out float right, out float bottom);
//            GetUVs(activeSprite, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
//            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, _transformType);

//            /*
//             *      1		2
//             * 		+-------+
//             * 		| 	  / |
//             * 		|   /  	|
//             * 		| /	  	|
//             * 		+-------+
//             *      0       3
//             */
//            //Color32 color32 = color;
//            //vh.AddVert(new Vector3(left, bottom), color32, new Vector4(uvLeft, uvBottom));
//            //vh.AddVert(new Vector3(left, top), color32, new Vector4(uvLeft, uvTop));
//            //vh.AddVert(new Vector3(right, top), color32, new Vector4(uvRight, uvTop));
//            //vh.AddVert(new Vector3(right, bottom), color32, new Vector4(uvRight, uvBottom));
//            //vh.AddTriangle(0, 1, 2);
//            //vh.AddTriangle(2, 3, 0);

//#if UNITY_EDITOR
//            FillMesh(vh);
//#endif
//        }
//    }
//}