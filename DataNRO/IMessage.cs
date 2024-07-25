namespace DataNRO
{
    internal interface IMessage
    {
        sbyte Command { get; }
        byte[] Buffer { get; }
        long DataLength { get; }
        long CurrentPosition { get; }
    }
}
