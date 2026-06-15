using UnityEngine;

namespace Anvil
{
    public interface ITransform
    {
        Vector3 Position { get; set; }
        Vector3 LocalPosition { get; set; }
        Vector3 LocalScale { get; set; }
        Quaternion LocalRotation { get; set; }
    }

    public class DefaultTransform : ITransform
    {
        Vector3 _position;
        Vector3 _localPosition;
        Vector3 _localScale = Defaults.Scale;
        Quaternion _localRotation = Defaults.Rotation;

        DefaultTransform()
        {

        }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set => _localPosition = value;
        }

        public Vector3 LocalScale
        {
            get => _localScale;
            set => _localScale = value;
        }

        public Quaternion LocalRotation
        {
            get => _localRotation;
            set => _localRotation = value;
        }

        static DefaultTransform _instance;
        public static DefaultTransform Instance => _instance ??= new();
    }
}