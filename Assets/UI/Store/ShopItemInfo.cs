using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfo : MonoBehaviour
{
    // ������ ������ ������
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
    // ������ ������Ʈ
    public void SetUpdateItem()
    {
        ShopItemimage.texture = ShopItem.iteminfo.ITEMICON;
        ShopItemNameTxt.text = ShopItem.iteminfo.Name;
        ShopItemDesTxt.text = ShopItem.iteminfo.Description;
        ShopItemPrice.text = ShopItem.iteminfo.Price.ToString();
    }

    // ������ ���� ��ư
    public void BuyItemButton()
    {
        // ��
        if (playerStatus.PlayerMoney >= ShopItem.iteminfo.Price)
        {
            playerStatus.PlayerMoney -= ShopItem.iteminfo.Price;
            playerStatus.UpdateMonney();
            inventory.pickUpItem(ShopItem, false);
        }
        else if (playerStatus.PlayerMoney < ShopItem.iteminfo.Price) return;
    }
}
