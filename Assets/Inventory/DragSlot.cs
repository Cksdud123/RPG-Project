using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;

    public Transform dragItemIcon;
    public Transform dragItemAmount;

    [HideInInspector] public Transform originParent;

    private int originalSiblingIndex;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
    }

    // �巡�׸� ������ �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� ����� �������� ���ٸ� ����
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

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
        Debug.Log("OnEndDrag����");
        if (dragItemIcon != null)
        {
            // �巡�װ� �����ڿ� �θ� ĵ������ �����Ǿ� �־��ٸ� 
            if(dragItemIcon.parent == canvas )
            {
                // �巡���� �������� �θ� �巡�� �ϱ��� �θ�� �ٲٰ�
                dragItemIcon.SetParent(originParent);
                dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                // ���� ��ġ�� �ǵ���
                dragItemIcon.transform.localPosition = Vector3.zero;

                // raycastTarget�� Ȱ��ȭ �� �ڿ�
                dragItemIcon.GetComponent<RawImage>().raycastTarget = true;
                dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

                dragItemIcon = null;
                dragItemAmount = null;
            }
        }
    }
}