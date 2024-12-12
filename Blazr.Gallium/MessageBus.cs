/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Gallium;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly List<Subscription> _subscriptions = new();
    private SemaphoreSlim _semaphore = new(1,1);

    public void Subscribe<T>(Action<object> callback) where T : IMessage
    {
        _semaphore.Wait();

        var subscription = new Subscription(typeof(T), callback);

        if (!_subscriptions.Contains(subscription))
            _subscriptions.Add(subscription);

        _semaphore.Release();
    }

    public void Publish<T>(T message) where T : IMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        _semaphore.Wait();

        var handlers = _subscriptions.Where(item => item.SubscriptionType == typeof(T));

        foreach (var handler in handlers)
            ThreadPool.QueueUserWorkItem((state) => handler.SubscriptionAction.Invoke(message));

        _semaphore.Release();
    }

    public void UnSubscribe<T>(Action<object> callback) where T : IMessage
    {
        _semaphore.Wait();

        var subscription = new Subscription(typeof(T), callback);

        if (_subscriptions.Contains(subscription))
            _subscriptions.Remove(subscription);

        _semaphore.Release();
    }

    public void Dispose()
    {
         _subscriptions.Clear();
    }
}
