using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public RectTransform content;

    private float itemHeight;
    private int currentFirstIndex;

    private void Start()
    {
        itemHeight = itemPrefabs[0].GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            CreateItem(i);
        }
    }

    private void Update()
    {
        if (Mathf.Abs(content.anchoredPosition.y - (currentFirstIndex * itemHeight)) > itemHeight / 2)
        {
            UpdateItems();
        }
    }

    private void CreateItem(int index)
    {
        GameObject newItem = Instantiate(itemPrefabs[index % itemPrefabs.Length], content);

        newItem.SetActive(true);
        RectTransform rectTransform = newItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -index * itemHeight);
    }

    private void RemoveItem(int index)
    {
        Destroy(content.GetChild(index).gameObject);
    }

    private void UpdateItems()
    {
        int newIndex = Mathf.FloorToInt(content.anchoredPosition.y / itemHeight);
        int offset = newIndex - currentFirstIndex;

        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                RemoveItem(currentFirstIndex);
                currentFirstIndex++;
            }
        }
        else if (offset < 0)
        {
            for (int i = 0; i > offset; i--)
            {
                currentFirstIndex--;
                CreateItem(currentFirstIndex);
            }
        }
    }
}
