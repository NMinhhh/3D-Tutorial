using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen 
            && !CraftingSystem.Instance.isOpen && !SelectionManager.Instance.isHand)
        {
            StartCoroutine(DelaySwingSound());
            anim.SetTrigger("hit");
        }
    }

    void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator DelaySwingSound()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }
}
