using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfo : MonoBehaviour
{
    // 아이템 정보를 가져옴
    [Header("Item Info")]
    public ItemInfo ShopItem;
    public Inventory inventory;
    public PlayerStatus playerStatus;

    [Header("Shop Item Info")]
    [SerializeField] RawImage ShopItemimage;
    [SerializeField] TextMeshProUGUI ShopItemNameTxt;
    [SerializeField] TextMeshProUGUI ShopItemDesTxt;
    [SerializeField] TextMeshProUGUI ShopItemPrice;
    public void SetShop()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SetUpdateItem();
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    // 아이템 업데이트
    public void SetUpdateItem()
    {
        ShopItemimage.texture = ShopItem.iteminfo.ITEMICON;
        ShopItemNameTxt.text = ShopItem.iteminfo.Name;
        ShopItemDesTxt.text = ShopItem.iteminfo.Description;
        ShopItemPrice.text = ShopItem.iteminfo.Price.ToString();
    }

    // 아이템 구입 버튼
    public void BuyItemButton()
    {
        // 비교
        if (playerStatus.PlayerMoney >= ShopItem.iteminfo.Price)
        {
            playerStatus.PlayerMoney -= ShopItem.iteminfo.Price;
            playerStatus.UpdateMonney();
            inventory.pickUpItem(ShopItem, false);
        }
        else if (playerStatus.PlayerMoney < ShopItem.iteminfo.Price) return;
    }
}
