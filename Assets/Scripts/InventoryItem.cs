using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Trashable")]
    public bool trashable;
    [Space]

    [Header("Item info")]
    private GameObject itemInfoUI;
    private Text itemName_ItemInfoUI;
    private Text itemDescription_ItemInfoUI;
    private Text itemFunctionality_ItemInfoUI;

    [SerializeField] private string thisName, thisDescription, thisFunctionality;
    [Space]

    [Header("Consumption")]
    private GameObject itemPendingConsumption;
    [SerializeField] private bool isConsumable;
    [SerializeField] private float healthEffect;
    [SerializeField] private float caloriesEffect;
    [SerializeField] private float hydrationEffect;

    [Header("Equip")]
    private GameObject itemPendingEquipping;
    public bool isEquippable;
    public bool isNowEquipped;
    public bool isSelected;


    // Start is called before the first frame update
    void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInfoPanelUI;
        itemName_ItemInfoUI = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemDescription_ItemInfoUI = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemFunctionality_ItemInfoUI = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemName_ItemInfoUI.text = thisName;
        itemDescription_ItemInfoUI.text = thisDescription;
        itemFunctionality_ItemInfoUI.text = thisFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                ConsumingFuction(healthEffect, caloriesEffect, hydrationEffect);
            }

            if(isEquippable && !isNowEquipped && !EquipSystem.Instance.CheckIfFull())
            {
                EquipSystem.Instance.AddToQuickSlot(gameObject);
                isNowEquipped = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCaculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

        }
    }

    private void ConsumingFuction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);
        HealthEffectCaculate(healthEffect);
        CaloriesEffectCaculate(caloriesEffect);
        HydrationEffectCaculate(hydrationEffect);
    }

    private void HealthEffectCaculate(float healthEffect)
    {
        float currentHealth = PlayerStats.Instance.currentHealth;
        float maxHealth = PlayerStats.Instance.maxHealth;

        if(healthEffect != 0)
        {
            float healthAfterConsumption = Mathf.Clamp(currentHealth + healthEffect, 0, maxHealth);
            PlayerStats.Instance.SetHealth(healthAfterConsumption);
        }
    }

    private void CaloriesEffectCaculate(float caloriesEffect)
    {
        float currentCalories = PlayerStats.Instance.currentCalories;
        float maxCalories = PlayerStats.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            float caloriesAfterConsumption = Mathf.Clamp(currentCalories + caloriesEffect, 0, maxCalories);
            PlayerStats.Instance.SetCalories(caloriesAfterConsumption);
        }
    }

    private void HydrationEffectCaculate(float hydrationEffect)
    {
        float currentHydration = PlayerStats.Instance.currentHydrationPercent;
        float maxHydration = PlayerStats.Instance.maxHydrationPercent;

        if (hydrationEffect != 0)
        {
            float hydrationAfterConsumption = Mathf.Clamp(currentHydration + hydrationEffect, 0, maxHydration);
            PlayerStats.Instance.SetHydration(hydrationAfterConsumption);
        }
    }

  

}
