using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Reference to canvas (we'll make it a list since we have so many)
    public List<Canvas> Canvas;

    //We need to reverence all the following
    public TextMeshProUGUI HISCORETEXT;
    public TextMeshProUGUI SCORETEXT;
    public Image SPIRITS;
    public Image SLOT1, SLOT2, SLOT3;

    //Get lives, score, etc
    int hiScore;
    int score;
    int tSpirits;
    float magic;

    float texWidth, texHeight;

    // Start is called before the first frame update
    void Start()
    {
        SetLives(3);
        #region Singleton
            if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        HISCORETEXT.text = hiScore.ToString("D8");
        SCORETEXT.text = score.ToString("D8");

        AddToScore(1);

        if (Input.GetKeyDown(KeyCode.Backspace)) ResetScore();
    }

    public void AddToScore(int _total)
    {
        score += _total;
        if (score > hiScore) PostHighScore();
    }

    public void DecrementLives()
    {

    }

    public void DecrementMagic(int _value)
    {

    }

    public void ActivateSlot(Image _slot)
    {

    }

    public void PostHighScore()
    {
        hiScore = score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public int GetLives()
    {
        return tSpirits;
    }

    void SetLives(int _value)
    {
        tSpirits = _value;
    }
}