using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static float GetWidth(this Camera camera)
        {
            return camera.orthographicSize * camera.aspect * 2;
        }

        public static void GetSize(this Camera camera, out float width, out float height)
        {
            height = camera.orthographicSize * 2;
            width = height * camera.aspect;
        }

        public static float GetLeft(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            return position.x - halfWidth;
        }

        public static float GetRight(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            return position.x + halfWidth;
        }

        public static void GetLeftRight(this Camera camera, out float left, out float right)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            left = position.x - halfWidth;
            right = position.x + halfWidth;
        }

        public static float GetBottom(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            return position.y - halfHeight;
        }

        public static float GetTop(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            return position.y + halfHeight;
        }

        public static Vector3 GetTopLeft(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            return new Vector3(position.x - halfWidth, position.y - halfHeight);
        }

        public static void GetTopLeft(this Camera camera, out float top, out float left)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            left = position.x - halfWidth;
            top = position.y + halfHeight;
        }

        public static Vector3 GetBottomRight(this Camera camera)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            return new Vector3(position.x + halfWidth, position.y - halfHeight);
        }

        public static void GetBottomRight(this Camera camera, out float bottom, out float right)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            right = position.x + halfWidth;
            bottom = position.y - halfHeight;
        }

        public static void GetAABB(this Camera camera, out float left, out float top, out float right, out float bottom)
        {
            var position = camera.transform.position;
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            left = position.x - halfWidth;
            top = position.y + halfHeight;
            right = position.x + halfWidth;
            bottom = position.y - halfHeight;
        }
    }
}