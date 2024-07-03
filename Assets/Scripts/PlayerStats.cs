using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Singleton
    public static PlayerStats Instance {  get; private set; }

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

    [Header("Player health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Player Calories")]
    public float maxCalories;
    public float currentCalories;
    private float distanceTravelled;
    private Vector3 lastPosition;
    [SerializeField] private Transform playerBody;

    [Header("Player Hydration")]
    public float maxHydrationPercent;
    public float currentHydrationPercent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        StartCoroutine(DecreaseHydration());
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTravelled >= 10)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }
    }

    IEnumerator DecreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
    }

    public void SetCalories(float calories)
    {
        currentCalories = calories;
    }

    public void SetHydration(float hydration)
    {
        currentHydrationPercent = hydration;
    }
}
