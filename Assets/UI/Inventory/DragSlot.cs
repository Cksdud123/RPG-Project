using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Tooltip;
    [SerializeField] private GameObject EquipTooltip;

    public Transform dragItemIcon;
    public Transform dragItemAmount;

    [HideInInspector] public Transform canvas;
    [HideInInspector] public Transform originParent;

    private int originalSiblingIndex;

    private bool isShiftMode = false; // 쉬프트 모드 여부

    GameObject Helmet, Weapon, Shield, BodyPlate, Pants, Shoes;
    Slot HelmetSlot, WeaponSlot, ShieldSlot, BodyPlateSlot, PantsSlot, ShoesSlot;

    GameObject Portion;
    Slot PortionSlot;

    [HideInInspector] public GameObject inventory;
    [HideInInspector] public Inventory inventoryPanel;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("InventoryCanvas").transform;
    }
    private void InitializeEquipmentSlots()
    {
        Helmet = GameObject.FindGameObjectWithTag("Helmet");
        Weapon = GameObject.FindGameObjectWithTag("Weapon");
        Shield = GameObject.FindGameObjectWithTag("Shield");
        BodyPlate = GameObject.FindGameObjectWithTag("BodyPlate");
        Pants = GameObject.FindGameObjectWithTag("Pants");
        Shoes = GameObject.FindGameObjectWithTag("Shoes");

        if (Helmet != null) HelmetSlot = Helmet.GetComponent<Slot>();
        if (Weapon != null) WeaponSlot = Weapon.GetComponent<Slot>();
        if (Shield != null) ShieldSlot = Shield.GetComponent<Slot>();
        if (BodyPlate != null) BodyPlateSlot = BodyPlate.GetComponent<Slot>();
        if (Pants != null) PantsSlot = Pants.GetComponent<Slot>();
        if (Shoes != null) ShoesSlot = Shoes.GetComponent<Slot>();
    }
    private void InitializeQuickSlots()
    {
        Portion = GameObject.FindGameObjectWithTag("Portion");

        if(Portion != null) PortionSlot = Portion.GetComponent<Slot>();
    }
    // 드래그를 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 슬롯에 저장된 아이템이 없다면 리턴
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;
        // 우클릭을 한채 누르면 수량을 반만 가져가게 해놓음
        if (eventData.button == PointerEventData.InputButton.Right) isShiftMode = true;
        else isShiftMode = false;

        // 드래그할 오브젝트의 정보를 가져옴
        dragItemIcon = transform.GetComponentInChildren<RawImage>().transform;
        dragItemIcon.GetComponent<RawImage>().raycastTarget = false;

        // 원래 정보를 저장
        originParent = transform;
        originalSiblingIndex = dragItemIcon.GetSiblingIndex();

        // 텍스트 정보는 드래그 중에 비활성화
        dragItemAmount = transform;
        dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        if (dragItemIcon != null)
        {
            dragItemIcon.SetParent(canvas);
            dragItemIcon.SetAsLastSibling();
        }
    }

    // 드래그 중일 때 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null)
        {
            dragItemIcon.transform.position = eventData.position;
        }
    }

    // 드래그가 끝날 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null)
        {
            // 드래그가 끝난뒤에 부모가 캔버스로 설정되어 있었다면 
            if (dragItemIcon.parent == canvas)
            {
                // 드래그한 아이템의 부모를 드래그 하기전 부모로 바꾸고
                dragItemIcon.SetParent(originParent);
                dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                // 원래 위치로 되돌림
                dragItemIcon.transform.localPosition = Vector3.zero;

                // raycastTarget을 활성화 한 뒤에
                dragItemIcon.GetComponent<RawImage>().raycastTarget = true;

                if (dragItemAmount.GetComponentInChildren<TextMeshProUGUI>() != null) dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            }
            dragItemIcon = null;
            dragItemAmount = null;
        }
    }
    // 슬롯을 클릭할 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        Slot slot = GetSlotFromEvent(eventData);
        if (slot == null || slot.ItemInSlot == null) return;

        // 강화창이라면 정보창은 나오지 않게
        if (slot.tag == "Rainforcement") return;

        // 인벤토리 초기화
        InitializeInventory();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleLeftClick(slot);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            HandleRightClick(slot);
        }
    }
    // 슬롯에서 나갈때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Tooltip != null) Tooltip.SetActive(false);
        if (EquipTooltip != null) EquipTooltip.SetActive(false);
    }



    // 각각의 장비 아이템에 맞게 할당
    public void Equipment(Slot slot, EquipmentType equipmentType)
    {
        // 슬롯의 장비에 맞는 태그를 찾아서 초기화
        InitializeEquipmentSlots();
        // 장비 아이템 처리 로직
        switch (equipmentType)
        {
            case EquipmentType.Helmet:
                MoveItem(HelmetSlot, slot);
                break;
            case EquipmentType.Weapon:
                MoveItem(WeaponSlot, slot);
                break;
            case EquipmentType.Shield:
                MoveItem(ShieldSlot, slot);
                break;
            case EquipmentType.BodyPlate:
                MoveItem(BodyPlateSlot, slot);
                break;
            case EquipmentType.Pants:
                MoveItem(PantsSlot, slot);
                break;
            case EquipmentType.Shoes:
                MoveItem(ShoesSlot, slot);
                break;
        }
    }
    public void MoveToQuickSlot(Slot slot)
    {
        InitializeQuickSlots();
        MoveItem(PortionSlot, slot);
    }
    // 장비 슬롯으로 아이템 이동
    public void MoveItem(Slot ItemSlot, Slot slot)
    {
        // 아이템이 존재하는 경우에만 처리
        if (slot.ItemInSlot != null)
        {
            // 동일한 장비가 이미 장착되어 있는 경우 반환
            if (ItemSlot.ItemInSlot != null && ItemSlot.ItemInSlot.ID == slot.ItemInSlot.ID) return;
            // 동일한 장비칸 이지만 아이디가 다른경우
            else if (ItemSlot.ItemInSlot != null && ItemSlot.ItemInSlot.ID != slot.ItemInSlot.ID) return;
            else
            {
                ItemSlot.ItemInSlot = slot.ItemInSlot;
                ItemSlot.AmountInSlot = slot.AmountInSlot;

                // 아이콘과 텍스트 활성화
                if (ItemSlot.icon != null || ItemSlot.txt_amount != null)
                {
                    ItemSlot.icon.gameObject.SetActive(true);
                    ItemSlot.icon.texture = slot.ItemInSlot.ITEMICON;

                    ItemSlot.txt_amount.gameObject.SetActive(true);
                    ItemSlot.txt_amount.text = slot.AmountInSlot.ToString();
                }
                ItemSlot.SetSlot();
            }
        }
        // 현재 슬롯의 정보 초기화
        slot.ResetSlot();
    }
    // 장비 슬롯에서 인벤토리로 아이템 이동
    public void UpdateEquipment(Slot slot)
    {
        for(int i = 0; i < inventoryPanel.slots.Length; i++)
        {
            if (inventoryPanel.slots[i].ItemInSlot == null)
            {
                inventoryPanel.slots[i].ItemInSlot = slot.ItemInSlot;
                inventoryPanel.slots[i].AmountInSlot = slot.AmountInSlot;
                inventoryPanel.slots[i].SetSlot();
                return;
            }
        }
    }

    // 슬롯 객체 가져오기
    private Slot GetSlotFromEvent(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return null;
        return dropped.GetComponent<Slot>();
    }
    // 인벤토리 찾아서 초기화
    private void InitializeInventory()
    {
        if (inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Inventory");
            inventoryPanel = inventory.GetComponent<Inventory>();
        }
    }
    // 마우스 좌클릭이 눌렸을때
    private void HandleLeftClick(Slot slot)
    {
        // 아이템이 장비 타입인 경우 EquipTooltip을 사용
        if (slot.ItemInSlot.ITEMTYPE == ItemType.Equipment && EquipTooltip != null)
        {
            EquipTooltip.GetComponent<ActiveEquipToolTip>().clickedSlot = slot;
            EquipTooltip.gameObject.SetActive(true);
        }
        // 장비 타입이 아닌 경우 일반 Tooltip을 사용
        else if (Tooltip != null)
        {
            Tooltip.GetComponent<ActiveToolTip>().clickedSlot = slot;
            Tooltip.SetActive(true);
        }
    }
    // 마우스 우클릭이 눌렸을때
    private void HandleRightClick(Slot slot)
    {
        // 장비 타입인 경우
        if (slot.ItemInSlot.ITEMTYPE == ItemType.Equipment)
        {
            HandleEquipmentSlot(slot);
        }
        // 소비 타입인 경우
        else if (slot.ItemInSlot.ITEMTYPE == ItemType.Consumable)
        {
            HandleConsumableSlot(slot);
        }
    }
    // 장비 패널에서 인벤토리 패널로 아이템 이동
    private void HandleEquipmentSlot(Slot slot)
    {
        EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;
        if (equipmentData == null) return;

        EquipmentType equipmentType = equipmentData.equipmentType;

        // 위치가 장비패널이라면 인벤토리에 집어넣고 슬롯 초기화
        if (slot.transform.parent.name == "EquipmentPanel")
        {
            UpdateEquipment(slot);
            slot.ResetSlot();
        }
        else
        {
            Equipment(slot, equipmentType);
        }
    }
    // 퀵 슬롯 패널에서 인벤토리 패널로 아이템 이동
    private void HandleConsumableSlot(Slot slot)
    {
        if (slot.transform.parent.name == "PortionUI")
        {
            // 인벤토리에 집어넣고 슬롯 초기화
            UpdateEquipment(slot);
            slot.ResetSlot();
        }
        else
        {
            MoveToQuickSlot(slot);
        }
    }

    public bool ShihtMode => isShiftMode;
}