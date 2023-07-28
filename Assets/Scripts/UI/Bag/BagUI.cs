using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    [SerializeField] private PlayerBag playerBag;
    [SerializeField] private List<GameObject> itemsList;
    [SerializeField] private Sprite selectedItemImage;
    [SerializeField] private Sprite unselectedItemImage;
    private ItemType selectedType;
    private ItemSO selectedItem;
    private Dictionary<ItemType, int> cursorsByItemType = new Dictionary<ItemType, int>();
    // Start is called before the first frame update
    void Start()
    {
        selectedType = ItemType.GENERIC_ITEM;
        InitializeCursors();
        RenderItemsList();
    }

    private void InitializeCursors()
    {
        foreach(ItemType itemType in playerBag.PlayerItems.Keys)
        {
            cursorsByItemType[itemType] = 0;
        }

    }

    public void RenderItemsList()
    {
        List<ItemSO> itemsOfTypeList = playerBag.PlayerItems[selectedType];
        int i = 0;
        if (cursorsByItemType[selectedType] >= itemsList.Count - 1)
        {
            i = cursorsByItemType[selectedType];
        }
        
        Debug.Log("Cursor: " + cursorsByItemType[selectedType]);
        Debug.Log("i: " + i);
        List<ItemSO> subListOfItems = new List<ItemSO>();
        if (i == 0)
        {
            subListOfItems = itemsOfTypeList.GetRange(i, itemsList.Count);
        }
        else if (i < itemsOfTypeList.Count - 1)
        {
            subListOfItems = itemsOfTypeList.GetRange((i + 1) - (itemsList.Count - 1), itemsList.Count);
        }
        else
        {
            subListOfItems = itemsOfTypeList.GetRange(i - itemsList.Count, itemsList.Count);
        }
        for(int itemIndex = 0; itemIndex < itemsList.Count; itemIndex++)
        {
            ItemSO currentItem = subListOfItems[itemIndex];
            GameObject itemInfoContainer = itemsList[itemIndex];
            if (Mathf.Min(cursorsByItemType[selectedType], subListOfItems.Count - 2) == itemIndex)
            {
                itemsList[itemIndex].GetComponent<Image>().sprite = selectedItemImage;
            }
            else
            {
                itemsList[itemIndex].GetComponent<Image>().sprite = unselectedItemImage;
            }
            itemsList[itemIndex].GetComponentInChildren<TextMeshProUGUI>().text = currentItem.ItemName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursorsByItemType[selectedType] += 1;
            if (cursorsByItemType[selectedType] >= playerBag.PlayerItems[selectedType].Count)
            {
                cursorsByItemType[selectedType] = playerBag.PlayerItems[selectedType].Count - 1;
            }
            RenderItemsList();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            cursorsByItemType[selectedType] -= 1;
            if (cursorsByItemType[selectedType] <= 0)
            {
                cursorsByItemType[selectedType] = 0;
            }
            RenderItemsList();
        }
    }
}
