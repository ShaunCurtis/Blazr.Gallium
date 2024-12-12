/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Collections.Concurrent;

namespace Blazr.Gallium;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly List<Subscription> _subscriptions = new();
    private SemaphoreSlim _semaphore = new(1,1);

    public void Subscribe<T>(Action<object> callback) where T : IMessage
    {
        var subscription = new Subscription(typeof(T), callback);

        if (!_subscriptions.Contains(subscription))
            _subscriptions.Add(subscription);
    }

    public void Publish<T>(T message) where T : IMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        var handlers = _subscriptions.Where(item => item.SubscriptionType == typeof(T)).ToList();

        foreach (var handler in handlers)
            ThreadPool.QueueUserWorkItem((state) => handler.SubscriptionAction.Invoke(message));

    }

    public void UnSubscribe<T>(Action<object> callback) where T : IMessage
    {
        var subscription = new Subscription(typeof(T), callback);

        if (_subscriptions.Contains(subscription))
            _subscriptions.Remove(subscription);
    }

    public void Dispose()
    {
         _subscriptions.Clear();
    }
}
