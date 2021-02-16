using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : SelectionObject
{
    private CharacterProfile currentProfile;

    [SerializeField]
    private CharacterProfile[] profiles;

    [SerializeField]
    private TextMeshProUGUI characterName;

    [SerializeField]
    private TextMeshProUGUI characterDescription;

    [SerializeField]
    private Image characterPortrait;

    [SerializeField]
    private TextMeshProUGUI characterBackstory;

    [SerializeField]
    private TextMeshProUGUI characterAttribute;

    [SerializeField]
    private Rating characterSpeed, 
        characterPower, 
        characterAnnoyance, 
        characterPriority, 
        characterMagic, 
        characterKnowledge, 
        characterEvasiveness;

    private void OnEnable()
    {
        UpdateCurrentProfile();
        DisplayProfile();
    }

    void NextCharacter()
    {
        selectionIndex++;
        UpdateCurrentProfile();
        DisplayProfile();
    }

    void PreviousCharacter()
    {
        selectionIndex--;
        UpdateCurrentProfile();
        DisplayProfile();
    }

    void OnConfirm()
    {
        GameManager.UpdateCharacterRAC(currentProfile.GetRAC());
        GameManager.UpdateStats(currentProfile.InitStatValues());
        GameManager.StartGame();
    }

    void OnCancel()
    {
        GameSceneManager.LoadScene("DIFFICULTY_SELECTION");
    }

    void UpdateCurrentProfile()
    {
        selectionIndex = (selectionIndex > profiles.Length - 1) ?
        selectionIndex - profiles.Length : (selectionIndex < 0) ?
        selectionIndex + profiles.Length : selectionIndex; 

        currentProfile = profiles[selectionIndex];
    }

    void DisplayProfile()
    {
        characterPortrait.sprite = currentProfile.GetPortrait();

        characterName.text          = currentProfile.GetName();
        characterDescription.text   = currentProfile.GetDescription();
        characterBackstory.text     = currentProfile.GetBackstory();
        Stats.DetermineStatValueRating(18);
        characterAttribute.text     = currentProfile.GetAttribute().            ToString();
        characterSpeed.             SetRating(currentProfile.SpeedRating).      DisplayRating();
        characterPower.             SetRating(currentProfile.PowerRating).      DisplayRating();
        characterAnnoyance.         SetRating(currentProfile.AnnoyanceRating).  DisplayRating();
        characterPriority.          SetRating(currentProfile.PriorityRating).   DisplayRating();
        characterMagic.             SetRating(currentProfile.MagicRating).      DisplayRating();
        characterKnowledge.         SetRating(currentProfile.KnowledgeRating).  DisplayRating();
        characterEvasiveness.       SetRating(currentProfile.EvasivenessRating).DisplayRating();
    }

    public override void SetupEvents()
    {
        _onSelectPrevious   = EventManager.AddEvent(190, "onCharacterSelectPrevious", PreviousCharacter, () => cursorSound.Play());
        _onSelectNext       = EventManager.AddEvent(191, "onCharacterSelectNext"    , NextCharacter, () => cursorSound.Play());
        _onConfirm          = EventManager.AddEvent(192, "onCharacterConfirm"       , OnConfirm, () => confirmSound.Play());
        _onCancel           = EventManager.AddEvent(193, "onCharacterCancel"        , OnCancel, () => cancelSound.Play());
    }
}
