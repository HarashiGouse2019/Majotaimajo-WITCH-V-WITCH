using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureRenderer : MonoBehaviour
{
    public Image[] spirits = new Image[10];

    readonly float texWidth, texHeight;
    readonly float texPosX, texPosY;

    public Canvas canvas;

    public RectTransform rectSize;

    public bool check = true;

    int startLives;

    private void Start()
    {
        startLives = GameManager.Instance.GetLives();
    }
    private void Update()
    {
        
        if (check) UpdateLives();
    }

    // Start is called before the first frame update
    private void UpdateLives()
    {
        check = false;
        for (int i = 0; i < GameManager.Instance.GetLives(); i++)
        {
            spirits[i].gameObject.SetActive(true);
        }

        spirits[startLives - (startLives - GameManager.Instance.GetLives())].gameObject.SetActive(false);
        
        Debug.Log(GameManager.Instance.GetLives());
    }
}
