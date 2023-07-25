using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    [SerializeField] private PlayerBag playerBag;
    [SerializeField] private List<GameObject> itemsList;
    private ItemType selectedType;
    private ItemSO selectedItem;
    // Start is called before the first frame update
    void Start()
    {
        selectedType = ItemType.GENERIC_ITEM;
    }

    public void RenderItemsList()
    {
        StringItemsDictionary itemsOfTypeList = playerBag.PlayerItems[selectedType];
        foreach(string itemName in itemsOfTypeList.Keys)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
