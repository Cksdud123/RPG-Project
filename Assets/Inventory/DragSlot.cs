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

    // 드래그를 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 슬롯에 저장된 아이템이 없다면 리턴
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

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
        Debug.Log("OnEndDrag실행");
        if (dragItemIcon != null)
        {
            // 드래그가 끝난뒤에 부모가 캔버스로 설정되어 있었다면 
            if(dragItemIcon.parent == canvas )
            {
                // 드래그한 아이템의 부모를 드래그 하기전 부모로 바꾸고
                dragItemIcon.SetParent(originParent);
                dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                // 원래 위치로 되돌림
                dragItemIcon.transform.localPosition = Vector3.zero;

                // raycastTarget을 활성화 한 뒤에
                dragItemIcon.GetComponent<RawImage>().raycastTarget = true;
                dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

                dragItemIcon = null;
                dragItemAmount = null;
            }
        }
    }
}