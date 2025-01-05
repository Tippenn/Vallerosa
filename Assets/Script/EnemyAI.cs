using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyAI : MonoBehaviour,IDamageable
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public CaveMechanic caveSource;
    public DungeonEnterTrigger dungeonSource;
    public enum MonsterType
    {
        range,
        melee
    }

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;
    public bool isDead;
    public bool isStun;
    public float stunDuration;
    public float bloodSpreadRadius;
    public MonsterType type;

    //Attacking
    [Header("Melee Attack")]
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public bool readyToMeleeAttack = true;
    public bool isAttacking = false;
    public Transform meleeAttackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenMeleeAttacks;
    public VisualEffect[] visualEffectMelee;

    [Header("Range Attack")]
    public GameObject rangeAttackPrefab;
    public int rangeAttackDamage = 20;         // Damage dealt by the melee attack
    public bool readyToRangeAttack = true;
    public bool isRangeAttacking = false;
    public Transform rangeAttackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenRangeAttacks;

    public float meleeAttackRange,rangeAttackRange,sightRange;
    public bool playerInMeleeAttackRange, playerInRangeAttackRange, playerInSightRange;
    public GameObject target;


    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Start()
    {

    }

    void Update()
    {
        if (isDead)
        {
            Dead();
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRangeAttackRange = Physics.CheckSphere(transform.position, rangeAttackRange, whatIsPlayer);
        playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsPlayer);

        if(!isAttacking)
        {
            if(type == MonsterType.melee)
            {
                if (!playerInMeleeAttackRange && !playerInSightRange) Idle();
                if (!playerInMeleeAttackRange && playerInSightRange) ChasePlayer();
                if (playerInMeleeAttackRange && playerInSightRange && type == MonsterType.melee) MeleeAttackPlayer();
            }
            else
            {
                if (!playerInRangeAttackRange && !playerInSightRange) Idle();
                if (!playerInRangeAttackRange && playerInSightRange) ChasePlayer();
                if (playerInRangeAttackRange && playerInSightRange) RangeAttackPlayer();
            }
        }
        else
        {

        }
    }

    public void Idle()
    {
        Debug.Log("idle");
        agent.SetDestination(transform.position);
        if(animator != null)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isChasing", false);
        }
        
    }

    public void ChasePlayer()
    {
        Debug.Log("chasing");
        agent.SetDestination(player.position);
        if (animator != null)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isIdle", false);
        }
    }

    public void RangeAttackPlayer()
    {
        //enemy stop moves
        //agent.SetDestination(transform.position);
        agent.isStopped = true;
        if (readyToRangeAttack)
        {
            isAttacking = true;
            // Play attack animation if necessary
            if (animator != null)
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isIdle", false);
                animator.SetTrigger("rangeAttack");
            }           

            readyToRangeAttack = false;
            Invoke(nameof(ResetRangeAttack), timeBetweenRangeAttacks);
        }
    }

    //for animation trigger
    public void RangeAttack()
    {
        Rigidbody rb = Instantiate(rangeAttackPrefab, rangeAttackPoint.position, rangeAttackPoint.rotation).GetComponent<Rigidbody>();

        rb.AddForce(rb.transform.forward * 12f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2f, ForceMode.Impulse);
    }

    public void ResetRangeAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        readyToRangeAttack = true;
        transform.LookAt(player.position);
    }

    public void MeleeAttackPlayer()
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
                animator.SetTrigger("meleeAttack");
            }

            readyToMeleeAttack = false;
            Invoke(nameof(ResetMeleeAttack), timeBetweenMeleeAttacks);
        }
    }

    //animation trigger
    public void MeleeAttack()
    {
        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, meleeAttackRadius, whatIsPlayer);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("AI is attacking");
            if (enemy.GetComponentInParent<IDamageable>() != null)
                enemy.GetComponentInParent<IDamageable>().TakeDamage(meleeAttackDamage);
        }

        foreach(VisualEffect visualEffect in visualEffectMelee)
        {
            visualEffect.Play();
        }
        
    }

    public void ResetMeleeAttack()
    {
        isAttacking = false;
        readyToMeleeAttack = true;
        agent.isStopped = false;
        transform.LookAt(player.position);
    }

    public void TakeDamage(float damage)
    {

        Debug.Log("Orc Taking Damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {           
            if(!isDead)
            {
                if (caveSource != null)
                {
                    caveSource.totalSpawn--;

                }
                if (dungeonSource != null)
                {
                    dungeonSource.totalDeath++;
                }
                if (animator != null)
                {
                    animator.SetBool("isChasing", false);
                    animator.SetBool("isIdle", false);
                    animator.SetTrigger("deads");
                }
            }
            isDead = true;
        }
        else
        {
            if (animator != null)
            {
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, bloodSpreadRadius, whatIsPlayer);
                if(hitEnemies != null)
                {
                    PlayerStats.Instance.AddHealth(5f);
                }             
                Invoke(nameof(ResetStun), stunDuration);
            }
        }
    }
    public void Dead()
    {
        agent.isStopped = true;
    }

    public void ResetStun()
    {
        isStun = false;
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    public void AssignCave(CaveMechanic cave)
    {
        caveSource = cave;
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
        Gizmos.DrawWireSphere(transform.position, bloodSpreadRadius);
    }
}
