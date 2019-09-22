using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance;
    public string[] dialogue;

    GameManager manager;

    public IEnumerator displayText;
    private void Awake()
    {
        Instance = this;

    }
    public void Run(int _index, float _speed = 0.05f)
    {
        Debug.Log("Run!");
        manager= GameManager.Instance;
        displayText = manager.DisplayText(dialogue[_index], _speed);
        StartCoroutine(displayText);
    }
}
