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
    // �巡�׸� ������ �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� ����� �������� ���ٸ� ����
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;
        // ��Ŭ���� ��ä ������ ������ �ݸ� �������� �س���
        if (eventData.button == PointerEventData.InputButton.Right) isShiftMode = true;
        else isShiftMode = false;

        // �巡���� ������Ʈ�� ������ ������
        dragItemIcon = transform.GetComponentInChildren<RawImage>().transform;
        dragItemIcon.GetComponent<RawImage>().raycastTarget = false;

        // ���� ������ ����
        originParent = transform;
        originalSiblingIndex = dragItemIcon.GetSiblingIndex();

        // �ؽ�Ʈ ������ �巡�� �߿� ��Ȱ��ȭ
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
    // ������ Ŭ���� �� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

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
                // ��ġ�� ����г��̶��
                if (slot.transform.parent.name == "EquipmentPanel")
                {
                    inventory = GameObject.FindGameObjectWithTag("Inventory");
                    inventoryPanel = inventory.GetComponent<Inventory>();
                    // �κ��丮�� ����ְ�
                    UpdateEquipment(slot);
                    // ���� ������ ��Ȱ��ȭ ��
                    slot.ResetSlot();
                }
                else Equipment(slot, equipmentType);
            }
        }
    }
    // ���Կ��� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
    }
    // ������ ��� �����ۿ� �°� �Ҵ�
    public void Equipment(Slot slot, EquipmentType equipmentType)
    {
        InitializeEquipmentSlots();
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
    // ��� �������� ������ �̵�
    public void MoveEquip(Slot Equipment, Slot slot)
    {
        // �������� �����ϴ� ��쿡�� ó��
        if (slot.ItemInSlot != null)
        {
            // ������ ��� �̹� �����Ǿ� �ִ� ��� ��ȯ
            if (Equipment.ItemInSlot != null && Equipment.ItemInSlot.ID == slot.ItemInSlot.ID) return;
            // ������ ���ĭ ������ ���̵� �ٸ����
            else if (Equipment.ItemInSlot != null && Equipment.ItemInSlot.ID != slot.ItemInSlot.ID) return;
            else
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
                Equipment.SetSlot();
            }
        }
        // ���� ������ ���� �ʱ�ȭ
        slot.ResetSlot();
    }
    // ��� ���Կ��� �κ��丮�� ������ �̵�
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
    public bool ShihtMode => isShiftMode;
}

