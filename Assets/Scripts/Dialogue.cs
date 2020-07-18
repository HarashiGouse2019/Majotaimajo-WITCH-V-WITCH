using System.Collections;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance;

    [System.Serializable]
    public class Script
    {
        public enum Voices
        {
            None,
            Amben,
            Augusta,
            Crystal,
            Luu,
            Maple,
            Raven
        }

        public Expression expression;
        public Voices voice;
        public string speech;
    }

    public Script[] dialogue;

    GameManager manager;

    public IEnumerator displayText;

    public static bool IsRunning = false;
    public bool isRunning = true;
    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }
    public void Run(int _index, float _speed = 0.05f)
    {

        manager = GameManager.Instance;

        GameManager.Instance.expression.sprite = dialogue[_index].expression.image;

        displayText = manager.DisplayText(dialogue[_index].speech, _speed, dialogue[_index].voice);

        IsRunning = true;
        isRunning = IsRunning;

        StartCoroutine(displayText);
    }

    public static void OnDialogueEnd()
    {
        Instance.isRunning = IsRunning;
        if (!IsRunning)
        {
            EventManager.TriggerEvent("DialogueEnd");
        }
    }
}
