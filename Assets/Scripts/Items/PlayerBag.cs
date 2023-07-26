using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ItemType, StringItemsDictionary> playerItems;
    // Start is called before the first frame update
    void Start()
    {
        playerItems = new SerializableDictionary<ItemType, StringItemsDictionary>();
    }

    public SerializableDictionary<ItemType, StringItemsDictionary> PlayerItems => playerItems;

    public void AddItemToBag(ItemSO itemSO)
    {
        if (!playerItems.ContainsKey(itemSO.ItemType))
        {
            playerItems.Add(itemSO.ItemType, new Dictionary<string, List<ItemSO>>());
        }
        StringItemsDictionary itemsOfType = playerItems[itemSO.ItemType];
        if (!itemsOfType.ContainsKey(itemSO.ItemName))
        {
            itemsOfType.Add(itemSO.ItemName, new List<ItemSO>());
        }
        itemsOfType[itemSO.ItemName].Add(itemSO);
        playerItems[itemSO.ItemType] = itemsOfType;

        Debug.Log("Current player's bag:");

        foreach(ItemType itemType in playerItems.Keys)
        {
            foreach(string itemName in playerItems[itemType].Keys)
            {
                foreach(ItemSO item in playerItems[itemType][itemName])
                {
                    Debug.Log(item.ItemName);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}