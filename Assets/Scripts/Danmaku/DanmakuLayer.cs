using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuLayer : MonoBehaviour
{
    /*This will take in all layers of Emitters (in which has the Danmaku Sequencer and Shoot_Trig)*/

    public List<Emitter> emitters = new List<Emitter>();

    public List<DanmakuSequencer> sequencers { get; private set; } = new List<DanmakuSequencer>();

    public DanmakuSequencer[] GrabAllDanmakuSequencers()
    {
        foreach(Emitter emitter in emitters)
        {
            //There should be a Danmaku Sequencer for all Emitters
            DanmakuSequencer newSequencer = emitter.GetComponent<DanmakuSequencer>();
            sequencers.Add(newSequencer);
        }

        return sequencers.ToArray();
    }

    public void AddEmitterToLayer(Emitter emitter) => emitters.Add(emitter);
    public void ClearLayer() => emitters.Clear();
}
