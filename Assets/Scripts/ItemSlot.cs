using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null; 
            
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Item)
        {
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;
            if (!DragDrop.itemBeingDragged.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isNowEquipped = false;
            }
            else if (DragDrop.itemBeingDragged.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isNowEquipped = true;
            }
            
        }
    }
}
