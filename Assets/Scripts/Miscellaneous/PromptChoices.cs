using UnityEngine;
using UnityEngine.Events;

public class PromptChoices
{ 
    string _message;

    EventManager.Event _onSelect;

    public string Message
    {
        get
        {
            return _message;
        }
    }

    public EventManager.Event OnSelect
    {
        get
        {
            return _onSelect;
        }
    }
    public PromptChoices(string message, EventManager.CallBackMethod onSelectEvent = null)
    {
        _message = message;
        _onSelect = EventManager.AddEvent(EventManager.AnyFreeID(), message, onSelectEvent);
    }
}
