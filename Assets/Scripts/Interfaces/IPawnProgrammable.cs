using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This mostly pertains to bosses.
/// We're able to specify what happens when a boss is at
/// a certain value, and also trigger a set of events
/// when their patience are out or when their HP is low.
/// </summary>
public interface IPawnProgrammable
{
    /// <summary>
    /// The main function used for all boss actions
    /// and routines.
    /// </summary>
    void Main();

    /// <summary>
    /// The actual routine for main
    /// </summary>
    /// <returns></returns>
    IEnumerator BossRoutine();
}
