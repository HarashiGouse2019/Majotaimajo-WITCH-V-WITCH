using UnityEngine;

public class PromptMessage
{
    string _message;
    public string Message
    {
        get
        {
            return _message;
        }
    }
    public PromptMessage(string content)
    {
        _message = content;
    }
}
