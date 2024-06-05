using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropEvent : MonoBehaviour,IDropHandler
{
    [SerializeField] private DropItem dropItems;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();
        Slot slot = draggableItem.originParent.GetComponent<Slot>();

        RectTransform inventoryPanel = GetComponent<RectTransform>();

        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, eventData.position, Camera.main))
        {
            dropItems.GetComponent<DropItem>().DropSlot = slot;
            dropItems.gameObject.SetActive(true);
        }
    }
}
