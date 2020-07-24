using UnityEngine;

public class LuuEventTimeline : EventTimeline, IEventSetup
{
    private LuuPawn Luu;

    //Create some events
    EventManager.Event @ev_dialogueRun;
    EventManager.Event @ev_dialogueEnd;
    EventManager.Event @ev_sakuraBurst;
    EventManager.Event @ev_sakuraFan;
    EventManager.Event @ev_sakuraDance;
    EventManager.Event @ev_sakuraHanabi;

    //Witch Ritual - Birth of A One Man Sakura Regiment
    EventManager.Event ev_sakuraRitual;

    //This will be a test...
    protected override void MainTimeline()
    {
        //Make sure Entity is LuuPawn
        Luu = Entity as LuuPawn;

        switch (TimelineIndex)
        {
            #region Timeline 0
            //Start of Luu Stage
            case 0:
                Luu.SetBasePriority(3);

                //Add events for Dialouge End
                if (!ev_dialogueRun.HasTriggered())
                {
                    print("Running");
                    //Request for Dialogue Set 0
                    ev_dialogueRun.Trigger();
                }

                //Check if all events with DialogueEnd eventCode has been triggered
                if (ev_dialogueEnd.HasTriggered())
                {
                    ev_dialogueRun.Reset();
                    Next();
                }

                break;
            #endregion

            #region Timeline 1
            //First time losing patience
            case 1:
                if (Luu.HasLostPatience)
                {
                    print("She has lost patience!!!");
                    ev_sakuraFan.Trigger();
                    
                }

                if (ev_sakuraFan.HasTriggered())
                {
                    ev_sakuraFan.Reset();
                    Next();
                }
                break;
            #endregion

            #region TImeline 2
            //First time having health lowered
            case 2:
                if (Luu.HasHealthLowered)
                {
                    print("Got her!!!");
                    Luu.SetMaxHealthValue(2500);
                    Luu.SetPatienceValue(5000);
                    ev_sakuraDance.Trigger();
                    Luu.ResetValues();
                }

                if (ev_sakuraDance.HasTriggered())
                {
                    ev_sakuraDance.Reset();
                    Next();
                }
                break;
            #endregion

            #region Timeline 3
            //Second time having patience lowered
            case 3:
                if (Luu.HasLostPatience)
                {
                    print("She lost patience again!");
                    ev_sakuraHanabi.Trigger();
                    Next();
                }

                if (ev_sakuraHanabi.HasTriggered())
                {
                    ev_sakuraHanabi.Reset();
                    Next();
                }
                break;
            #endregion

            #region Timeline 4
            //Second time having health lowered
            case 4:
                if (Luu.HasHealthLowered)
                {
                    print("Got her again!");
                    Luu.SetMaxHealthValue(5000);
                    Luu.SetPatienceValue(7500);
                    Luu.ResetValues();
                    Next();
                }
                break;
                #endregion
        }
    }

    public override void SetupEvents()
    {
        ev_dialogueEnd = EventManager.AddNewEvent(0, "DialogueEnd",
            () => Luu.OnInitialized(ev_sakuraBurst));

        ev_sakuraBurst = EventManager.AddNewEvent(1, "Sakura Burst",
            () => print("Activate Sakura Burst"),
            () => Luu.ActivateSpell("Sakura Burst"));

        ev_sakuraFan = EventManager.AddNewEvent(2, "Sakura Fan",
            () => print("Activate Sakura Fan"),
            () => Luu.ActivateSpell("Sakura Fan", true));

        ev_sakuraDance = EventManager.AddNewEvent(3, "Sakura Dance",
            () => print("Activate Sakura Dance"),
            () => Luu.ActivateSpell("Sakura Dance", true));

        ev_sakuraHanabi = EventManager.AddNewEvent(4, "Sakura Hanabi",
            () => print("Activate Sakura Hanabi"),
            () => Luu.ActivateSpell("Sakura Hanabi", true));

        ev_sakuraRitual = EventManager.AddNewEvent(6, "Sakura Ritual - One Man Regiment");

        ev_dialogueRun = EventManager.AddNewEvent(5, "DialogueRun",
            () => Dialogue.Instance.Run(0));
    }
}
