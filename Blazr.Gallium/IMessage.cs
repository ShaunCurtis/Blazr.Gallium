/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Gallium;

public interface IMessage
{
}

public readonly record struct CounterMessage : IMessage
{
    public int Value { get; init; }
}