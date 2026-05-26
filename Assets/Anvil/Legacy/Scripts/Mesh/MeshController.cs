using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class MeshController
    {
        Mesh _mesh;
        //int _quadCount;
        Vector3[] _vertices;
        int[] _triangles;
        Vector2[] _uvs;
        int _index4, _index6;

        public int QuadCount
        {
            get
            {
                Assert.IsTrue(_index4 % 4 == 0, $"Invalid index 4 {_index4}");
                Assert.IsTrue(_index6 % 6 == 0, $"Invalid index 6 {_index6}");
                int quadCount = _index4 / 4;
                Assert.IsEquals(quadCount, _index6 / 6);
                return quadCount;
            }
        }

        void Construct(Mesh mesh, int quadCount)
        {
            //_quadCount = quadCount;
            if (quadCount > 0)
            {
                _mesh = mesh;
                _vertices = new Vector3[quadCount * 4];
                _triangles = new int[quadCount * 6];
                _uvs = new Vector2[quadCount * 4];
                _index4 = _index6 = 0;
            }
            else
            {
                mesh.Clear();
                mesh.RecalculateBounds();
            }
        }

        void AddVertices(float left, float top, float right, float bottom)
        {
            _vertices[_index4].Set(left, bottom, 0);
            _vertices[_index4 + 1].Set(left, top, 0);
            _vertices[_index4 + 2].Set(right, bottom, 0);
            _vertices[_index4 + 3].Set(right, top, 0);
        }

        void AddTriangles()
        {
            _triangles[_index6++] = _index4;
            _triangles[_index6++] = _index4 + 1;
            _triangles[_index6++] = _index4 + 2;
            _triangles[_index6++] = _index4 + 2;
            _triangles[_index6++] = _index4 + 1;
            _triangles[_index6++] = _index4 + 3;
        }

        void AddUVs(float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            _uvs[_index4++].Set(uvLeft, uvBottom);
            _uvs[_index4++].Set(uvLeft, uvTop);
            _uvs[_index4++].Set(uvRight, uvBottom);
            _uvs[_index4++].Set(uvRight, uvTop);
        }

        public void AddQuad(float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            AddVertices(left, top, right, bottom);
            AddTriangles();
            AddUVs(uvLeft, uvTop, uvRight, uvBottom);
        }

        public void AddQuad(MeshQuad quad)
        {
            AddVertices(quad.Left, quad.Top, quad.Right, quad.Bottom);
            AddTriangles();
            AddUVs(quad.UVLeft, quad.UVTop, quad.UVRight, quad.UVBottom);
        }

        public void AddQuads(List<MeshQuad> quads)
        {
            int count = quads.GetCount();
            for (int i = 0; i < count; i++)
            {
                AddQuad(quads[i]);
            }
        }

        public void UpdateMesh(bool returnToPool = true)
        {
            var mesh = _mesh;
            if (mesh != null)
            {
                mesh.Clear();
                mesh.vertices = _vertices;
                mesh.triangles = _triangles;
                mesh.uv = _uvs;
                mesh.RecalculateNormals();
            }

            if (returnToPool)
            {
                ReturnToPool();
            }
        }

        public override string ToString()
        {
            return $"quadCount={QuadCount}";
        }

        #region Pool
        void ReturnToPool()
        {
            _mesh = null;
            _vertices = null;
            _triangles = null;
            _uvs = null;

            _pool.Return(this);
        }

        static Pool<MeshController> _pool = new();

        public static MeshController Get(Mesh mesh, int quadCount)
        {
            var meshController = _pool.Get();
            meshController.Construct(mesh, quadCount);
            return meshController;
        }
        #endregion
    }
}