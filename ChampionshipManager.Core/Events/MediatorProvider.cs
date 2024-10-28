namespace ChampionshipManager.Core.Events;

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

/// <summary>
/// Simple implementation of the Mediator Pattern. Decouples domain events from the multiple posible actions they may trigger.
/// </summary>
internal static class MediatorProvider<T>
{
    private const int BufferSize = 10;

    private static ReplaySubject<T> Mediator { get; set; } = new(BufferSize);

    public static void Publish(T @event)
    {
        Mediator.OnNext(@event);
    }

    public static void Subscribe(Action<T> action)
    {
        Mediator.Subscribe(action);
    }

    public static void Subscribe(Func<T, bool> predicate, Action<T> action)
    {
        Mediator
            .Where(predicate)
            .Subscribe(action);
    }
}
