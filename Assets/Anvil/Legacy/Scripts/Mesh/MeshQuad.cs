using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class MeshQuad
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        public float UVLeft { get; set; }
        public float UVTop { get; set; }
        public float UVRight { get; set; }
        public float UVBottom { get; set; }

        public float CenterX => (Left + Right) * 0.5f;
        public float CenterY => (Bottom + Top) * 0.5f;

        public void Construct(float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;

            UVLeft = uvLeft;
            UVTop = uvTop;
            UVRight = uvRight;
            UVBottom = uvBottom;
        }

        public void Construct(float left, float top, float right, float bottom, float[] uvs, UVs uvLeft, UVs uvTop, UVs uvRight, UVs uvBottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;

            UVLeft = uvs[(int)uvLeft];
            UVTop = uvs[(int)uvTop];
            UVRight = uvs[(int)uvRight];
            UVBottom = uvs[(int)uvBottom];
        }

        #region Pool
        static Pool<MeshQuad> _pool = new();

        public static MeshQuad GetQuad(float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            var quad = _pool.Get();
            quad.Construct(left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
            return quad;
        }

        public static MeshQuad GetQuad(float left, float top, float right, float bottom, float[] uvs, UVs uvLeft, UVs uvTop, UVs uvRight, UVs uvBottom)
        {
            var quad = _pool.Get();
            quad.Construct(left, top, right, bottom, uvs, uvLeft, uvTop, uvRight, uvBottom);
            return quad;
        }

        //public static void AddQuad(List<MeshQuad> quads, float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        //{
        //    var quad = _pool.Get();
        //    quad.Construct(left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
        //    quads.Add(quad);
        //}

        public static void Return(List<MeshQuad> quads)
        {
            _pool.Return(quads);
        }
        #endregion
    }
}