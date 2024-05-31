using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData ItemInSlot;
    public int AmountInSlot;

    RawImage icon;
    TextMeshProUGUI txt_amount;

    public void initSlot()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void AddSlot()
    {
        // Slot의 자식객체를 활성화 한 뒤
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // 자식 객체의 컴포넌트를 참조
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        // 인자값으로 받은 아이템의 정보를 설정
        icon.texture = ItemInSlot.ITEMICON;
        txt_amount.text = $"{AmountInSlot}";
    }
    public void UpdateSlot()
    {
        // 자식 객체의 컴포넌트를 참조
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        // 인자값으로 받은 아이템의 정보를 설정
        icon.texture = ItemInSlot.ITEMICON;
        txt_amount.text = $"{AmountInSlot}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragSlot dragSlot = eventData.pointerDrag.GetComponent<DragSlot>();

        // 드래그할 
    }
}
