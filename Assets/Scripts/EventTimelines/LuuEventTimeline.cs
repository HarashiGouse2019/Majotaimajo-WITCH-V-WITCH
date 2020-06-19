using System.Diagnostics;
using UnityEngine;

public class LuuEventTimeline : EventTimeline
{
    private LuuPawn Luu;
    string testString = "";

    //Create some events
    EventManager.Event ev_dialogueEnd = EventManager.AddNewEvent(0, "DialogueEnd", null);
    EventManager.Event ev_patternChange = EventManager.AddNewEvent(1, "PatternChange", null);

    //This will be a test...
    protected override void MainTimeline()
    {
        if (string.IsNullOrEmpty(testString))
        {
            //Fjdkoaso
        }
        Luu = Entity as LuuPawn;

        switch (TimelineIndex)
        {
            //Start of Luu Stage
            case 0:
                //Add events for Dialouge End
                if (!Dialogue.IsRunning) Dialogue.Instance.Run(0);


                ev_dialogueEnd.AddNewListener(Luu.OnInitialized);

                //Check if all events with DialogueEnd eventCode has been triggered
                if (ev_dialogueEnd.HasTriggered())
                {
                    ev_dialogueEnd.RemoveListener(Luu.OnInitialized);
                    Next();
                }

                break;

            //First time losing patience
            case 1:
                if (Luu.HasLostPatience)
                {
                    print("She has lost patience!!!");
                    Next();
                }

                break;

            //First time having health lowered
            case 2:
                if (Luu.HasHealthLowered)
                {
                    print("Got her!!!");
                    Luu.SetMaxHealthValue(5000);
                    Luu.SetPatienceValue(10000);
                    Luu.ResetValues();
                    Next();
                }
                break;

            case 3:
                if (Luu.HasHealthLowered)
                {
                    print("Got her again!");
                    Luu.SetMaxHealthValue(10000);
                    Luu.SetPatienceValue(15000);
                    Luu.ResetValues();
                    Next();
                }
                break;
        }
    }
}
