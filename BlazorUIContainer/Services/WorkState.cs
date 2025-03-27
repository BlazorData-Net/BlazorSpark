public class WorkState
{
    private string _currentMessage;
    public string CurrentMessage => _currentMessage;

    public void SetMessage(string message)
    {
        if (!string.Equals(_currentMessage, message, StringComparison.Ordinal))
        {
            _currentMessage = message;
        }
    }
}