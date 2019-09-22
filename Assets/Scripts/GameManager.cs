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
    bool typeIn;

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
        } else {
            Destroy(gameObject);
        }
        #endregion
        
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
            if (dialogue.text.Length > 0)
            {
                dialogue.text = "";
            }

            //This give a typewritter effect. With a ton of trial and error, this one works the best!!!
            for (int i = 0; i < text.Length; i++)
            {
                dialogue.text = text.Substring(0, i);
                //AudioManager.audio.Play("Type000");
                //Just in chase you don't want to read dialogue...
                if (Input.GetKeyDown(skipKey))
                {
                     dialogue.text = text;
                    break;
                }

                yield return new WaitForSeconds(textspeed);

            }
            isDone = true;
        }
    }

    public IEnumerator DisableDelay(float _delay)
    {
        
        yield return new WaitForSeconds(_delay);
        textBoxUI.gameObject.SetActive(false);
        typeIn = false;
        StopCoroutine(DisableDelay(_delay));
    }

    public void ToNextDialogue()
    {
        StopCoroutine(Dialogue.Instance.displayText);
        if (Input.GetKeyDown(skipKey))
        {
            isDone = false;
            dialoguePos++;
            Dialogue.Instance.Run(dialoguePos);
        }
    }
}