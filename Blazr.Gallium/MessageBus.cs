/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Threading.Tasks.Dataflow;

namespace Blazr.Gallium;

public class MessageBus : IMessageBus
{
    private readonly record struct Handler(Subscription Subscription, object Message);

    private readonly List<Subscription> _subscriptions = new();
    private ActionBlock<Handler> _queue;

    public MessageBus()
    {
        _queue = new ActionBlock<Handler>((handler) =>
        {
            handler.Subscription.SubscriptionAction.Invoke(handler.Message);
        });
    }

    public void Subscribe<TMessage>(Action<object> callback)
    {
        var subscription = new Subscription(typeof(TMessage), callback);

        if (!_subscriptions.Contains(subscription))
            _subscriptions.Add(subscription);
    }

    public void Publish<TTarget>(object? message)
    {
        ArgumentNullException.ThrowIfNull(message);

        //List<Subscription> handlers;

        var handlers = _subscriptions.Where(item => item.SubscriptionType == typeof(TTarget));

        lock (_subscriptions)
        {
            foreach (var handler in handlers)
                _queue.Post(new(handler, message));
        }
    }

    public void UnSubscribe<TMessage>(Action<object> callback)
    {
        var subscription = new Subscription(typeof(TMessage), callback);

        if (_subscriptions.Contains(subscription))
            _subscriptions.Remove(subscription);
    }
}
