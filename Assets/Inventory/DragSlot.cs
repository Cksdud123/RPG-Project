using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform dragItemIcon;
    public Transform dragItemAmount;

    private Transform originItem;
    private Transform originAmount;

    // �巡�׸� �����Ҷ� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� ����� �������� ���ٸ� ����
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        // �巡�� �� �������� ������ ����
        originItem = transform.parent;
        originAmount = transform.parent;

        // �巡���� ������Ʈ�� ������ ������
        dragItemIcon = transform.GetComponentInChildren<RawImage>().transform;
        dragItemAmount = transform.GetComponentInChildren<TextMeshProUGUI>().transform;

        if (dragItemIcon != null && dragItemAmount != null)
        {
            dragItemIcon.SetParent(transform.root);
            dragItemAmount.SetParent(transform.root);
            dragItemIcon.SetAsLastSibling();
            dragItemAmount.SetAsLastSibling();
        }
    }
    // �巡�� ���϶� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null && dragItemAmount != null)
        {
            dragItemIcon.transform.position = Input.mousePosition;
            dragItemAmount.transform.position = Input.mousePosition;
        }
    }
    // �巡�װ� ������ ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null && dragItemAmount != null)
        {
            dragItemIcon.SetParent(originItem);
            dragItemAmount.SetParent(originAmount);

            dragItemIcon.localPosition = Vector3.zero;
            dragItemAmount.localPosition = Vector3.zero;
        }
    }
}
