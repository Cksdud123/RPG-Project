using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UpdateStoreItem : MonoBehaviour
{
    public ShopItemInfo[] shopItemInfo = new ShopItemInfo[8];

    private void Awake()
    {
        for (int i = 0; i < shopItemInfo.Length; i++)
        {
            shopItemInfo[i].SetShop();
        }
    }
}
