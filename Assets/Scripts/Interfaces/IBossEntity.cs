public interface IBossEntity
{

    int UniqueIdentifier { get; set; }
    float BossCurrentHealth { get; set; }
    float BossMaxHealth { get; set; }
    float CurrentPatience { get; set; }
    float MaxPatience { get; set; }

    bool IsActive { get; set; }

    int HPLayer { get; set; }

    bool HasLostPatience { get; set; }

    bool HasHealthLowered { get; set; }

    //How fast their patience go down
    //Every 0.001 seconds, patience decreases
    float PatienceDepletionRate { get; set; }

    /// <summary>
    /// Set the total amount of phases that the boss can have
    /// </summary>
    /// <param name="value"></param>
    void SetHealthLayer(int value);

    /// <summary>
    /// Set the amount of patience the boss has
    /// </summary>
    /// <param name="value"></param>
    void SetPatienceValue(float value, bool addTo = false);

    /// <summary>
    /// Set the Max Patience of the boss
    /// </summary>
    /// <param name="value"></param>
    void SetMaxPatienceValue(int value);

    /// <summary>
    /// Set the boss' health value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="addTo"></param>
    void SetHealthValue(float value, bool addTo = false);

    /// <summary>
    /// Set the boss' max health value
    /// </summary>
    /// <param name="value"></param>
    void SetMaxHealthValue(int value);

    /// <summary>
    /// Decreases the number you see on the right side of the boss' HP
    /// </summary>
    void DecrementHPLayer();

    /// <summary>
    /// Set the boss active or not
    /// </summary>
    /// <param name="active"></param>
    void SetActive(bool active);
}
