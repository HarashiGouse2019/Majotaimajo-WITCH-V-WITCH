using System;
using System.Collections;
using UnityEngine;
using static Keymapper;

public class PauseSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseSelectionObj, _gameOverSelectionObj;
    private bool _isPaused = false;

    public const int ZERO = 0;

    public const int ONE = 1;

    private void Start()
    {
        StartCoroutine(Listen());
    }

    public void TogglePauseMenu()
    {
        _isPaused = !_isPaused;
        _pauseSelectionObj.gameObject.SetActive(_isPaused);
        Time.timeScale = _isPaused ? ZERO : ONE;
    }

    public void ToggleGameOverMenu()
    {
        _isPaused = !_isPaused;
        _gameOverSelectionObj.gameObject.SetActive(_isPaused);
        Time.timeScale = _isPaused ? ZERO : ONE;
    }

    IEnumerator Listen()
    {
        while (true)
        {
            EventManager.Watch(GameManager.IsPlayerAlive,
                () => ControlAction("pause", false, TogglePauseMenu),
                () => ToggleGameOverMenu());       

            yield return null;
        }
    }
}
