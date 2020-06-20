using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public Slider MAGIC;
    public Image[] SLOTS = new Image[3];

    [Header("Text Box")]
    public Image textBoxUI;
    public TextMeshProUGUI dialogue;
    public Image expression;

    //Score System
    public int timesHit;

    public static bool IsPractice = false;
    #endregion

    #region Private Members
    //Get lives, score, etc
    int hiScore;
    int score;
    int tSpirits;

    float magic;
    float maxMagic = 100f;


    readonly KeyCode skipKey = KeyCode.Return;
    int dialoguePos = 0;
    bool isDone = false;

    readonly float flashVal = 255f;
    float rVal, gVal, bVal;

    List<ExposeAs> exposedObj = new List<ExposeAs>();
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        
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

    }



    // Update is called once per frame
    void Update()
    {
        if(HISCORETEXT != null) HISCORETEXT.text = hiScore.ToString("D10");
        if(SCORETEXT != null) SCORETEXT.text = score.ToString("D10");

        if (textBoxUI != null && !textBoxUI.gameObject.activeSelf) AddToScore(1);

        if (isDone == true) ToNextDialogue();
    }

    public static void StartGame()
    {
        switch (IsPractice) {
            case true:
                Instance.SetPlayerLives(10);
                break;

            case false:
                Instance.SetPlayerLives(5);
                break;
        }
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

    public void SetMaxMagic(int _value)
    {
        maxMagic = _value;
        MAGIC.maxValue = maxMagic;
    }

    /// <summary>
    /// Decrement player magic
    /// </summary>
    /// <param name="_value"></param>
    public void IncrementMagic(float _value)
    {
        magic += _value;
        MAGIC.value = magic;
    }

    /// <summary>
    /// Decrement player magic
    /// </summary>
    /// <param name="_value"></param>
    public void DecrementMagic(float _value)
    {
        magic -= _value;
        MAGIC.value = magic;
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
        ScoreSystem.SetHighScore(hiScore);
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
                        AudioManager.Play("Type000", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Amber:
                        AudioManager.Play("Type000", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.August:
                        AudioManager.Play("Type000", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Crystal:
                        AudioManager.Play("Type000", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Luu:
                        AudioManager.Play("LuuVoice", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Maple:
                        AudioManager.Play("MapleVoice", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Raven:
                        AudioManager.Play("RavenVoice", _oneShot: true);
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
        if (Input.GetKeyDown(skipKey) && textBoxUI != null)
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

                //This is where we start our Danmaku routines
                //In another script of course!!
                Dialogue.IsRunning = false;
                Dialogue.OnDialogueEnd();
            }
        }
    }

    void FindAllExposedValues()
    {
        exposedObj = FindObjectsOfType<ExposeAs>().ToList();
    }

    void AssignUIElements()
    {
        //Wait til 100
        int ping = 0;
        while (ping < 1000)
        {
            FindAllExposedValues();

            if (exposedObj == null)
                return;

            foreach (ExposeAs obj in exposedObj)
            {
                switch (obj.GetExposedAs())
                {
                    case "DialogueBox":
                        textBoxUI = obj.GetComponent<Image>();
                        break;
                    case "DialogueText":
                        dialogue = obj.GetComponent<TextMeshProUGUI>();
                        break;
                    case "ExpressionImage":
                        expression = obj.GetComponent<Image>();
                        break;
                    case "HighScore":
                        HISCORETEXT = obj.GetComponent<TextMeshProUGUI>();
                        break;
                    case "Score":
                        SCORETEXT = obj.GetComponent<TextMeshProUGUI>();
                        break;
                    case "TextureRenderer":
                        tRenderer = obj.GetComponent<TextureRenderer>();
                        break;
                    case "Magic":
                        MAGIC = obj.GetComponent<Slider>();
                        break;
                    case "Slot1":
                        SLOTS[0] = obj.GetComponent<Image>();
                        break;
                    case "Slot2":
                        SLOTS[1] = obj.GetComponent<Image>();
                        break;
                    case "Slot3":
                        SLOTS[2] = obj.GetComponent<Image>();
                        break;
                    default:
                        break;
                }
            }

            ping++;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        AssignUIElements();
    }
}