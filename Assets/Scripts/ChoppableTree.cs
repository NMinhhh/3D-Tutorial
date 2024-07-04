using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    [SerializeField] private float maxHealth;
    private float currentHealth;

    [SerializeField] private float caloriesToChoppedTree;

    private Animator anim;

    void Start()
    {
        anim = transform.parent.transform.parent.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.currentResourceHealth = currentHealth;
            GlobalState.Instance.maxResourceHealth = maxHealth;
        }
    }

    public void GetHit()
    {
        anim.SetTrigger("shake");
        currentHealth -= 1;
        PlayerStats.Instance.currentCalories -= caloriesToChoppedTree;
        if (currentHealth <= 0)
        {
            TreeIsDead();
        }
    }

    private void TreeIsDead()
    {
        Vector3 pos = transform.position;
        canBeChopped = false;
        Destroy(transform.parent.transform.parent.gameObject);
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.choppableHolder.SetActive(false);
        Instantiate(Resources.Load<GameObject>("Model/ChoppedTree"), new Vector3(pos.x, pos.y + 1, pos.z), Quaternion.Euler(0, 0, 0));

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
