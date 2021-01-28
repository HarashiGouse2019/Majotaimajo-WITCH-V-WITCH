using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Bank", menuName = "Sprite Bank")]
public class SpriteBank : ScriptableObject
{
    [SerializeField]
    private SpriteData[] spriteData;
}