using UnityEngine;

namespace Anvil.Legacy
{
    public class UnityLineRenderer : ILineRenderer
    {
        LineRenderer _lineRenderer;

        public UnityLineRenderer(LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = 2;
            _lineRenderer = lineRenderer;
        }

        public void SetLine(Vector3 startPos, Vector3 endPos)
        {
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }

        public GameObject GameObject => _lineRenderer?.gameObject;

        public Color Color
        {
            get => _lineRenderer.startColor;
            set => _lineRenderer.startColor = _lineRenderer.endColor = value;
        }

        public void ReturnToPool()
        {

        }
    }
}