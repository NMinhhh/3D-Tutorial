using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    #region Singleton

    public static SelectionManager Instance { get; private set; }

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

    [SerializeField] private GameObject interaction_Info_UI;
    [SerializeField] private Image centerDotIcon;
    [SerializeField] private Image centerHandIcon;

    public bool isHand {  get; private set; }

    public GameObject selectedObject;

    Text text;
    
    public bool onTarget {  get; private set; }

    [Header("Chopped Tree")]
    public GameObject selectedTree;
    public GameObject choppableHolder;

    void Start()
    {
        text = interaction_Info_UI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            
            if(choppableTree && choppableTree.playerInRange)
            {
                selectedTree = choppableTree.gameObject;
                choppableTree.canBeChopped = true;
                choppableHolder.SetActive(true);
            }
            else
            {
                if(selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    choppableHolder.SetActive(false);
                }
            }


            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                if (interactable.CompareTag("Pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    centerHandIcon.gameObject.SetActive(true);
                    isHand = true;
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    centerHandIcon.gameObject.SetActive(false);
                    isHand = false;
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                centerHandIcon.gameObject.SetActive(false);
                isHand = false;
            }
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            centerHandIcon.gameObject.SetActive(false);
        }
    }

    internal void DisableSelection()
    {
        Instance.GetComponent<SelectionManager>().enabled = false;
        centerDotIcon.gameObject.SetActive(false);
        centerHandIcon.gameObject.SetActive(false);
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }

    internal void EnableSelection()
    {
        Instance.GetComponent<SelectionManager>().enabled = true;
        centerDotIcon.gameObject.SetActive(true);
        if (onTarget)
        {
            centerHandIcon.gameObject.SetActive(true);
            interaction_Info_UI.SetActive(true);
        }
  
    }
}
