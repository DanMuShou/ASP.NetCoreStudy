namespace ServiceManagement.StateStore;

public class TorontoOnlineServersStore : Observer
{
    private int _numServersOnline;

    public int GetNumberServersOnline() => _numServersOnline;

    public void SetNumbersServerOnline(int number)
    {
        _numServersOnline = number;
        BroadcastStateChange();
    }
}
