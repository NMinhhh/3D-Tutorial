using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    #region Singleton
    public static InventorySystem Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    [Header("Inventory")]

    [SerializeField] private GameObject inventoryScreen;

    [SerializeField] private List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject whatIsSlotToEquip;
    private GameObject itemAdd;
    public bool isOpen {  get; private set; }
    [Space]
    [Space]


    [Header("Pickup PopUp")]
    [SerializeField] private GameObject pickupAlert;
    [SerializeField] private Text pickupName;
    [SerializeField] private Image pickupImage;
    [SerializeField] private float appearTimer;
    private float startAppearTimer;
    private bool isPickup;

    [Header("Item Info Panel")]
    public GameObject itemInfoPanelUI;


    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform chill in inventoryScreen.transform)
        {
            if (chill.CompareTag("Slot"))
            {
                slotList.Add(chill.gameObject);
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            inventoryScreen.SetActive(true);
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            isOpen = false;
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
            }
            inventoryScreen.SetActive(false);
        }
        if (isPickup)
        {
            if(Time.time >= startAppearTimer + appearTimer)
            {
                isPickup = false;
                pickupAlert.gameObject.SetActive(false);
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        whatIsSlotToEquip = FindNextEmptySlot();
        itemAdd = Instantiate(Resources.Load<GameObject>(itemName), whatIsSlotToEquip.transform.position, whatIsSlotToEquip.transform.rotation);
        itemAdd.name = itemName;
        itemList.Add(itemName);
        itemAdd.transform.SetParent(whatIsSlotToEquip.transform);
        TriggerPickupPopUp(itemName, itemAdd.transform.GetComponent<Image>().sprite);
        ReCaculateList();
        CraftingSystem.Instance.RefreshNeededItems();

    }

    private void TriggerPickupPopUp(string pickupName, Sprite pickupImage)
    {
        isPickup = true;
        startAppearTimer = Time.time;
        pickupAlert.SetActive(true);
        this.pickupName.text = "+1 " + pickupName;
        this.pickupImage.sprite = pickupImage;
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
                counter++;
        }
        if (counter == 21)
            return true;
        else 
            return false;
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for(int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).gameObject.name == nameToRemove && amountToRemove > 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    amountToRemove -= 1;
                }
            }
        }
        ReCaculateList();
        CraftingSystem.Instance.RefreshNeededItems();

    }

    public void ReCaculateList()
    {
        itemList.Clear();
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                itemList.Add(slot.transform.GetChild(0).gameObject.name);
            }
        }
    }
}
