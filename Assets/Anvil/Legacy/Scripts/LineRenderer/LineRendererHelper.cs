using UnityEngine;

namespace Anvil.Legacy
{
    public static class LineRendererHelper
    {
        public static ILineRenderer Create(GameObject gameObject)
        {
            var lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                return new UnityLineRenderer(lineRenderer);
            }

            LegacyLog.Warning($"Can't create line renderer for {gameObject}!");
            return new DefaultLineRenderer();
        }
    }
}