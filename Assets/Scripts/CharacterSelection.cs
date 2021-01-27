using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Extensions;

public class CharacterSelection : SelectionObject
{
    private int _characterIndex = 0;
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
        _characterIndex++;
        UpdateCurrentProfile();
        DisplayProfile();
    }

    void PreviousCharacter()
    {
        _characterIndex--;
        UpdateCurrentProfile();
        DisplayProfile();
    }

    void OnConfirm()
    {
        GameManager.UpdateGamePlayer(_characterIndex);
        Debug.Log($"You've selected to play as {profiles[_characterIndex].GetName()}");
        GameSceneManager.LoadScene("STAGE1_GRASSLANDS");
    }

    void OnCancel()
    {
        Debug.Log("Cancelled Character");
        GameSceneManager.LoadScene("DIFFICULTY_SELECTION");
        
    }

    void UpdateCurrentProfile()
    {
        _characterIndex = (_characterIndex > profiles.Length - 1) ?
            _characterIndex - profiles.Length : (_characterIndex < 0) ?
            _characterIndex + profiles.Length : _characterIndex; 

        currentProfile = profiles[_characterIndex];
    }

    void DisplayProfile()
    {

        characterName.text = currentProfile.GetName();
        characterDescription.text = currentProfile.GetDescription();

        characterPortrait.sprite = currentProfile.GetPortrait();

        characterBackstory.text = currentProfile.GetBackstory();

        characterAttribute.text = currentProfile.GetAttribute().ToString();

        characterSpeed.SetRating(currentProfile.GetSpeed()).DisplayRating();
        characterPower.SetRating(currentProfile.GetPower()).DisplayRating();
        characterAnnoyance.SetRating(currentProfile.GetAnnoyance()).DisplayRating();
        characterPriority.SetRating(currentProfile.GetPriority()).DisplayRating();
        characterMagic.SetRating(currentProfile.GetMagic()).DisplayRating();
        characterKnowledge.SetRating(currentProfile.GetKnowledge()).DisplayRating();
        characterEvasiveness.SetRating(currentProfile.GetEvasiveness()).DisplayRating();
    }

    public override void SetupEvents()
    {
        _onSelectPrevious = EventManager.AddEvent(190, "onCharacterSelectPrevious", PreviousCharacter, () => cursorSound.Play());
        _onSelectNext = EventManager.AddEvent(191, "onCharacterSelectNext", NextCharacter, () => cursorSound.Play());
        _onConfirm = EventManager.AddEvent(192, "onCharacterConfirm", OnConfirm, () => confirmSound.Play());
        _onCancel = EventManager.AddEvent(193, "onCharacterCancel", OnCancel, () => cancelSound.Play());
    }
}
