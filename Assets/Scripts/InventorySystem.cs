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

    public bool isOpen {  get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            inventoryScreen.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryScreen.SetActive(false);
        }
    }
}
