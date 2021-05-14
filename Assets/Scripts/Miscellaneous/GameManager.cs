using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using static Keymapper;
using Extensions;
using System;

public class GameManager : MonoBehaviour
{
    //TargetframeRate
    [SerializeField]
    private FrameRate targetFrameRate = FrameRate.FPS60;
    public static FrameRate TargetFrameRate { get => Instance.targetFrameRate; }

    #region Public Members
    public static GameManager Instance;

    public bool NoDeaths = false;

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
    public static bool HasGameStarted = false;
    public static bool IsPlayerAlive = false;
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

    public static int DifficultyIndex { get; private set; } = 0;

    List<ExposeAs> exposedObj = new List<ExposeAs>();

    public static RuntimeAnimatorController CharacterAnimatorController { get; private set; }
    public static Stats CharacterStats { get; private set; }

    public static PlayerPawn PlayerPawn { get; private set; }

    public static Stage CurrentStage { get; private set; }

    private bool OnStandAlone = true;

    public static float MaxMagic
    {
        get
        {
            return Instance.maxMagic;
        }
    }

    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        SetRatio(3, 2);

        #region Singleton
        if (Instance == null)
        {
            Instance = this;
#if UNITY_EDITOR
            OnStandAlone = false;
#endif
            if(OnStandAlone) DisableMouseVisibility();
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
        Application.targetFrameRate = (int)TargetFrameRate;

        SceneManager.sceneLoaded += OnLoadedScene;

        Configure(
           new Key("left", KeyCode.LeftArrow),
           new Key("right", KeyCode.RightArrow),
           new Key("up", KeyCode.UpArrow),
           new Key("down", KeyCode.DownArrow),
           new Key("shoot", KeyCode.Z),
           new Key("dash", KeyCode.X),
           new Key("sneak", KeyCode.LeftShift),
           new Key("specialA", KeyCode.A),
           new Key("specialB", KeyCode.S),
           new Key("specialC", KeyCode.D),
           new Key("itemSelection", KeyCode.Space),
           new Key("start", KeyCode.Return),
           new Key("cancel", KeyCode.Backspace),
           new Key("pause", KeyCode.Escape)
       );

        UIUpdateCycle(0.0001f).Start();
    }

    IEnumerator UIUpdateCycle(float delta)
    {
        while (true)
        {
            if (HISCORETEXT != null) HISCORETEXT.text = hiScore.ToString("D10");
            if (SCORETEXT != null) SCORETEXT.text = score.ToString("D10");

            //if (textBoxUI != null && !textBoxUI.gameObject.activeSelf) 
                AddToScore(1);

            if (isDone == true) ToNextDialogue();

            yield return new WaitForSeconds(delta);
        }
    }
    public static void StartGame()
    {
        
        MusicManager.StopNowPlaying();
        
        switch (IsPractice)
        {
            case true:
                GameSceneManager.LoadScene("STAGE1_GRASSLANDS");
                Instance.SetPlayerLives(6);
                break;

            case false:
                GameSceneManager.LoadScene("STAGE1_GRASSLANDS");
                Instance.SetPlayerLives(3);
                break;
        }

        //At this moment, game has started and player is considered alive
        HasGameStarted = true;
        IsPlayerAlive = HasGameStarted;
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
                    case Dialogue.Script.Voices.Mythril:
                        AudioManager.Play("Type000", _oneShot: true);
                        break;
                    case Dialogue.Script.Voices.Augusta:
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
                Dialogue.Instance.isRunning = Dialogue.IsRunning;
                Dialogue.OnDialogueEnd();
            }
        }
    }

    private void OnLoadedScene(Scene _scene, LoadSceneMode mode)
    {

    }

    public static void UpdateGameDifficulty(int difficultyLevel)
    {
        DifficultyIndex = difficultyLevel;
    }

    public static void UpdateStats(Stats stats)
    {
        CharacterStats = stats;
    }

    public static void UpdateCharacterRAC(RuntimeAnimatorController animatorController)
    {
        CharacterAnimatorController = animatorController;
    }

    public static void UpdatePlayerPawn(PlayerPawn pawn)
    {
        PlayerPawn = pawn;
    }

    public static void UpdateCurrentStage(Stage stage)
    {
        CurrentStage = stage;
    }

    void LockCursor() => Cursor.lockState = CursorLockMode.Locked;

    void DisableMouseVisibility() => Cursor.visible = false;

    void SetRatio(float width, float height)
    {
        if (((float)Screen.width) / ((float)Screen.height) > width / height)
            Screen.SetResolution((int)(((float)Screen.height) * (width/height)), Screen.height, true);
        else
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (height / width)), true);
    }
}