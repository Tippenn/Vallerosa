using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSuki : BossBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= maxHealth * 85 / 100)
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
