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

    // 드래그를 시작할때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 슬롯에 저장된 아이템이 없다면 리턴
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        // 드래그 전 아이템의 정보를 저장
        originItem = transform.parent;
        originAmount = transform.parent;

        // 드래그할 오브젝트의 정보를 가져옴
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
    // 드래그 중일때 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (dragItemIcon != null && dragItemAmount != null)
        {
            dragItemIcon.transform.position = Input.mousePosition;
            dragItemAmount.transform.position = Input.mousePosition;
        }
    }
    // 드래그가 끝날때 호출
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
