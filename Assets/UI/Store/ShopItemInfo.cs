using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfo : MonoBehaviour
{
    // 아이템 정보를 가져옴
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
    // 아이템 업데이트
    public void SetUpdateItem()
    {
        ShopItemimage.texture = iteminfo.ITEMICON;
        ShopItemNameTxt.text = iteminfo.Name;
        ShopItemDesTxt.text = iteminfo.Description;
        ShopItemPrice.text = iteminfo.Price.ToString();
    }

    // 아이템 구입 버튼
    public void BuyItemButton()
    {

    }
}
