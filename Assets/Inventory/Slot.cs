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
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();

        // 아이템에 슬롯이 없으면
        if (ItemInSlot == null)
        {
            // 옮겨진 아이템의 아이콘을 현재 슬롯으로 옮김
            draggableItem.dragItemIcon.SetParent(transform);
            draggableItem.dragItemIcon.localPosition = Vector3.zero;

            // 현재 슬롯의 아이템 데이터 갱신
            ItemInSlot = draggableItem.originItemIcon.GetComponent<Slot>().ItemInSlot;

            // 드래그한 아이템의 아이콘을 초기화
            draggableItem.dragItemIcon = null;
            // 이동한 슬롯의 아이템 데이터 초기화
            draggableItem.originItemIcon.GetComponent<Slot>().ItemInSlot = null;

            Transform emptyRawImage = FindEmptyRawImage(transform);
            if (emptyRawImage != null)
            {
                emptyRawImage.SetParent(draggableItem.originItemIcon);
                emptyRawImage.localPosition = Vector3.zero;
            }
        }
    }
    private Transform FindEmptyRawImage(Transform parent)
    {
        foreach (Transform child in parent)
        {
            RawImage rawImage = child.GetComponent<RawImage>();
            if (rawImage != null && rawImage.texture == null)
            {
                return child;
            }
        }
        return null;
    }
}
