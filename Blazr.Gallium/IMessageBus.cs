/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Gallium;

public interface IMessageBus
{
    public void Publish<T>(T message) where T : IMessage;

    public void Subscribe<T>(Action<object> callback) where T : IMessage;

    public void UnSubscribe<T>(Action<object> callback) where T : IMessage;

    //internal void ClearAllSubscriptions();
    //internal void ClearSubscriptions<T>() where T : IMessage;

}