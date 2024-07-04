using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [Range(0,1000)]
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform weaponHolder;

    float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            playerBody.Rotate(Vector3.up, mouseX);
            weaponHolder.SetParent(transform);
        }
       

    }
}
