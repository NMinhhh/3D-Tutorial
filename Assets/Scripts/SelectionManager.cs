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


    public GameObject selectedObject;

    Text text;
    
    public bool onTarget {  get; private set; }

    // Start is called before the first frame update
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
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    centerHandIcon.gameObject.SetActive(false);
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
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            centerHandIcon.gameObject.SetActive(false);
        }
    }
}
