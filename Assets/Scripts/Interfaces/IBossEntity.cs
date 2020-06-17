public interface IBossEntity
{
    float BossCurrentHealth { get; set; }
    float BossMaxHealth { get; set; }
    float CurrentPatience { get; set; }
    float MaxPatience { get; set; }

    //How fast their patience go down
    //Every 0.001 seconds, patience decreases
    float PatienceDepletionRate { get; set; }

    /// <summary>
    /// Set the total amount of phases that the boss can have
    /// </summary>
    /// <param name="value"></param>
    void SetTotalPhases(int value);

    /// <summary>
    /// Set the amount of patience the boss has
    /// </summary>
    /// <param name="value"></param>
    void SetPatienceValue(int value);

    /// <summary>
    /// Decreases the number you see on the right side of the boss' HP
    /// </summary>
    void DecrementHPLayer();
}
