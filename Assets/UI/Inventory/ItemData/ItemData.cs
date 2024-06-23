using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
};
public enum ItemType
{
    Equipment,
    Consumable,
    Misecellaneous
}

public class ItemData : ScriptableObject
{
    [Header("Item serial number")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Rarity rarity;
    [SerializeField] private ItemType itemtype;

    [Header("Item Desctiption")]
    [TextArea(3, 3)]
    [SerializeField] private string description;
    [SerializeField] private int maxStack;
    [SerializeField] private int price;
    [SerializeField] private float weight;
    [SerializeField] private float durability;

    [Header("Item Info")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Texture itemIcon;

    public Texture ITEMICON => itemIcon;
    public int ID => _id;
    public string Name => _name;
    public int MAXSTACK => maxStack;
    public float Weight => weight;
    public int Price => price;
    public Rarity RARITY => rarity;
    public ItemType ITEMTYPE => itemtype;
    public float Durability => durability;
    public string Description => description;
}
