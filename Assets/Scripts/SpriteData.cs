using Extensions;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Data", menuName = "Sprite Data")]
public class SpriteData : ScriptableObject
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private int _spriteFrameRate = _DEFAULT_FRAMERATE;

    [SerializeField]
    private Sprite[] _subImages = new Sprite[1];

    public int ImageIndex { get; private set; } = 0;
    public int TotalFrames { get; private set; }
    public bool IsPlaying { get; private set; }

    private const int _DEFAULT_FRAMERATE = 30;

    public string GetName => m_name;
    public  int GetFrameRate => _spriteFrameRate;
    IEnumerator animationCycle;

    private void Play()
    {
        IsPlaying = true;
        animationCycle.Start();
    }

    private void Stop()
    {
        IsPlaying = false;
        animationCycle.Stop();
    }

    private void OnEnable()
    {
        if (TotalFrames == 0 && _subImages != null) TotalFrames = _subImages.Length;
        animationCycle = Animate();
    }

    IEnumerator Animate()
    {
        while (true)
        {
            NextSubImage();
            yield return new WaitForSeconds(1 / _spriteFrameRate);
        }
    }

    void NextSubImage()
    {
        ImageIndex += (ImageIndex > TotalFrames - 1) ? -1 * TotalFrames : 1;
        Debug.Log(ImageIndex);
    }
}