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

    [HideInInspector] public Inventory inventory;

    private void Start()
    {
        inventory = GetComponentInParent<Inventory>();
    }
    // 슬롯 초기화
    public void initSlot()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // 슬롯 설정
    public void SetSlot()
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
        txt_amount.enabled = true;
    }
    // 슬롯 업데이트
    public void UpdateSlot()
    {
        if (ItemInSlot != null)
        {
            icon.texture = ItemInSlot.ITEMICON;
            txt_amount.text = $"{AmountInSlot}";
            txt_amount.enabled = true;
        }
        else
        {
            icon.texture = null;
            txt_amount.text = "";
            txt_amount.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();
        Slot slot = draggableItem.originParent.GetComponent<Slot>();

        // 현재 아이템 슬롯이 비었을때
        if (ItemInSlot == null)
        {
            ChangeEmptySlot(draggableItem, slot);
        }
        // 현재 아이템 슬롯이 있고 아이디가 다를때
        else if(ItemInSlot != null && ItemInSlot.ID != slot.ItemInSlot.ID)
        {
            SwapItems(draggableItem, slot);
        }
    }
    // 아이템을 빈 슬롯으로 가져다 놓음
    private void ChangeEmptySlot(DragSlot draggableItem, Slot slot)
    {
        // 옮겨진 아이템의 아이콘을 현재 슬롯으로 옮김
        draggableItem.dragItemIcon.SetParent(transform);
        draggableItem.dragItemIcon.SetSiblingIndex(0);
        draggableItem.dragItemIcon.localPosition = Vector3.zero;

        // 현재 슬롯의 아이템 데이터 갱신
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        // 원래 슬롯의 아이템 데이터 초기화
        slot.ItemInSlot = null;
        slot.AmountInSlot = 0;

        // 원래 슬롯의 자식객체에 있는 텍스트 정보 초기화
        draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().text = slot.AmountInSlot.ToString();
        draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);

        // RawImage의 값이 null인 객체를 찾아서 변경
        Transform emptyRawImage = FindEmptyRawImage(transform);
        emptyRawImage.SetParent(draggableItem.originParent);
        emptyRawImage.SetSiblingIndex(0);
        emptyRawImage.localPosition = Vector3.zero;

        SetSlot();

        // 드롭이 완료된 후 드래그 아이템을 초기화
        draggableItem.dragItemIcon = null;
        draggableItem.dragItemAmount = null;
    }
    // 두 아이템을 스왑함
    private void SwapItems(DragSlot draggableItem, Slot slot)
    {
        // 현재 슬롯의 아이템 데이터를 임시 변수에 저장
        ItemData tempItem = ItemInSlot;
        int tempAmount = AmountInSlot;

        // 현재 슬롯에 드래그한 아이템 데이터를 할당
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;
        UpdateSlot();

        // 드래그한 슬롯에 임시 저장된 아이템 데이터를 할당
        slot.ItemInSlot = tempItem;
        slot.AmountInSlot = tempAmount;
        slot.UpdateSlot();

        SetSlot();
    }
    // 자식객체중 RawImage를 찾음
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