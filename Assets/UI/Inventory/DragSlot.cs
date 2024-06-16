using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Tooltip;

    public Transform dragItemIcon;
    public Transform dragItemAmount;

    [HideInInspector] public Transform canvas;
    [HideInInspector] public Transform originParent;

    private int originalSiblingIndex;

    private bool isShiftMode = false; // 쉬프트 모드 여부

    [Header("Equipment GameObject")]
    GameObject Helmet, Weapon, Shield, BodyPlate, Pants, Shoes;

    [Header("Equipment Slot")]
    Slot HelmetSlot, WeaponSlot, ShieldSlot, BodyPlateSlot, PantsSlot, ShoesSlot;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("InventoryCanvas").transform;

        // 활성화 되었을때만 찾게 
        Helmet = GameObject.FindGameObjectWithTag("Helmet");
        Weapon = GameObject.FindGameObjectWithTag("Weapon");
        Shield = GameObject.FindGameObjectWithTag("Shield");
        BodyPlate = GameObject.FindGameObjectWithTag("BodyPlate");
        Pants = GameObject.FindGameObjectWithTag("Pants");
        Shoes = GameObject.FindGameObjectWithTag("Shoes");

        if (Helmet != null || Weapon != null || Shield != null || BodyPlate != null || Pants != null || Shoes != null)
        {
            HelmetSlot = Helmet.GetComponent<Slot>();
            WeaponSlot = Weapon.GetComponent<Slot>();
            ShieldSlot = Shield.GetComponent<Slot>();
            BodyPlateSlot = BodyPlate.GetComponent<Slot>();
            PantsSlot = Pants.GetComponent<Slot>();
            ShoesSlot = Shoes.GetComponent<Slot>();
        }
    }

    // 드래그를 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 슬롯에 저장된 아이템이 없다면 리턴
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isShiftMode = true;
        }
        else isShiftMode = false;

        // 드래그할 오브젝트의 정보를 가져옴
        dragItemIcon = transform.GetComponentInChildren<RawImage>().transform;
        dragItemIcon.GetComponent<RawImage>().raycastTarget = false;

        originParent = transform;
        originalSiblingIndex = dragItemIcon.GetSiblingIndex();

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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();
        ItemInfo slotInfo = slot.GetComponent<ItemInfo>();

        if (slot.ItemInSlot == null) return;

        // 마우스 왼쪽을 눌렀을때만 
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Tooltip.GetComponent<ActiveToolTip>().clickedSlot = slot;
            Tooltip.SetActive(true);
        }
        // 마우스 오른쪽 버튼을 클릭했고 장비타입 일때만
        else if (eventData.button == PointerEventData.InputButton.Right && slot.ItemInSlot.ITEMTYPE == ItemType.Equipment)
        {
            // ItemInSlot을 EquipmentData로 캐스팅하여 EquipmentType에 접근
            EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;

            if (equipmentData != null)
            {
                EquipmentType equipmentType = equipmentData.equipmentType;

                // 만약 슬롯의 부모 객체의 이름이 EquipmentPanel이라면
                if (slot.transform.parent.name == "EquipmentPanel")
                {
                    // 인벤토리에 집어넣고
                    UpdateEquipment(slot);
                    // 현재 슬롯을 비활성화 함
                    slot.ResetSlot();
                }
                else Equipment(slot, equipmentType);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
    }
    // 각각의 장비 아이템에 맞게 할당
    public void Equipment(Slot slot, EquipmentType equipmentType)
    {
        // 장비 아이템 처리 로직
        switch (equipmentType)
        {
            // slot을 현재 인벤토리에 Helmet에 이동시킴
            case EquipmentType.Helmet:
                MoveEquip(HelmetSlot, slot);
                break;
            case EquipmentType.Weapon:
                MoveEquip(WeaponSlot, slot);
                break;
            case EquipmentType.Shield:
                MoveEquip(ShieldSlot, slot);
                break;
            case EquipmentType.BodyPlate:
                MoveEquip(BodyPlateSlot, slot);
                break;
            case EquipmentType.Pants:
                MoveEquip(PantsSlot, slot);
                break;
            case EquipmentType.Shoes:
                MoveEquip(ShoesSlot, slot);
                break;
        }
    }
    public void MoveEquip(Slot Equipment, Slot slot)
    {
        Equipment.ItemInSlot = slot.ItemInSlot;
        Equipment.AmountInSlot = slot.AmountInSlot;

        // 아이콘과 텍스트 활성화
        if (Equipment.icon != null || Equipment.txt_amount != null)
        {
            Equipment.icon.gameObject.SetActive(true);
            Equipment.icon.texture = slot.ItemInSlot.ITEMICON;

            Equipment.txt_amount.gameObject.SetActive(true);
            Equipment.txt_amount.text = slot.AmountInSlot.ToString();
        }

        // SetSlot 호출
        Equipment.SetSlot();

        // 현재 슬롯의 정보 초기화
        slot.ResetSlot();
    }
    public void UpdateEquipment(Slot slot)
    {

    }
    public bool ShihtMode => isShiftMode;
}