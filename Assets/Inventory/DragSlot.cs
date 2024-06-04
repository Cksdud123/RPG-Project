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

    private bool isShiftMode = false; // ����Ʈ ��� ����

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
    }

    // �巡�׸� ������ �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� ����� �������� ���ٸ� ����
        if (transform.GetComponent<Slot>().ItemInSlot == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isShiftMode = true;
        }
        else isShiftMode = false;

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
        if (dragItemIcon != null)
        {
            // �巡�װ� ���� �� �������� ���
            if (dragItemIcon.parent == canvas)
            {
                // ��� ��ġ�� �κ��丮 �г� �ܺ����� Ȯ��
                Slot slot = originParent.GetComponent<Slot>();
                RectTransform inventoryPanelRect = slot.InventoryPanel.GetComponent<RectTransform>();

                // ���� �巡���� ������ �����Ͱ� inventoryPanelRect�� ���� �����鼭 ��Ŭ�����θ� ������
                if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryPanelRect, eventData.position, Camera.main) && eventData.button != PointerEventData.InputButton.Right)
                {
                    GameObject dropped = eventData.pointerDrag;
                    Slot dropslot = dropped.GetComponent<Slot>();

                    dropItem.GetComponent<DropItem>().DropSlot = dropslot;
                    dropItem.gameObject.SetActive(true);

                    // �巡���� �������� �θ� �巡���ϱ� �� �θ�� ����
                    dragItemIcon.SetParent(originParent);
                    dragItemIcon.SetSiblingIndex(originalSiblingIndex);
                    dragItemIcon.transform.localPosition = Vector3.zero;

                    // raycastTarget�� Ȱ��ȭ
                    dragItemIcon.GetComponent<RawImage>().raycastTarget = true;

                    if (dragItemAmount.GetComponentInChildren<TextMeshProUGUI>() != null) dragItemAmount.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    // �巡���� �������� �θ� �巡���ϱ� �� �θ�� ����
                    dragItemIcon.SetParent(originParent);
                    dragItemIcon.SetSiblingIndex(originalSiblingIndex);

                    // ���� ��ġ�� �ǵ���
                    dragItemIcon.transform.localPosition = Vector3.zero;

                    // raycastTarget�� Ȱ��ȭ
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