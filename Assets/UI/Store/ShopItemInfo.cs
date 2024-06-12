using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfo : MonoBehaviour
{
    // ������ ������ ������
    public ItemData iteminfo;

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
        ShopItemimage.texture = iteminfo.ITEMICON;
        ShopItemNameTxt.text = iteminfo.Name;
        ShopItemDesTxt.text = iteminfo.Description;
        ShopItemPrice.text = iteminfo.Price.ToString();
    }

    // ������ ���� ��ư
    public void BuyItemButton()
    {

    }
}
