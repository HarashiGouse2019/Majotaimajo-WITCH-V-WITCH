﻿using System.Collections;
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
            Amber,
            August,
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
    private void Awake()
    {
        Instance = this;
    }
    public void Run(int _index, float _speed = 0.05f)
    {

        manager = GameManager.Instance;

        GameManager.Instance.expression.sprite = dialogue[_index].expression.image;

        displayText = manager.DisplayText(dialogue[_index].speech, _speed, dialogue[_index].voice);

        IsRunning = true;

        StartCoroutine(displayText);
    }

    public static void OnDialogueEnd()
    {
        if (!IsRunning)
        {
            foreach (EventManager.Event _event in EventManager.GetAllEvents())
            {
                if (_event.GetEventCode() == "DialogueEnd"){
                    _event.Trigger();
                }
            }
        }
    }
}
