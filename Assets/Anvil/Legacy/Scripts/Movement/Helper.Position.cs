using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {

        public static Vector3 GetQuadBezierControl(Vector3 startPos, Vector3 endPos, Vector2 controlAnchor, Vector2 controlOffset, bool random)
        {
            if (random)
            {
                float offsetX = Mathf.Abs(controlOffset.x);
                if (startPos.x < endPos.x)
                {
                    // Top-Left
                    if (RandomBool())
                    {
                        controlOffset.x = -offsetX;
                    }
                    // Bottom-Right
                    else
                    {
                        controlAnchor.x = 1 - controlAnchor.x;
                        controlAnchor.y = 1 - controlAnchor.y;
                        controlOffset.x = offsetX;
                    }
                }
                else
                {
                    // Top-Right
                    if (RandomBool())
                    {
                        controlOffset.x = offsetX;
                    }
                    // Bottom-Left
                    else
                    {
                        controlAnchor.x = 1 - controlAnchor.x;
                        controlAnchor.y = 1 - controlAnchor.y;
                        controlOffset.x = -offsetX;
                    }
                }
            }

            float controlX = startPos.x + controlAnchor.x * (endPos.x - startPos.x) + controlOffset.x;
            float controlY = startPos.y + controlAnchor.y * (endPos.y - startPos.y) + controlOffset.y;
            return new Vector3(controlX, controlY);
        }
    }
}