using UnityEngine;

namespace Anvil.Legacy
{
    public interface IGameObject
    {
        GameObject GameObject { get; }
    }

    //public interface IGameObject
    //{
    //    ITransform Transform { get; }
    //    bool Visible { get; set; }
    //}

    //public class DefaultGameObject : IGameObject
    //{
    //    ITransform _transform;
    //    bool _visible;

    //    DefaultGameObject()
    //    {

    //    }

    //    public ITransform Transform => _transform;

    //    public bool Visible
    //    {
    //        get => _visible;
    //        set => _visible = value;
    //    }

    //    static DefaultGameObject _instance;
    //    public static DefaultGameObject Instance => _instance ??= new();
    //}
}