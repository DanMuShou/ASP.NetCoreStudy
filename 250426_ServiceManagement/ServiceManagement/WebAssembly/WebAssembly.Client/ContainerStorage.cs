namespace WebAssembly.Client;

public class ContainerStorage
{
    private string _message = string.Empty;

    public string GetMessage() => _message;

    public void SetMessage(string message) => _message = message;
}
