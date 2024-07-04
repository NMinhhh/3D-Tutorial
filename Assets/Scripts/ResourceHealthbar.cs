using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthbar : MonoBehaviour
{
    [SerializeField] private Text healthCounter;

    private Slider slider;

    public float currentResourceHealth, maxResourceHealth;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentResourceHealth = GlobalState.Instance.currentResourceHealth;
        maxResourceHealth = GlobalState.Instance.maxResourceHealth;

        float fillValue = currentResourceHealth / maxResourceHealth;
        slider.value = fillValue;
    }
}
