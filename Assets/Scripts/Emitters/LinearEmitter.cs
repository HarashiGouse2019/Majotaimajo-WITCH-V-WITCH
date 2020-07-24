using UnityEngine;

public class LinearEmitter : Emitter
{
    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
