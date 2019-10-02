using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
        Instance = this;
    }
    public void Run(int _index, float _speed = 0.05f)
    {

        manager= GameManager.Instance;

        GameManager.Instance.expression.sprite = dialogue[_index].expression.image;

        displayText = manager.DisplayText(dialogue[_index].speech, _speed, dialogue[_index].voice);

        StartCoroutine(displayText);
    }
}
