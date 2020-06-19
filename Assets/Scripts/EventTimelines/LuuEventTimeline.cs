using System.Diagnostics;

public class LuuEventTimeline : EventTimeline
{
    private LuuPawn Luu;

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

                EventManager.Event startEvent = EventManager.AddNewEvent(0, "DialogueEnd", Luu.OnInitialized);
                print(startEvent.HasTriggered());
                if (startEvent.HasTriggered())
                {
                    EventManager.RemoveEvent(0);
                    Next();
                }
                break;

            //First time losing patience
            case 1:
                print("If you see this, we gots a problem...");
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
