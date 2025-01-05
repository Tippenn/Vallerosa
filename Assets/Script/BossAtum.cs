using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAtum : BossBase
{
    private void Start()
    {        
        StartCoroutine(nameof(IsiHealth));
    }
    
    void Update()
    {
        UpdateHealthDisplay();
        if (currentHealth <= maxHealth * 90 / 100)
        {
            //not attack
        }
        else if (currentHealth <= maxHealth * 50 / 100)
        {
            //ulti gatot kaca
        }
        else
        {
            //marah + ngecharge
            //UI enemy biasa?
        }
    }
}
