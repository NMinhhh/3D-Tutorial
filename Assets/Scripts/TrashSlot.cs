using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Deletion Alert")]
    [SerializeField] private GameObject deletionAlertUI;
    [Header("Image")]
    [SerializeField] private Image trashIcon;
    [SerializeField] private Sprite trashOpen;
    [SerializeField] private Sprite trashClose;

    private Text ques;
    private Button yesBtn;
    private Button noBtn;


    GameObject ItemDrag
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    private GameObject itemToBeDeleted;

    string itemName
    {
        get
        {
            return itemToBeDeleted.name;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ques = deletionAlertUI.transform.Find("Question").GetComponent<Text>();
        yesBtn = deletionAlertUI.transform.Find("YesButton").GetComponent<Button>();
        yesBtn.onClick.AddListener(() => DeleteItem());
        noBtn = deletionAlertUI.transform.Find("NoButton").GetComponent<Button>();
        noBtn .onClick.AddListener(() => CancelDeleteItem());
    }



    private void DeleteItem()
    {
        trashIcon.sprite = trashClose;
        DestroyImmediate(itemToBeDeleted);
        deletionAlertUI.SetActive(false);
        InventorySystem.Instance.ReCaculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    private void CancelDeleteItem()
    {
        trashIcon.sprite = trashClose;
        deletionAlertUI.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDrag.GetComponent<InventoryItem>().trashable)
        {
            itemToBeDeleted = ItemDrag;
            StartCoroutine(NotifyBeforeDeletion());
        }
    }

    IEnumerator NotifyBeforeDeletion()
    {
        deletionAlertUI.SetActive(true);
        ques.text = "Throw away this " + itemName + " ?";
        yield return new WaitForSeconds(1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemDrag != null &&  ItemDrag.GetComponent<InventoryItem>().trashable)
        {
            trashIcon.sprite = trashOpen;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemDrag != null && ItemDrag.GetComponent<InventoryItem>().trashable)
        {
            trashIcon.sprite = trashClose;
        }
    }
}
