using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "PokemonSO/Item", order = 2)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite itemImage;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public ItemType ItemType => itemType;
    public Sprite ItemImage => itemImage;
}
