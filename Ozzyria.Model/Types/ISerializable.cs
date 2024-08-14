namespace Ozzyria.Model.Types
{
    public interface ISerializable
    {
        public void Write(System.IO.BinaryWriter w);
        public void Read(System.IO.BinaryReader r);
    }
}
