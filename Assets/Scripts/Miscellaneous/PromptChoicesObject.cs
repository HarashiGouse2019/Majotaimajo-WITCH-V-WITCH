using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptChoicesObject : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    Image choiceCursor;

    public TextMeshProUGUI GetMessageText() => messageText;
    public void EnableCursor() => choiceCursor.enabled = true;
    public void DisableCursor() => choiceCursor.enabled = false;
}