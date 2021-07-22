using SequenceEventUtility;
using UnityEngine;

public class Stage1_SES_Easy : SequenceEventScript
{
    Stage1_SES_Easy()
    {
        __EVENTS__ = new StageEvent[]
        {
            new StageEvent(
                0,
                StateInstructions.PLAY_MUSIC,
                "Search for Lost Ones"
            ),

            new StageEvent(
                25,
                StateInstructions.CALL_EVENT,
                "PLAY_MUSIC"
            ),

            new StageEvent(
                50,
                StateInstructions.NONE
            ),

            new StageEvent(
                75,
                StateInstructions.INITIATE_MINI_BOSS,
                100,
                500
            )
        };
    }
}
