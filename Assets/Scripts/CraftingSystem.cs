using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    #region Singleton

    public static CraftingSystem Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] private GameObject craftingScreenUI;
    [SerializeField] private GameObject toolsScreenUI;

    [SerializeField] private List<string> inventoryItemList = new List<string>();

    private Button toolsBtn;

    private Button craftBtn;

    private Text req1, req2;

    Blueprint axeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

    public bool isOpen {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(() => OpenToolsScreen());

        craftBtn = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftBtn.onClick.AddListener(() => CraftAnyItem(axeBLP));

        req1 = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        req2 = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();
    }

    private void OpenToolsScreen()
    {
        toolsScreenUI.SetActive(true);
        craftingScreenUI.SetActive(false);
    }

    private void CraftAnyItem(Blueprint blueprint)
    {
        InventorySystem.Instance.AddToInventory(blueprint.itemName);

        if(blueprint.numOfRequirement == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprint.req1, blueprint.req1Amount);
        }
        else if(blueprint.numOfRequirement == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprint.req1, blueprint.req1Amount);
            InventorySystem.Instance.RemoveItem(blueprint.req2, blueprint.req2Amount);
        }

        StartCoroutine(Caculate());
        

        RefreshNeededItems();
    }

    IEnumerator Caculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.ReCaculateList();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            craftingScreenUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            isOpen = false;
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
        }
    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;
        foreach(string itemName in inventoryItemList)
        {
            switch(itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        //---AXE---//

        req1.text = "3 Stone [" + stone_count + "]";
        req2.text = "3 Stick [" + stick_count + "]";

        if(stone_count >= 3 && stick_count >= 3)
        {
            craftBtn.gameObject.SetActive(true);
        }
        else
        {
            craftBtn.gameObject.SetActive(false);
        }
    }
}
