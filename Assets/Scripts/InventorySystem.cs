using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private GameObject inventoryScreen;

    [SerializeField] private List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject whatIsSlotToEquip;
    private GameObject itemAdd;


    public bool isOpen {  get; private set; }

    // Start is called before the first frame update
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
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            isOpen = false;
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            inventoryScreen.SetActive(false);
        }
    }

    public void AddToInventory(string itemName)
    {
        whatIsSlotToEquip = FindNextEmptySlot();
        itemAdd = Instantiate(Resources.Load<GameObject>(itemName), whatIsSlotToEquip.transform.position, whatIsSlotToEquip.transform.rotation);
        itemAdd.name = itemName;
        itemList.Add(itemName);
        itemAdd.transform.SetParent(whatIsSlotToEquip.transform);
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

    internal void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for(int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).gameObject.name == nameToRemove && amountToRemove > 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    amountToRemove -= 1;
                }
            }
        }
    }

    internal void ReCaculateList()
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
