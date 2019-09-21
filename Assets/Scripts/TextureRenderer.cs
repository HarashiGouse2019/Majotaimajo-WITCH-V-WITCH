using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRenderer : MonoBehaviour
{
    public Texture spirits;

    float texWidth, texHeight;
    float texPosX, texPosY;

    RectTransform rectSize;

    // Start is called before the first frame update
    void Awake()
    {
        rectSize = GetComponent<RectTransform>();

        texWidth = spirits.width;
        texHeight = spirits.height;
        texPosX = rectSize.localPosition.x;
        texPosY = rectSize.localPosition.y;
    }

    void OnGUI()
    {
        if (GameManager.Instance.GetLives() > 0)
        {
            var posRect = new Rect(texPosX + 612, texPosY + 300, texWidth / 5f * GameManager.Instance.GetLives(), texHeight);
            var texRect = new Rect(0, 0, 1f / 5f * GameManager.Instance.GetLives(), 1f);
            GUI.DrawTextureWithTexCoords(posRect, spirits, texRect);
        }
        Debug.Log(GameManager.Instance.GetLives());
    }
}
