using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private StringItemsDictionary playerItems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public StringItemsDictionary PlayerItems => playerItems;

    public void AddItemToBag(ItemSO itemSO)
    {
        if (!playerItems.ContainsKey(itemSO.ItemType))
        {
            playerItems.Add(itemSO.ItemType, new List<ItemSO>());
        }
        playerItems[itemSO.ItemType].Add(itemSO);

        // Debug.Log("Current player's bag:");

        // foreach(ItemType itemType in playerItems.Keys)
        // {
        //     foreach(string itemName in playerItems[itemType].Keys)
        //     {
        //         foreach(ItemSO item in playerItems[itemType][itemName])
        //         {
        //             Debug.Log(item.ItemName);
        //         }
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
