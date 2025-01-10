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
        if(isBootingUp || isDead)
        {
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRangeAttackRange = Physics.CheckSphere(transform.position, rangeAttackRange, whatIsPlayer);
        playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsPlayer);

        if (currentHealth <= maxHealth * 80 / 100)
        {
            if (!playerInMeleeAttackRange && !playerInRangeAttackRange && playerInSightRange) ChasePlayer();
            if (!playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) RangeAttackPlayer();
            if (playerInMeleeAttackRange && playerInRangeAttackRange && playerInSightRange) UltimateAttackPlayer();
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
        //enemy stop moves
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
        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, ultimateAttackRadius, whatIsPlayer);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponentInParent<IDamageable>() != null)
                enemy.GetComponentInParent<IDamageable>().TakeDamage(ultimateAttackDamage);
        }

        foreach (VisualEffect visualEffect in visualEffectUltimate)
        {
            visualEffect.Play();
        }

    }

    public void ResetUltimateAttack()
    {
        isAttacking = false;
        readyToUltimateAttack = true;
        agent.isStopped = false;
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
