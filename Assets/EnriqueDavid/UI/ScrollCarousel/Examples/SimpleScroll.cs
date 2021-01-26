using EnriqueDavid.UI;
using UnityEngine;

public class SimpleScroll : MonoBehaviour
{ 
    public ScrollCarousel horizontalScrollCarrousel;
    public ScrollCarousel verticalScrollCarrousel;
    public int index = 0;

    /// <summary>
    /// When item is centered, get the RectTransform and index of item
    /// </summary>
    /// <param name="data"></param>
    public void OnIndexChanged(IndexChangedEventData data)
    {
        Debug.Log("OnIndexChanged " + data.index + " " + data.item);
    }

    /// <summary>
    /// When item is centered, get the RectTransform of item
    /// </summary>
    /// <param name="data"></param>
    public void OnSnapRectTransform(SnapEventData data)
    {
        Debug.Log("OnSnapRectTransform " + data.item);
    }

    /// <summary>
    /// Many events of this type are launched, several for each item that contains the scroll. It is really useful to perform some effect on each item according to its scale: fade in / out, color change, rotation, ...
    /// </summary>
    /// <param name="data"></param>
    public void OnScaleEvent(ScaleEventData data)
    {
        //Debug.Log("OnScaleEvent " + data.item + " " + data.scale.ToString("N3"));
    }

    /// <summary>
    /// On click on item snap to it
    /// </summary>
    /// <param name="rt">Recttransform of item clicked</param>
    public void OnClickHorizontalItem(RectTransform rt)
    {
        horizontalScrollCarrousel.SnapTo(rt);
    }

    /// <summary>
    /// On click on item snap to it
    /// </summary>
    /// <param name="rt">Recttransform of item clicked</param>
    public void OnClickVerticalItem(RectTransform rt)
    {
        verticalScrollCarrousel.SnapTo(rt);
    }

    /// <summary>
    /// Snap to next item in scroll
    /// </summary>
    public void OnClickHorizontalNext()
    {
        // Get all items
        RectTransform[] items = horizontalScrollCarrousel.GetItems();

        // Get current item centered
        int currentIndex = horizontalScrollCarrousel.currentItemIndex;
        index = currentIndex;

        // If it is not in the last element, snap to next element
        if (currentIndex < items.Length - 1)
        {
            horizontalScrollCarrousel.SnapTo(currentIndex + 1);
            index++;
        }
    }

    /// <summary>
    /// Snap to prev item in scroll
    /// </summary>
    public void OnClickHorizontalPrev()
    {
        // If it is not in first element, snap to prev element
        int currentIndex = horizontalScrollCarrousel.currentItemIndex;
        index = currentIndex;

        if (currentIndex > 0)
        {
            horizontalScrollCarrousel.SnapTo(currentIndex - 1);
            index--;
        }
    }

    /// <summary>
    /// Snap to next item in scroll
    /// </summary>
    public void OnClickVerticalNext()
    {
        // Get all items
        RectTransform[] items = verticalScrollCarrousel.GetItems();

        // Get current item centered
        int currentIndex = verticalScrollCarrousel.currentItemIndex;

        // If it is not in the last element, snap to next element
        if (currentIndex < items.Length-1)
        {
            verticalScrollCarrousel.SnapTo(currentIndex + 1);
        }
    }

    /// <summary>
    /// Snap to prev item in scroll
    /// </summary>
    public void OnClickVerticalPrev()
    {
        // If it is not in first element, snap to prev element
        int currentIndex = verticalScrollCarrousel.currentItemIndex;
        if (currentIndex > 0)
        {
            verticalScrollCarrousel.SnapTo(currentIndex - 1);
        }
    }

   
}
