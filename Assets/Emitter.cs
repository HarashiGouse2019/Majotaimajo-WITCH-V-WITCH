using UnityEngine;

[RequireComponent(typeof(DanmakuSequencer))]
[RequireComponent(typeof(Shoot_Trig))]
public class Emitter : MonoBehaviour
{
    public int id = 0;
    private DanmakuSequencer sequencer;
    private Shoot_Trig shootTrig;

    private void Awake()
    {
        sequencer = GetComponent<DanmakuSequencer>();
        shootTrig = GetComponent<Shoot_Trig>();
    }

    public DanmakuSequencer GetSequencer() => sequencer;
    public Shoot_Trig GetShootTrig() => shootTrig;
}
