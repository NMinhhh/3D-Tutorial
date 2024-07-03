using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    [SerializeField] private Text healthCounter;

    private Slider slider;

    public float currentCalories, maxCalories;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCalories = PlayerStats.Instance.currentCalories;
        maxCalories = PlayerStats.Instance.maxCalories;

        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;
        healthCounter.text = currentCalories + "/" + maxCalories;
    }
}
