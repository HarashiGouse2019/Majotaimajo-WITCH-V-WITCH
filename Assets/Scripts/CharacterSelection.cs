using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void ConfirmCharacter()
    {
        GameManager.UpdateGamePlayer(_characterIndex);
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
        _onSelectPrevious = EventManager.AddNewEvent(190, "onCharacterSelectPrevious", PreviousCharacter);
        _onSelectNext = EventManager.AddNewEvent(191, "onCharacterSelectNext", NextCharacter);
        _onConfirm = EventManager.AddNewEvent(192, "onCharacterConfirm", ConfirmCharacter);
    }
}
