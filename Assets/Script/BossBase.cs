using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Ngurus UI untuk boss + healthnya
//behaviour sendiri nanti di inheritance aja soalnya tiap boss rada beda behaviour
public class BossBase : MonoBehaviour,IDamageable
{
    [SerializeField] private RectTransform healthDisplay;
    [SerializeField] private RectTransform healthBGDisplay;

    public float maxHealth;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        healthDisplay.sizeDelta = new Vector2(((currentHealth / maxHealth) * healthBGDisplay.sizeDelta.x), healthBGDisplay.sizeDelta.y);
    }

    public void TakeDamage(float damage)
    {
        
    }
}
