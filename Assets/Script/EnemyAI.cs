using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour,IDamageable
{
    public Animator animator;
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
    public bool isAttacking = false;
    public Transform attackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenAttacks;

    public float attackRange,sightRange;
    public bool playerInAttackRange,playerInSightRange;
    public GameObject target;


    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!isAttacking)
        {
            if (!playerInAttackRange && !playerInSightRange) Idle();
            if (!playerInAttackRange && playerInSightRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
        else
        {

        }
    }

    public void Idle()
    {
        if(animator != null)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isChasing", false);
        }
        
    }

    public void Patrol()
    {

    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
        if (animator != null)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isIdle", false);
        }
    }

    public void AttackPlayer()
    {
        //enemy stop moves
        agent.isStopped = true;

        if (readyToMeleeAttack)
        {
            isAttacking = true;
            // Play attack animation if necessary
            if (animator != null)
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isIdle", false);
                animator.SetTrigger("Attack");
            }

            // Create a sphere in front of the player to detect enemies in the attack range
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, meleeAttackRadius, whatIsPlayer);

            // Apply damage to all enemies hit by the sphere
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("AI is attacking");
                if (enemy.GetComponentInParent<IDamageable>() != null)
                    enemy.GetComponentInParent<IDamageable>().TakeDamage(meleeAttackDamage);
            }

            // (Optional) Visual feedback like attack sound or particle effects


            readyToMeleeAttack = false;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        readyToMeleeAttack = true;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Orc Taking Damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
