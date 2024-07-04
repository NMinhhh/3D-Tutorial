using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    #region Singleton
    public static EquipSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    [Header("QuickSlots")]
    [SerializeField] private GameObject quickSlotPanerl;

    [SerializeField] private List<GameObject> quickSlotList = new List<GameObject>();

    [Header("Numbers Holder")]
    [SerializeField] private GameObject numbersHolder;
    private int selectedNumber = -1;
    public GameObject selectedItem;

    [Header("Weapon Holder")]
    [SerializeField] private Transform weaponHolder;

    private GameObject selectedItemModel;


    // Start is called before the first frame update
    void Start()
    {
        PopulateQuickSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectedQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectedQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectedQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectedQuickSlot(7);
        }
    }

    private void SelectedQuickSlot(int number)
    {
        if (CheckIfSlotIsFull(number))
        {
            if(selectedNumber != number)
            {
                selectedNumber = number;

                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SelectedItemModel(selectedItem.gameObject.name);

                foreach (Transform child in numbersHolder.transform)
                {
                    child.Find("Text").GetComponent<Text>().color = Color.gray;
                }

                numbersHolder.transform.GetChild(number - 1).Find("Text").GetComponent<Text>().color = Color.green;
            }
            else
            {
                selectedNumber = -1;
                if(selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                numbersHolder.transform.GetChild(number - 1).Find("Text").GetComponent<Text>().color = Color.grey;
            }
           
         
        }
    }

    private void SelectedItemModel(string itemName)
    {
        if(selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }
        GameObject model = Resources.Load<GameObject>("Model/" + itemName);
        selectedItemModel = Instantiate(model, model.transform.localPosition,
            model.transform.localRotation);
        selectedItemModel.transform.SetParent(weaponHolder.transform, false);
    }

    private GameObject GetSelectedItem(int number)
    {
        return quickSlotList[number - 1].transform.GetChild(0).gameObject;
    }

    private bool CheckIfSlotIsFull(int number)
    {
        if (quickSlotList[number - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateQuickSlotList()
    {
        foreach (Transform quickSlot in quickSlotPanerl.transform)
        {
            if (quickSlot.CompareTag("QuickSlot"))
            {
                quickSlotList.Add(quickSlot.gameObject);
            }
        }
    }

    public void AddToQuickSlot(GameObject item)
    {
        GameObject avaiableSlot = FindNextEmptySlot();
        item.transform.SetParent(avaiableSlot.transform, false);
        InventorySystem.Instance.ReCaculateList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject quickSlot in quickSlotList)
        {
            if(quickSlot.transform.childCount == 0)
            {
                return quickSlot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject quickSlot in quickSlotList)
        {
            if (quickSlot.transform.childCount > 0)
                counter++;
        }
        if (counter == 7)
            return true;
        else
            return false;
    }
}
