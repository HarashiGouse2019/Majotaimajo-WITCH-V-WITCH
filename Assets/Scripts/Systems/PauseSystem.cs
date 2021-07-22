using System;
using System.Collections;
using UnityEngine;
using static Keymapper;

public class PauseSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseSelectionObj, _gameOverSelectionObj, _spellLibraryListObj;
    private bool _isPaused = false;

    public const int ZERO = 0;

    public const int ONE = 1;

    public float initTrackVol = -1;

    private void Start()
    {
        StartCoroutine(Listen());
    }

    public void TogglePauseMenu()
    {
        if(initTrackVol == -1)
        {
            initTrackVol = MusicManager.NowPlaying.volume;
        }

        _isPaused = !_isPaused;
        _pauseSelectionObj.gameObject.SetActive(_isPaused);
        Time.timeScale = _isPaused ? ZERO : ONE;
        MusicManager.SetVolume(MusicManager.NowPlaying.name, _isPaused ? initTrackVol / 2 : initTrackVol);
    }

    public void ToggleGameOverMenu()
    {
        _isPaused = !_isPaused;
        _gameOverSelectionObj.gameObject.SetActive(_isPaused);
        Time.timeScale = _isPaused ? ZERO : ONE;
    }

    public void Confirm(ConfirmationType type)
    {
        switch (type)
        {
            case ConfirmationType.NULL:
                return;
            case ConfirmationType.RETRY:
                //TODO: Toggle Prompt, give title (Do you wish to try the level?)
                return;
            case ConfirmationType.GIVEUP:
                //TODO: Toggle Prompt, give tittle (Do you wish to give up?)
                return;
            default:
                return;
        }
    }

    public void ToggleSpellLibraryList()
    {
        _spellLibraryListObj.SetActive(!_spellLibraryListObj.activeSelf);
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
