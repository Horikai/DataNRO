namespace DataNRO.Interfaces
{
    public interface IMessage
    {
        sbyte Command { get; }
        byte[] Buffer { get; }
        long DataLength { get; }
        long CurrentPosition { get; }
    }
}
