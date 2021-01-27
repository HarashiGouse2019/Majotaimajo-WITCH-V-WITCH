using UnityEngine;

public class MusicTheme : MonoBehaviour
{
    [SerializeField] string themeSong;

    private void Start()
    {
       MusicManager.Play(themeSong);
    }
}
