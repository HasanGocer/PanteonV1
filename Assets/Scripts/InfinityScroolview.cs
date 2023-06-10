using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinityScroolview : MonoBehaviour
{
    public GameObject[] items;
    public RectTransform content;

    private void Start()
    {
        ResizeContent();
    }

    private void ResizeContent()
    {
        float itemSize = items[0].GetComponent<RectTransform>().rect.width; 
        float spacing = content.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing; 

        float contentSize = (itemSize + spacing) * items.Length; 
        content.sizeDelta = new Vector2(contentSize, content.sizeDelta.y);
    }

}
