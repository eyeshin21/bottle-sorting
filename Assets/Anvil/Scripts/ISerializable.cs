namespace Anvil
{
    public interface ISerializable
    {
        public string Serialize();
    }
    public interface IDeserializable
    {
        public void Deserialize(string data);
    }
}