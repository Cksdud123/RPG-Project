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
    }
    public void UpdateSlot()
    {
        // �ڽ� ��ü�� ������Ʈ�� ����
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        // ���ڰ����� ���� �������� ������ ����
        icon.texture = ItemInSlot.ITEMICON;
        txt_amount.text = $"{AmountInSlot}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();

        // �����ۿ� ������ ������
        if (ItemInSlot == null)
        {
            // �Ű��� �������� �������� ���� �������� �ű�
            draggableItem.dragItemIcon.SetParent(transform);
            draggableItem.dragItemIcon.SetSiblingIndex(0);
            draggableItem.dragItemIcon.localPosition = Vector3.zero;

            // ���� ������ ������ ������ ����
            ItemInSlot = draggableItem.originParent.GetComponent<Slot>().ItemInSlot;
            AmountInSlot = draggableItem.originParent.GetComponent<Slot>().AmountInSlot;

            // �巡���� �������� ������ �ʱ�ȭ (�ٽ� �巡�� �� ���ֵ���)
            draggableItem.dragItemIcon = null;
            draggableItem.dragItemAmount = null;

            // ���� ������ ������ ������ �ʱ�ȭ
            draggableItem.originParent.GetComponent<Slot>().ItemInSlot = null;
            draggableItem.originParent.GetComponent<Slot>().AmountInSlot = 0;

            // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
            draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().text = "0";
            //draggableItem.originParent.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            // RawImage�� ���� null�� ��ü�� ã�Ƽ� ����
            Transform emptyRawImage = FindEmptyRawImage(transform);
            emptyRawImage.SetParent(draggableItem.originParent);
            emptyRawImage.SetSiblingIndex(0);
            emptyRawImage.localPosition = Vector3.zero;

            SetSlot();
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
