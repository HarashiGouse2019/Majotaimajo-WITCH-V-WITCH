using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Set a rating between 1 - 5")]
    [Range(minRating, maxRating)]
    private int ratingValue;

    [SerializeField]
    private Sprite ratingGraphics;

    [SerializeField]
    private Image[] childImages = new Image[maxRating];

    private Sprite nullImage;

    private const int maxRating = 5;
    private const int minRating = 1;
    private bool initialized = false;

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        if (initialized == false)
        {
            nullImage = Resources.Load<Sprite>("NullImage");

            childImages = GetComponentsInChildren<Image>();

            for (int index = 0; index < maxRating; index++)
            {
                if (childImages != null)
                {
                    childImages[index].sprite = nullImage;
                }
            }

            initialized = true;
        }
    }

    public Rating SetRating(int value)
    {
        ratingValue = (ratingValue > maxRating) ? maxRating : (ratingValue < minRating) ? minRating : value;
        return this;
    }

    public void DisplayRating()
    {
        if (!initialized) return;
        Flush();
        for(int index = 0; index < ratingValue; index++)
        {
            childImages[index].sprite = ratingGraphics;
        }
    }

    void Flush()
    {
        if (!initialized) return;
        for (int index = 0; index < maxRating; index++)
        {
            if (childImages != null)
            {
                childImages[index].sprite = nullImage;
            }
        }
    }
}
