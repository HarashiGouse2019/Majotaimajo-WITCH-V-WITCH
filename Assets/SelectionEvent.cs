using UnityEngine;
using UnityEngine.Events;

public class SelectionEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent @onSelected = new UnityEvent();

    public UnityEvent GetUnityEvent() => onSelected;
}
