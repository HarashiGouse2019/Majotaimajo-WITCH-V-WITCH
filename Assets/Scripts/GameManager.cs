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
    [Header("Game Status")]
    public TextMeshProUGUI HISCORETEXT;
    public TextMeshProUGUI SCORETEXT;
    public Image SPIRITS;
    public Image SLOT1, SLOT2, SLOT3;

    [Header("Text Box")]
    public Image textBoxUI;
    public TextMeshProUGUI dialogue;

    //Get lives, score, etc
    int hiScore;
    int score;
    int tSpirits;
    DanmakuSequencer danmakuSequencer;

    KeyCode skipKey = KeyCode.Return;
    public int dialoguePos = 0;
    public bool isDone = false;


    // Start is called before the first frame update
    void Awake()
    {
        SetLives(3);
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        danmakuSequencer = GetComponent<DanmakuSequencer>();
    }

    private void Start()
    {
        Dialogue.Instance.Run(0);
    }

    // Update is called once per frame
    void Update()
    {
        HISCORETEXT.text = hiScore.ToString("D8");
        SCORETEXT.text = score.ToString("D8");

        AddToScore(1);

        if (Input.GetKeyDown(KeyCode.Backspace)) ResetScore();
        if (isDone == true) ToNextDialogue();
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

    public IEnumerator DisplayText(string text, float textspeed)
    {
        if (isDone == false)
        {
            textBoxUI.gameObject.SetActive(true);
            dialogue.text = "";


            //This give a typewritter effect. With a ton of trial and error, this one works the best!!!
            for (int i = 0; i < text.Length + 1; i++)
            {
                if (Input.GetKeyDown(skipKey) && i > 0)
                {
                    i = text.Length + 1;
                    dialogue.text = text;
                    break;
                }
                else
                {
                    dialogue.text = text.Substring(0, i);
                    //AudioManager.audio.Play("Type000");

                    yield return new WaitForSeconds(textspeed);
                }
            }
            isDone = true;
        }
    }

    public IEnumerator DisableDelay(float _delay)
    {

        yield return new WaitForSeconds(_delay);
        textBoxUI.gameObject.SetActive(false);
        StopCoroutine(DisableDelay(_delay));
    }

    public void ToNextDialogue()
    {
        if (Input.GetKeyDown(skipKey))
        {
            if (dialoguePos < Dialogue.Instance.dialogue.Length - 1)
            {
                isDone = false;
                dialoguePos++;
                dialogue.text = "";
                Dialogue.Instance.Run(dialoguePos);
            } else
            {
                dialogue.text = "";
                textBoxUI.gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("target").GetComponent<DanmakuSequencer>().enabled = true;
                //This is where we start our Danmaku routines
                //In another script of course!!

            }
        }
    }
}