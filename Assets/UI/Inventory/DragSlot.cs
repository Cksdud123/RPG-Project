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

    private bool isShiftMode = false; // ����Ʈ ��� ����

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
        Slot slot = GetSlotFromEvent(eventData);
        if (slot == null || slot.ItemInSlot == null) return;

        // ��ȭâ�̶�� ����â�� ������ �ʰ�
        if (slot.tag == "Rainforcement") return;

        // �κ��丮 �ʱ�ȭ
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
    // ���Կ��� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Tooltip != null) Tooltip.SetActive(false);
        if (EquipTooltip != null) EquipTooltip.SetActive(false);
    }



    // ������ ��� �����ۿ� �°� �Ҵ�
    public void Equipment(Slot slot, EquipmentType equipmentType)
    {
        // ������ ��� �´� �±׸� ã�Ƽ� �ʱ�ȭ
        InitializeEquipmentSlots();
        // ��� ������ ó�� ����
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
    // ��� �������� ������ �̵�
    public void MoveItem(Slot ItemSlot, Slot slot)
    {
        // �������� �����ϴ� ��쿡�� ó��
        if (slot.ItemInSlot != null)
        {
            // ������ ��� �̹� �����Ǿ� �ִ� ��� ��ȯ
            if (ItemSlot.ItemInSlot != null && ItemSlot.ItemInSlot.ID == slot.ItemInSlot.ID) return;
            // ������ ���ĭ ������ ���̵� �ٸ����
            else if (ItemSlot.ItemInSlot != null && ItemSlot.ItemInSlot.ID != slot.ItemInSlot.ID) return;
            else
            {
                ItemSlot.ItemInSlot = slot.ItemInSlot;
                ItemSlot.AmountInSlot = slot.AmountInSlot;

                // �����ܰ� �ؽ�Ʈ Ȱ��ȭ
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

    // ���� ��ü ��������
    private Slot GetSlotFromEvent(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return null;
        return dropped.GetComponent<Slot>();
    }
    // �κ��丮 ã�Ƽ� �ʱ�ȭ
    private void InitializeInventory()
    {
        if (inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Inventory");
            inventoryPanel = inventory.GetComponent<Inventory>();
        }
    }
    // ���콺 ��Ŭ���� ��������
    private void HandleLeftClick(Slot slot)
    {
        // �������� ��� Ÿ���� ��� EquipTooltip�� ���
        if (slot.ItemInSlot.ITEMTYPE == ItemType.Equipment && EquipTooltip != null)
        {
            EquipTooltip.GetComponent<ActiveEquipToolTip>().clickedSlot = slot;
            EquipTooltip.gameObject.SetActive(true);
        }
        // ��� Ÿ���� �ƴ� ��� �Ϲ� Tooltip�� ���
        else if (Tooltip != null)
        {
            Tooltip.GetComponent<ActiveToolTip>().clickedSlot = slot;
            Tooltip.SetActive(true);
        }
    }
    // ���콺 ��Ŭ���� ��������
    private void HandleRightClick(Slot slot)
    {
        // ��� Ÿ���� ���
        if (slot.ItemInSlot.ITEMTYPE == ItemType.Equipment)
        {
            HandleEquipmentSlot(slot);
        }
        // �Һ� Ÿ���� ���
        else if (slot.ItemInSlot.ITEMTYPE == ItemType.Consumable)
        {
            HandleConsumableSlot(slot);
        }
    }
    // ��� �гο��� �κ��丮 �гη� ������ �̵�
    private void HandleEquipmentSlot(Slot slot)
    {
        EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;
        if (equipmentData == null) return;

        EquipmentType equipmentType = equipmentData.equipmentType;

        // ��ġ�� ����г��̶�� �κ��丮�� ����ְ� ���� �ʱ�ȭ
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
    // �� ���� �гο��� �κ��丮 �гη� ������ �̵�
    private void HandleConsumableSlot(Slot slot)
    {
        if (slot.transform.parent.name == "PortionUI")
        {
            // �κ��丮�� ����ְ� ���� �ʱ�ȭ
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