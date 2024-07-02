using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string itemName;

    public bool playerInRange;


    private void Update()
    {
        if (Input.GetMouseButton(0) && playerInRange & SelectionManager.Instance.onTarget)
        {
            Debug.Log("Picked up item");
            Destroy(gameObject);
        }
    }

    public string GetItemName()
    {
        return itemName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
