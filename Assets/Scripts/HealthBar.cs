using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text healthCounter;

    private Slider slider;
    
    public float currentHealth, maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = PlayerStats.Instance.currentHealth;
        maxHealth = PlayerStats.Instance.maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
