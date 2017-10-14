namespace Clickstreamer.Processing
{
    public interface IProcess : ICanStart, ICanStop
    {
        string Name { get; }
    }
}