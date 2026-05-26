using UnityEngine;

namespace Anvil.Legacy
{
    public class CircleMesh
    {
        float _radius;
        Mesh _mesh;

        public float Radius => _radius;
        public Mesh Mesh => _mesh;

        public CircleMesh(float radius, float segmentLength)
        {
            _radius = radius;
            _mesh = MeshHelper.GenerateCircle(radius, segmentLength);
        }
    }
}