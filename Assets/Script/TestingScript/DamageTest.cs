using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTest : MonoBehaviour, IDamageable
{
    public TMP_Text damageDisplay;

    public float damageAmount = 0f;
    public float resetTime = 5f;
    public float currentTime = 0f;

    private void Awake()
    {
        damageDisplay = GetComponentInChildren<TMP_Text>();
    }
    private void Start()
    {
        UpdateDamageDisplay();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > resetTime)
        {
            currentTime = 0f;
            ResetDamage();
        }
    }
    public void TakeDamage(float damage)
    {
        damageAmount += damage;
        currentTime = 0f;
        UpdateDamageDisplay();
    }

    public void UpdateDamageDisplay()
    {
        damageDisplay.text = damageAmount.ToString();
    }

    public void ResetDamage()
    {
        damageAmount = 0f;
        UpdateDamageDisplay();
    }
}
