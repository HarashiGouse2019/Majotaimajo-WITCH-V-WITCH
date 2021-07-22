using System.Collections;
using System.IO;
using UnityEngine;

/// <summary>
/// This mostly pertains to bosses.
/// We're able to specify what happens when a boss is at
/// a certain value, and also trigger a set of events
/// when their patience are out or when their HP is low.
/// </summary>
/// 
//If you see this, delete the other one
public class PawnProgrammable : Pawn
{
    public static bool IsRunning { get; private set; } = true;

    /// <summary>
    /// The main function used for all boss actions
    /// and routines.
    /// </summary>
    public virtual void Main() { }

    /// <summary>
    /// The actual routine for main
    /// </summary>
    /// <returns></returns>
    IEnumerator BossRoutine() {
        while (true)
        {
            try
            {
                if (IsRunning) Main();
            }
            catch (IOException e)
            {
                Debug.LogException(e);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Stop the main cycle
    /// </summary>
    public static void StopProcess()
    {
        IsRunning = false;
    }

    /// <summary>
    /// Resume the main cycle
    /// </summary>
    public static void ResumeProcess()
    {
        IsRunning = true;
    }
}
