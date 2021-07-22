using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Set a rating between 1 - 5")]
    [Range(MIN_RATING, MAX_RATING)]
    private int ratingValue;

    [SerializeField]
    private Sprite ratingGraphics;

    [SerializeField]
    private Image[] childImages = new Image[MAX_RATING];

    private Sprite nullImage;

    private const int MAX_RATING = 5;
    private const int MIN_RATING = 1;
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

            for (int index = 0; index < MAX_RATING; index++)
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
        ratingValue = (ratingValue > MAX_RATING) ? MAX_RATING : (ratingValue < MIN_RATING) ? MIN_RATING : value;
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
        for (int index = 0; index < MAX_RATING; index++)
        {
            if (childImages != null)
            {
                childImages[index].sprite = nullImage;
            }
        }
    }
}
