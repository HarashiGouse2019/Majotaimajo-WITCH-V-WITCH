using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static Keymapper;

public class PauseSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseSelectionObj;
    private bool _isPaused = false;

    [SerializeField]
    PostProcessProfile profile;
    DepthOfField depthOfField;

    private void Start()
    {
        depthOfField = profile.GetSetting<DepthOfField>();
    }

    private void OnEnable()
    {
        StartCoroutine(Listen());
    }

    public void TogglePauseMenu()
    {
        BoolParameter enableBlur = new BoolParameter();
        _isPaused = !_isPaused;
        enableBlur.Override(_isPaused);
        _pauseSelectionObj.gameObject.SetActive(_isPaused);
        depthOfField.enabled = enableBlur;
        Time.timeScale = _isPaused ? 0 : 1;
    }

    IEnumerator Listen()
    {
        while (true)
        {
            ControlAction("pause", false, TogglePauseMenu);
            yield return null;
        }
    }
}
