using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    [SerializeField] private Text healthCounter;

    private Slider slider;

    public float currentHydration, maxHydration;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = PlayerStats.Instance.currentHydrationPercent;
        maxHydration = PlayerStats.Instance.maxHydrationPercent;

        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;
        healthCounter.text = currentHydration + "%";
    }
}
