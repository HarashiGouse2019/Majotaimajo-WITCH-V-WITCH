using System.Diagnostics;
using UnityEngine;

public class LuuEventTimeline : EventTimeline
{
    private LuuPawn Luu;

    //Create some events
    EventManager.Event ev_dialogueEnd = EventManager.AddNewEvent(0, "DialogueEnd", null);
    EventManager.Event ev_patternChange = EventManager.AddNewEvent(1, "PatternChange", null);

    //This will be a test...
    protected override void MainTimeline()
    {
        Luu = Entity as LuuPawn;

        switch (TimelineIndex)
        {
            //Start of Luu Stage
            case 0:
                //Add events for Dialouge End
                if (!Dialogue.IsRunning) Dialogue.Instance.Run(0);


                ev_dialogueEnd.AddNewListener(Luu.OnInitialized);

                //Check if all events with DialogueEnd eventCode has been triggered
                if (EventManager.FindEventsOfEventCode("DialogueEnd").HaveAllTriggered())
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
                    Next();
                }
                break;
        }
    }
}
