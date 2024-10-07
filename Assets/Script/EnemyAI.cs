using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;

    //Attacking
    [Header("Melee Attack")]
    public float meleeAttackRange = 2f;        // The maximum range of the melee attack
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public bool readyToMeleeAttack = true;
    public Transform attackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float attackRange;
    public bool playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("CharObj").transform;
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    public void Patrol()
    {

    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        //enemy stop moves
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            // Play attack animation if necessary
            // animator.SetTrigger("Attack");

            // Create a sphere in front of the player to detect enemies in the attack range
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, meleeAttackRadius, whatIsPlayer);

            // Apply damage to all enemies hit by the sphere
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("AI is attacking");
                if (enemy.GetComponentInParent<PlayerHealth>() != null)
                    enemy.GetComponentInParent<PlayerHealth>().TakeDamage(meleeAttackDamage);
            }

            // (Optional) Visual feedback like attack sound or particle effects


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRadius);
    }
}
