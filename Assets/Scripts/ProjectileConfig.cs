using UnityEngine;

public enum ProjectileMotion
{
    Linear,
    EaseIn,
    EaseOut
}

[CreateAssetMenu(fileName = "New Projectile Config", menuName = "Projectile Config")]
public class ProjectileConfig : ScriptableObject
{
    public Sprite projectileGraphic;
    public Emitter emitter;
    public GraphicAnimation animation;
    public ProjectileMotion motion;
 
}
