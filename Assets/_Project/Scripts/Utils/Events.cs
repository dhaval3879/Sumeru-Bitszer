using System;

public static class Events
{
    public static readonly Evt<Profile> OnProfileReceived = new Evt<Profile>();

    // To Invoke
    // Events.actionWithoutAnyParams.Invoke();

    // To Subscribe
    // Events.actionWithoutAnyParams.AddListener(MethodName);

    // To Unsubscribe
    // Events.actionWithoutAnyParams.RemoveListener(MethodName);
}

public class Evt
{
    private Action _action;

    public void Invoke() => _action?.Invoke();

    public void AddListener(Action listener) => _action += listener;

    public void RemoveListener(Action listener) => _action -= listener;
}

public class Evt<T>
{
    private Action<T> _action;

    public void Invoke(T param) => _action?.Invoke(param);

    public void AddListener(Action<T> listener) => _action += listener;

    public void RemoveListener(Action<T> listener) => _action -= listener;
}