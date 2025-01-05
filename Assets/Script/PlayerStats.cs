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
    public GameObject deadCanvas;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;
    public float currentEnergy;
    public float maxEnergy;
    public float currentMag;
    public float maxMag;
    public bool isDead;
    public bool isWin;

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
        if (PlayerStats.Instance.isDead || PlayerStats.Instance.isWin)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }   
        UpdateHealthDisplay();
        UpdateEnergyDisplay();
        UpdateMagDisplay();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        AddEnergy(15f);
        if (currentHealth < 0)
        {
            Dead();
        }
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void AddEnergy(float energy)
    {
        currentEnergy += energy;
        if(currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
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
    
    public void Dead()
    {
        Time.timeScale = 0f;
        deadCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        isDead = true;
        Invoke(nameof(RestartLevel), 2f);
    }
    public void RestartLevel()
    {
        LevelManager.instance.RestartLevel();
    }
}
