using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BossSuki : BossBase
{
    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay();

        if (isDead)
        {
            Dead();
            return;
        }     

        if (!isAttacking)
        {
            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && !playerInSightRange) Idle();
            
        }
        else
        {

        }


        if (currentHealth >= maxHealth * 85 / 100)
        {
            Idle();
        }
        else if (currentHealth <= maxHealth * 50 / 100)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInRangeAttackRange = Physics.CheckSphere(transform.position, rangeAttackRange, whatIsPlayer);
            playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsPlayer);

            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && playerInSightRange) ChasePlayer();
            if (!playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) RangeAttackPlayer();
            if (playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) UltimateAttackPlayer();
        }
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInRangeAttackRange = Physics.CheckSphere(transform.position, rangeAttackRange, whatIsPlayer);
            playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsPlayer);

            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && playerInSightRange) ChasePlayer();
            if (!playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) RangeAttackPlayer();
            if (playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) MeleeAttackPlayer();
        }        
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeAttackRadius);
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, ultimateAttackRadius);
    }


}
