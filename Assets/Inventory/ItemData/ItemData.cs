using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Create new Item")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string description;
    [SerializeField] private int maxStack;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Texture itemIcon;

    public Texture ITEMICON => itemIcon;
    public int ID => _id;
    public int MAXSTACK => maxStack;
}
