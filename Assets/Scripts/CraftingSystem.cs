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

    [Header("UI")]
    [SerializeField] private GameObject craftingScreenUI;
    [SerializeField] private GameObject toolsScreenUI, survivalScreenUI, refineScreenUI;

    [Header("List item")]
    [SerializeField] private List<string> inventoryItemList = new List<string>();

    //Tools Button 
    private Button toolsBtn, survivalBtn, refineBtn;

    //Craft Button
    private Button craftAxeBtn, craftPlankBtn;

    //Req
    private Text axeReq1, axeReq2, plankReq;

    Blueprint axeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3, 1);
    Blueprint plankBLP = new Blueprint("Plank", 1, "Log", 1, "", 0, 2);

    public bool isOpen {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(() => OpenToolsScreen());

        survivalBtn = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBtn.onClick.AddListener(() => OpenSurvivalScreen());

        refineBtn = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBtn.onClick.AddListener(() => OpenRefineScreen());

        //Axe
        craftAxeBtn = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBtn.onClick.AddListener(() => CraftAnyItem(axeBLP));

        axeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        axeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();

        //Plank
        craftPlankBtn = refineScreenUI.transform.Find("Plank").transform.Find("CraftButton").GetComponent<Button>();
        craftPlankBtn.onClick.AddListener(() => CraftAnyItem(plankBLP));

        plankReq = refineScreenUI.transform.Find("Plank").transform.Find("Req1").GetComponent<Text>();
    }

    private void OpenToolsScreen()
    {
        toolsScreenUI.SetActive(true);
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
    }

    private void OpenSurvivalScreen()
    {
        toolsScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
        refineScreenUI.SetActive(false);
    }
    private void OpenRefineScreen()
    {
        toolsScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
    }

    private void CraftAnyItem(Blueprint blueprint)
    {
        for(int i = 0; i < blueprint.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprint.itemName);
        }

        if (blueprint.numOfRequirement == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprint.req1, blueprint.req1Amount);
        }
        else if(blueprint.numOfRequirement == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprint.req1, blueprint.req1Amount);
            InventorySystem.Instance.RemoveItem(blueprint.req2, blueprint.req2Amount);
        }

        StartCoroutine(Caculate());
        

    }

    IEnumerator Caculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCaculateList();
        RefreshNeededItems();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            craftingScreenUI.SetActive(true);
            SelectionManager.Instance.DisableSelection();
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            isOpen = false;
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
            }
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;

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
                case "Log":
                    log_count += 1;
                    break;
            }
        }

        //---AXE---//

        axeReq1.text = "3 Stone [" + stone_count + "]";
        axeReq2.text = "3 Stick [" + stick_count + "]";

        if(stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotToAvailable(axeBLP.numberOfItemsToProduce))
        {
            craftAxeBtn.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBtn.gameObject.SetActive(false);
        }
        
        //---PLANK---//

        plankReq.text = plankBLP.req1Amount + plankBLP.req1 + " ["  + log_count + "]";

        if(log_count >= plankBLP.req1Amount && InventorySystem.Instance.CheckSlotToAvailable(plankBLP.numberOfItemsToProduce))
        {
            craftPlankBtn.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBtn.gameObject.SetActive(false);
        }
    }
}
