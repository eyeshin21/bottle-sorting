using UnityEngine;

namespace Anvil.Legacy
{
    public interface ISerialize
    {
        string Serialize();
    }

    public interface IDeserialize
    {
        void Deserialize(string json);
    }

    public interface IData : ISerialize, IDeserialize
    {

    }
}