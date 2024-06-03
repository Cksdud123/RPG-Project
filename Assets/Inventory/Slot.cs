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
    // ���� �ʱ�ȭ
    public void initSlot()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // ���� ����
    public void SetSlot()
    {
        // Slot�� �ڽİ�ü�� Ȱ��ȭ �� ��
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // �ڽ� ��ü�� ������Ʈ�� ����
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        // ���ڰ����� ���� �������� ������ ����
        icon.texture = ItemInSlot.ITEMICON;
        txt_amount.text = $"{AmountInSlot}";
        txt_amount.enabled = true;
    }
    // ���� ������Ʈ
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

        // ���� ������ ������ �������
        if (ItemInSlot == null)
        {
            ChangeEmptySlot(draggableItem, slot);
        }
        // ���� ������ ������ �ְ� ���̵� �ٸ���
        else if(ItemInSlot != null && ItemInSlot.ID != slot.ItemInSlot.ID)
        {
            SwapItems(draggableItem, slot);
        }
    }
    // �������� �� �������� ������ ����
    private void ChangeEmptySlot(DragSlot draggableItem, Slot slot)
    {
        // �Ű��� �������� �������� ���� �������� �ű�
        draggableItem.dragItemIcon.SetParent(transform);
        draggableItem.dragItemIcon.SetSiblingIndex(0);
        draggableItem.dragItemIcon.localPosition = Vector3.zero;

        // ���� ������ ������ ������ ����
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        // ���� ������ ������ ������ �ʱ�ȭ
        slot.ItemInSlot = null;
        slot.AmountInSlot = 0;

        // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
        draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().text = slot.AmountInSlot.ToString();
        draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);

        // RawImage�� ���� null�� ��ü�� ã�Ƽ� ����
        Transform emptyRawImage = FindEmptyRawImage(transform);
        emptyRawImage.SetParent(draggableItem.originParent);
        emptyRawImage.SetSiblingIndex(0);
        emptyRawImage.localPosition = Vector3.zero;

        SetSlot();

        // ����� �Ϸ�� �� �巡�� �������� �ʱ�ȭ
        draggableItem.dragItemIcon = null;
        draggableItem.dragItemAmount = null;
    }
    // �� �������� ������
    private void SwapItems(DragSlot draggableItem, Slot slot)
    {
        // ���� ������ ������ �����͸� �ӽ� ������ ����
        ItemData tempItem = ItemInSlot;
        int tempAmount = AmountInSlot;

        // ���� ���Կ� �巡���� ������ �����͸� �Ҵ�
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;
        UpdateSlot();

        // �巡���� ���Կ� �ӽ� ����� ������ �����͸� �Ҵ�
        slot.ItemInSlot = tempItem;
        slot.AmountInSlot = tempAmount;
        slot.UpdateSlot();

        SetSlot();
    }
    // �ڽİ�ü�� RawImage�� ã��
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