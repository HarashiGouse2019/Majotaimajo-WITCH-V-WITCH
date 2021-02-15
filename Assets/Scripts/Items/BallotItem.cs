//using UnityEngine;
//public class BallotItem : Item
//{
//    public BallotItem()
//    {
//        Name = "Ballot Item";
//        Description = "";
//        Image = Resources.Load<Sprite>(@"ItemImages/Ballot");
//        if(Image == null) { Debug.Log("Resource did not load the image."); }

//        SetupEvents();
//    }
//    public void SetupEvents()
//    {
//        EV_OnUse = EventManager.AddEvent(200, "Increase Magic 50+",
//            () => GameManager.Instance.IncrementMagic(50f));
//    }
//}