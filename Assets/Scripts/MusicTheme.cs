using UnityEngine;

public class MusicTheme : MonoBehaviour
{
    [SerializeField] string themeSong;

    public void Play()
    {
       MusicManager.Play(themeSong);
    }
}
