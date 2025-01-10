using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BossAtum : BossBase
{
    public Transform ultimateAttackPoint;
    public GameObject ultimatePrefab;
    
    void Update()
    {
        UpdateHealthDisplay();
        if (isBootingUp || isDead)
        {
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRangeAttackRange = Physics.CheckSphere(transform.position, rangeAttackRange, whatIsPlayer);
        playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsPlayer);

        if (currentHealth <= maxHealth * 80 / 100)
        {
            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && playerInSightRange) ChasePlayer();
            if (!playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) UltimateAttackPlayer();
            if (playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) MeleeAttackPlayer();
        }
        else
        {
            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && playerInSightRange) ChasePlayer();
            if (!playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) RangeAttackPlayer();
            if (playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) MeleeAttackPlayer();
        }
    }

    public void UltimateAttackPlayer()
    {
        agent.isStopped = true;
        if (readyToUltimateAttack)
        {
            isAttacking = true;
            // Play attack animation if necessary
            if (animator != null)
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isIdle", false);
                animator.SetTrigger("ultimateAttack");
            }

            readyToUltimateAttack = false;
            Invoke(nameof(ResetUltimateAttack), timeBetweenUltimateAttacks);
        }
    }

    //animation trigger
    public void UltimateAttack()
    {
        Rigidbody rb = Instantiate(ultimatePrefab, ultimateAttackPoint.position, ultimateAttackPoint.rotation).GetComponent<Rigidbody>();
        BulletScript bs = rb.GetComponent<BulletScript>();
        bs.bulletDamage = ultimateAttackDamage;
        rb.transform.forward = attackDirection;
        rb.AddForce(rb.transform.forward * 50f, ForceMode.Impulse);
    }

    public void ResetUltimateAttack()
    {
        isAttacking = false;
        readyToUltimateAttack = true;
        agent.isStopped = false;
    }
}
