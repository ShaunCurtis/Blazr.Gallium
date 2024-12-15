/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Gallium.Web;

public readonly record struct CounterMessage 
{
    public int Value { get; init; }
}