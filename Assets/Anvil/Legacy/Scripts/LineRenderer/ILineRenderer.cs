using UnityEngine;

namespace Anvil.Legacy
{
    public interface ILineRenderer : IColorController
    {
        void SetLine(Vector3 startPos, Vector3 endPos);
    }

    public class DefaultLineRenderer : ILineRenderer
    {
        public GameObject GameObject => default;

        public Color Color { get; set; }

        public void SetLine(Vector3 startPos, Vector3 endPos)
        {

        }

        public void ReturnToPool()
        {

        }
    }
}