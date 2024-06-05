using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropEvent : MonoBehaviour,IDropHandler
{
    [SerializeField] private DropItem dropItems;

    RectTransform inventoryPanel;
    private void Awake()
    {
        inventoryPanel = GetComponent<RectTransform>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();
        if (draggableItem == null || draggableItem.originParent == null) return;

        Slot slot = draggableItem.originParent.GetComponentInParent<Slot>();
        if (slot.ItemInSlot != null)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, eventData.pointerDrag.transform.position, Camera.main) && eventData.button != PointerEventData.InputButton.Right)
            {
                dropItems.GetComponent<DropItem>().DropSlot = slot;
                dropItems.gameObject.SetActive(true);
            }
        }
    }
}
