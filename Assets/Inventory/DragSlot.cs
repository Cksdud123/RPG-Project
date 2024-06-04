using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Tooltip;
    [SerializeField] private DropItem dropItem;

    public Transform dragItemIcon;
    public Transform dragItemAmount;

    [HideInInspector] public Transform canvas;
    [HideInInspector] public Transform originParent;

    private int originalSiblingIndex;

    private bool isShiftMode = false; // 쉬프트 모드 여부

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
    }

    // 드래그를 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 슬롯에 저장된 아이템이 없다면 리턴
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isShiftMode = true;
        }
        else isShiftMode = false;

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
        if (dragItemIcon != null)
        {
            // 드래그가 끝난 뒤 아이템을 드랍
            if (dragItemIcon.parent == canvas)
            {
                // 드랍 위치가 인벤토리 패널 외부인지 확인
                Slot slot = originParent.GetComponent<Slot>();
                RectTransform inventoryPanelRect = slot.InventoryPanel.GetComponent<RectTransform>();

                // 현재 드래그한 아이템 데이터가 inventoryPanelRect에 있지 않으면서 좌클릭으로만 나눌때
                if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryPanelRect, eventData.position, Camera.main) && eventData.button != PointerEventData.InputButton.Right)
                {
                    GameObject dropped = eventData.pointerDrag;
                    Slot dropslot = dropped.GetComponent<Slot>();

                    dropItem.GetComponent<DropItem>().DropSlot = dropslot;
                    dropItem.gameObject.SetActive(true);

                    // 드래그한 아이템의 부모를 드래그하기 전 부모로 변경
                    dragItemIcon.SetParent(originParent);
                    dragItemIcon.SetSiblingIndex(originalSiblingIndex);
                    dragItemIcon.transform.localPosition = Vector3.zero;

                    // raycastTarget을 활성화
                    dragItemIcon.GetComponent<RawImage>().raycastTarget = true;

                    if (dragItemAmount.GetComponentInChildren<TextMeshProUGUI>() != null) dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    // 드래그한 아이템의 부모를 드래그하기 전 부모로 변경
                    dragItemIcon.SetParent(originParent);
                    dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                    // 원래 위치로 되돌림
                    dragItemIcon.transform.localPosition = Vector3.zero;

                    // raycastTarget을 활성화
                    dragItemIcon.GetComponent<RawImage>().raycastTarget = true;

                    if (dragItemAmount.GetComponentInChildren<TextMeshProUGUI>() != null) dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
            }
            dragItemIcon = null;
            dragItemAmount = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

        if (slot.ItemInSlot == null) return;

        Tooltip.GetComponent<ActiveToolTip>().clickedSlot = slot;
        Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
    }
    public bool ShihtMode => isShiftMode;
}