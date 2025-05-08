namespace ServiceManagement.StateStore;

public class Observer
{
    protected Action? Listeners;

    public void AddStateChanggeListeners(Action? listener)
    {
        Listeners += listener;
    }

    public void RemoveStateChangeListeners(Action? listener)
    {
        if (listener != null)
            Listeners -= listener;
    }

    public void BroadcastStateChange()
    {
        Listeners?.Invoke();
    }
}
