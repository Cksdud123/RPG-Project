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
            draggableItem.dragItemIcon.localPosition = Vector3.zero;

            // ���� ������ ������ ������ ����
            ItemInSlot = draggableItem.originItemIcon.GetComponent<Slot>().ItemInSlot;

            // �巡���� �������� �������� �ʱ�ȭ
            draggableItem.dragItemIcon = null;
            // �̵��� ������ ������ ������ �ʱ�ȭ
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
