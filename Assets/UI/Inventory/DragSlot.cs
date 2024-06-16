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

    private bool isShiftMode = false; // ����Ʈ ��� ����

    [Header("Equipment GameObject")]
    GameObject Helmet, Weapon, Shield, BodyPlate, Pants, Shoes;

    [Header("Equipment Slot")]
    Slot HelmetSlot, WeaponSlot, ShieldSlot, BodyPlateSlot, PantsSlot, ShoesSlot;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("InventoryCanvas").transform;

        // Ȱ��ȭ �Ǿ������� ã�� 
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

    // �巡�׸� ������ �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� ����� �������� ���ٸ� ����
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isShiftMode = true;
        }
        else isShiftMode = false;

        // �巡���� ������Ʈ�� ������ ������
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

    // �巡�� ���� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null)
        {
            dragItemIcon.transform.position = eventData.position;
        }
    }

    // �巡�װ� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null)
        {
            // �巡�װ� �����ڿ� �θ� ĵ������ �����Ǿ� �־��ٸ� 
            if (dragItemIcon.parent == canvas)
            {
                // �巡���� �������� �θ� �巡�� �ϱ��� �θ�� �ٲٰ�
                dragItemIcon.SetParent(originParent);
                dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                // ���� ��ġ�� �ǵ���
                dragItemIcon.transform.localPosition = Vector3.zero;

                // raycastTarget�� Ȱ��ȭ �� �ڿ�
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

        // ���콺 ������ ���������� 
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Tooltip.GetComponent<ActiveToolTip>().clickedSlot = slot;
            Tooltip.SetActive(true);
        }
        // ���콺 ������ ��ư�� Ŭ���߰� ���Ÿ�� �϶���
        else if (eventData.button == PointerEventData.InputButton.Right && slot.ItemInSlot.ITEMTYPE == ItemType.Equipment)
        {
            // ItemInSlot�� EquipmentData�� ĳ�����Ͽ� EquipmentType�� ����
            EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;

            if (equipmentData != null)
            {
                EquipmentType equipmentType = equipmentData.equipmentType;

                // ���� ������ �θ� ��ü�� �̸��� EquipmentPanel�̶��
                if (slot.transform.parent.name == "EquipmentPanel")
                {
                    // �κ��丮�� ����ְ�
                    UpdateEquipment(slot);
                    // ���� ������ ��Ȱ��ȭ ��
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
    // ������ ��� �����ۿ� �°� �Ҵ�
    public void Equipment(Slot slot, EquipmentType equipmentType)
    {
        // ��� ������ ó�� ����
        switch (equipmentType)
        {
            // slot�� ���� �κ��丮�� Helmet�� �̵���Ŵ
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

        // �����ܰ� �ؽ�Ʈ Ȱ��ȭ
        if (Equipment.icon != null || Equipment.txt_amount != null)
        {
            Equipment.icon.gameObject.SetActive(true);
            Equipment.icon.texture = slot.ItemInSlot.ITEMICON;

            Equipment.txt_amount.gameObject.SetActive(true);
            Equipment.txt_amount.text = slot.AmountInSlot.ToString();
        }

        // SetSlot ȣ��
        Equipment.SetSlot();

        // ���� ������ ���� �ʱ�ȭ
        slot.ResetSlot();
    }
    public void UpdateEquipment(Slot slot)
    {

    }
    public bool ShihtMode => isShiftMode;
}