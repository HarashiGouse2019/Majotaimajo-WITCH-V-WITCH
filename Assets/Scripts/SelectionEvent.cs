using UnityEngine;
using UnityEngine.Events;

public class SelectionEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent @onSelected = new UnityEvent();

    bool unavaliableSelection = false;

    public UnityEvent GetUnityEvent() => onSelected;

    public bool HasEvent()
    {
        unavaliableSelection = !(GetUnityEvent().GetPersistentEventCount() != 0);
        return !unavaliableSelection;
    }
    public bool IsUnAvaliable => (unavaliableSelection);

    public void MarkAsUnavaliable() => unavaliableSelection = true;

    public void MarkAsAvaliable() => unavaliableSelection = false;

    
}
