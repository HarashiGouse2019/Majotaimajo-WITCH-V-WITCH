using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public Members
    public static GameManager Instance;

    //Reference to canvas (we'll make it a list since we have so many)
    public List<Canvas> Canvas;

    public TextureRenderer tRenderer;

    //We need to reverence all the following
    [Header("Game Status")]
    public TextMeshProUGUI HISCORETEXT;
    public TextMeshProUGUI SCORETEXT;
    public Image SPIRITS;
    public Image MAGIC;
    public Image[] SLOTS = new Image[3];
    public Image BOSSHEALTH;

    [Header("Text Box")]
    public Image textBoxUI;
    public TextMeshProUGUI dialogue;
    public Image expression;

    //Score System
    public int timesHit;


    #endregion

    #region Private Members
    //Get lives, score, etc
    int hiScore;
    int score;
    int tSpirits;
    float magic;


    readonly KeyCode skipKey = KeyCode.Return;
    int dialoguePos = 0;
    bool isDone = false;

    readonly float flashVal = 255f;
    float rVal, gVal, bVal;


    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        SetPlayerLives(5);
        Application.targetFrameRate = 60;
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
    }

    private void Start()
    {
        Dialogue.Instance.Run(0);
        tRenderer.check = true;
        magic = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HISCORETEXT.text = hiScore.ToString("D10");
        SCORETEXT.text = score.ToString("D10");

        if (!textBoxUI.gameObject.activeSelf) AddToScore(1);

        if (isDone == true) ToNextDialogue();
    }

    /// <summary>
    /// Add any value to current score.
    /// </summary>
    /// <param name="_total"></param>
    public void AddToScore(int _total)
    {
        score += _total;
        if (score > hiScore) UpdateHighScore();
    }

    /// <summary>
    /// Decrement the player's lives
    /// </summary>
    public void DecrementLives()
    {
        tSpirits--;
        timesHit = 0;
        tRenderer.check = true;
        ResetScore();
    }

    /// <summary>
    /// Decrease Patience Value by a certain amount
    /// </summary>
    /// <param name="_value"></param>
    public void DecrementPatience(float _value)
    {
        BOSSHEALTH.fillAmount -= _value / 100f;
    }

    /// <summary>
    /// Decrement player magic
    /// </summary>
    /// <param name="_value"></param>
    public void DecrementMagic(float _value)
    {
        magic -= _value;
        MAGIC.fillAmount = magic / 100f;
    }

    /// <summary>
    /// Activate a slot based on the key pressed (A, S, or D)
    /// </summary>
    /// <param name="_slotIndex"></param>
    /// <param name="_on"></param>
    public void ActivateSlot(int _slotIndex, bool _on)
    {

        switch (_on)
        {
            case true:
                rVal = SLOTS[_slotIndex].color.r;
                gVal = SLOTS[_slotIndex].color.g;
                bVal = SLOTS[_slotIndex].color.b;

                SLOTS[_slotIndex].color = new Color(rVal, flashVal, bVal);

                break;
            case false:
                SLOTS[_slotIndex].color = new Color(rVal, gVal, bVal);

                break;
        }
    }

    /// <summary>
    /// Update the High Score value
    /// </summary>
    public void UpdateHighScore()
    {
        hiScore = score;
    }

    /// <summary>
    /// Reset the Score Completely
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }

    /// <summary>
    /// Decrease the score by a certain amount.
    /// </summary>
    /// <param name="_value"></param>
    public void DecrementScore(int _value)
    {
        score -= _value;
    }

    public int GetPlayerLives()
    {
        return tSpirits;
    }

    public float GetPlayerMagic()
    {
        return magic;
    }

    void SetPlayerLives(int _value)
    {
        tSpirits = _value;
    }

    public IEnumerator DisplayText(string text, float textspeed, Dialogue.Script.Voices voice)
    {
        if (isDone == false)
        {
            textBoxUI.gameObject.SetActive(true);
            dialogue.text = "";


            //This give a typewritter effect. With a ton of trial and error, this one works the best!!!
            for (int i = 0; i < text.Length + 1; i++)
            {

                dialogue.text = text.Substring(0, i);

                #region Voices

                switch (voice)
                {
                    case Dialogue.Script.Voices.None:
                        AudioManager.audio.Play("Type000");
                        break;
                    case Dialogue.Script.Voices.Amber:
                        AudioManager.audio.Play("Type000");
                        break;
                    case Dialogue.Script.Voices.August:
                        AudioManager.audio.Play("Type000");
                        break;
                    case Dialogue.Script.Voices.Crystal:
                        AudioManager.audio.Play("Type000");
                        break;
                    case Dialogue.Script.Voices.Luu:
                        AudioManager.audio.Play("LuuVoice");
                        break;
                    case Dialogue.Script.Voices.Maple:
                        AudioManager.audio.Play("Type000");
                        break;
                    case Dialogue.Script.Voices.Raven:
                        AudioManager.audio.Play("RavenVoice");
                        break;
                    default:
                        break;
                }
                #endregion

                yield return new WaitForSeconds(textspeed);

                if (Input.GetKeyDown(skipKey))
                {
                    i = text.Length + 1;
                    dialogue.text = text;
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
            }
            else
            {
                dialogue.text = "";
                textBoxUI.gameObject.SetActive(false);
                PlayerPawn.Instance.originOfRotation.GetComponent<LuuPawn>().ActivateSpell("Sakura Burst");
                //This is where we start our Danmaku routines
                //In another script of course!!

            }
        }
    }
}