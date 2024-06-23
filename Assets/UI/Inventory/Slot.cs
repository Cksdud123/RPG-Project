using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [Header("Item Info")]
    public ItemData ItemInSlot;
    public int AmountInSlot;
    [SerializeField] private DropItem dropItems;

    [HideInInspector] public RawImage icon;
    [HideInInspector] public TextMeshProUGUI txt_amount;

    // 슬롯 비활성화
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

        UpdateSlot();
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
            txt_amount.text = "0";
            txt_amount.enabled = false;
        }
    }
    // 슬롯 초기화
    public void ResetSlot()
    {
        ItemInSlot = null;
        AmountInSlot = 0;

        // 아이콘과 텍스트 비활성화
        if (icon != null || txt_amount != null)
        {
            icon.gameObject.SetActive(false);
            icon.texture = null;

            txt_amount.gameObject.SetActive(false);
            txt_amount.text = AmountInSlot.ToString();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();

        if (draggableItem == null || draggableItem.originParent == null) return;

        Slot slot = draggableItem.originParent.GetComponent<Slot>();

        // 현재 슬롯이 EquipmentPanel에 있고 드래그된 아이템이 Equipment 타입이 아닌 경우 리턴
        if (transform.parent.name == "EquipmentPanel")
        {
            // ItemType이 Equipment이 아닌 경우 리턴
            if (slot.ItemInSlot.ITEMTYPE != ItemType.Equipment) return;

            // 현재 장비의 타입과 태그가 불일치하면 리턴
            EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;
            if (equipmentData.equipmentType.ToString() != transform.tag) return;
        }

        // 현재 아이템 슬롯이 비었을때
        if (ItemInSlot == null)
        {
            // 쉬프트 모드일 때
            if (draggableItem.ShihtMode) HalfItemAmount(slot);
            // 일반 모드 일때
            else ChangeEmptySlot(slot);
        }
        // 현재 아이템 슬롯이 있고 아이디가 다를때
        else if(ItemInSlot != null && ItemInSlot.ID != slot.ItemInSlot.ID) SwapItems(slot);
        // 현재 아이템 슬롯이 있고 아이디가 같을때
        else if(ItemInSlot != null && ItemInSlot.ID == slot.ItemInSlot.ID) AddItems(slot);
    }
    // 아이템을 빈 슬롯으로 가져다 놓음
    public void ChangeEmptySlot(Slot slot)
    {
        // 현재 슬롯의 아이템 데이터 갱신
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        SetSlot();
        slot.ResetSlot();
    }
    // 아이템의 수량을 반만 이동시킴
    private void HalfItemAmount(Slot slot)
    {
        // 현재 수량이 1이면 리턴
        if (slot.AmountInSlot == 1) return;

        // 현재 슬롯의 아이템 데이터 갱신
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = Mathf.CeilToInt(slot.AmountInSlot / 2f);
        SetSlot();

        // 원래 슬롯의 아이템 데이터 갱신
        slot.AmountInSlot -= AmountInSlot;
        slot.UpdateSlot();
    }
    // 두 아이템을 교체
    public void SwapItems(Slot slot)
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
    }
    // 아이디가 같을때 더함
    public void AddItems(Slot slot)
    {
        // 두개의 슬롯에 있는 아이템이 전부 최대 수량일때
        if(AmountInSlot == ItemInSlot.MAXSTACK && slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) return;
        // 두개의 슬롯에 있는 아이템중 하나라도 최대 수량일때
        if(AmountInSlot == ItemInSlot.MAXSTACK || slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) SwapItems(slot);
        // 두개의 슬롯에 있는 아이템을 더했을때 최대 수량을 넘기지 않을때
        if(AmountInSlot + slot.AmountInSlot <= ItemInSlot.MAXSTACK)
        {
            // 현재 슬롯에 수량을 더한 뒤
            AmountInSlot += slot.AmountInSlot;
            UpdateSlot();

            slot.ResetSlot();
        }
        // 두개의 슬롯에 있는 아이템을 더했을때 최대 수량을 넘길때
        else if (AmountInSlot + slot.AmountInSlot >= ItemInSlot.MAXSTACK)
        {
            // 모두합친 수량을 구한 뒤
            int overStack = AmountInSlot + slot.AmountInSlot;

            // 현재 슬롯의 수량을 최대로
            AmountInSlot = ItemInSlot.MAXSTACK;
            UpdateSlot();
            // 원래 슬롯엔 남은 수량을 더함
            slot.AmountInSlot = overStack - AmountInSlot;
            slot.UpdateSlot();
        }
    }
    // 아이템을 삭제
    public void DropItems(Slot slot)
    {
        // 0개면 리턴
        if (slot.AmountInSlot == 0) return;

        // 버리는 값이 현재 스택값과 같다면
        if (dropItems.dropAmount == slot.AmountInSlot) slot.ResetSlot();

        // 최대스택이 아니라면
        else if (dropItems.dropAmount <= slot.ItemInSlot.MAXSTACK)
        {
            // 원래 슬롯의 아이템 데이터 초기화
            slot.AmountInSlot -= dropItems.dropAmount;

            // DropItem의 Slider 최대 갯수를 줄임
            dropItems.DropCountSlider.maxValue = slot.AmountInSlot;

            // 원래 슬롯의 자식객체에 있는 텍스트 정보 초기화
            slot.txt_amount.text = slot.AmountInSlot.ToString();

            slot.UpdateSlot();
        }
    }

    public RawImage SellIcon => icon;
    public TextMeshProUGUI SellAmount => txt_amount;
}
