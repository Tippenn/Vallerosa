using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDamageable
{
    public static PlayerStats Instance;

    [Header("References")]
    public RectTransform healthDisplay;
    public RectTransform healthBGDisplay;
    public RectTransform energyDisplay;
    public RectTransform energyBGDisplay;
    public RectTransform magDisplay;
    public RectTransform magBGDisplay;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;
    public float currentEnergy;
    public float maxEnergy;
    public float currentMag;
    public float maxMag;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
        currentMag = maxMag;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay();
        UpdateEnergyDisplay();
        UpdateMagDisplay();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Debug.Log("You ded");
        }
    }

    public void UpdateHealthDisplay()
    {
        healthDisplay.sizeDelta = new Vector2 (((currentHealth / maxHealth) * healthBGDisplay.sizeDelta.x) - 2.5f, healthBGDisplay.sizeDelta.y - 2.5f);
    }

    public void UpdateEnergyDisplay()
    {
        energyDisplay.sizeDelta = new Vector2 (((currentEnergy / maxEnergy) * energyBGDisplay.sizeDelta.x) - 2.5f, energyBGDisplay.sizeDelta.y - 2.5f);
    }

    public void UpdateMagDisplay()
    {
        magDisplay.sizeDelta = new Vector2((magBGDisplay.sizeDelta.x), (currentMag / maxMag) * magBGDisplay.sizeDelta.y);
    }
}
