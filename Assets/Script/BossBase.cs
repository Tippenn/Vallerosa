using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;

//Ngurus UI untuk boss + healthnya
//behaviour sendiri nanti di inheritance aja soalnya tiap boss rada beda behaviour
public class BossBase : MonoBehaviour,IDamageable
{
    public RectTransform healthDisplay;
    public RectTransform healthBGDisplay;


    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public DungeonEnterTrigger dungeonSource;
    public GameObject winUI;
    private Vector3 lastRootMotionPosition;
    private Quaternion lastRootMotionRotation;

    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;
    public bool isDead;
    public bool isStun;
    public bool isBootingUp;
    public float bloodSpreadRadius;

    //Attacking
    [Header("Melee Attack")]
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public float meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public bool readyToMeleeAttack = true;
    public bool isAttacking = false;
    public Transform meleeAttackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenMeleeAttacks;
    public VisualEffect[] visualEffectMelee;

    [Header("Range Attack")]
    public GameObject rangeAttackPrefab;
    public float rangeAttackDamage = 20;         // Damage dealt by the melee attack
    public bool readyToRangeAttack = true;
    public bool isRangeAttacking = false;
    public Transform rangeAttackPoint;         // Point in front of the player where the attack is centered
    public float timeBetweenRangeAttacks;
    public Vector3 attackDirection;

    [Header("Ultimate Attack")]
    public float ultimateAttackDamage;
    public bool readyToUltimateAttack;
    public float timeBetweenUltimateAttacks;
    public float ultimateAttackRadius;
    public VisualEffect[] visualEffectUltimate;

    private float ultimateAttackCurrDuration;

    public float meleeAttackRange, rangeAttackRange, sightRange;
    public bool playerInMeleeAttackRange, playerInRangeAttackRange, playerInSightRange;
    public GameObject target;

    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        isBootingUp = true;
        StartCoroutine("IsiHealth");
    }

    public void UpdateHealthDisplay()
    {
        if (healthDisplay)
        {
            healthDisplay.sizeDelta = new Vector2(((currentHealth / maxHealth) * healthBGDisplay.sizeDelta.x), healthBGDisplay.sizeDelta.y);
        }
        
    }

    public IEnumerator IsiHealth()
    {
        
        while (currentHealth < maxHealth)
        {
            currentHealth += maxHealth/40;
            yield return null;
        }
        isBootingUp = false;
        
    }

    public void Idle()
    {
        agent.SetDestination(transform.position);
        if (animator != null)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isChasing", false);
        }

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

    public void LockAttackDirection()
    {
        attackDirection = (player.position - rangeAttackPoint.position).normalized;
    }

    //for animation trigger
    public void RangeAttack()
    {
        Rigidbody rb = Instantiate(rangeAttackPrefab, rangeAttackPoint.position, rangeAttackPoint.rotation).GetComponent<Rigidbody>();
        rb.transform.forward = attackDirection;
        rb.AddForce(rb.transform.forward * 25f, ForceMode.Impulse);
    }

    public void ResetRangeAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        readyToRangeAttack = true;

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

        foreach (VisualEffect visualEffect in visualEffectMelee)
        {
            visualEffect.Play();
        }

    }

    public void ResetMeleeAttack()
    {
        isAttacking = false;
        readyToMeleeAttack = true;
        agent.isStopped = false;
    }

    public void TakeDamage(float damage)
    {

        Debug.Log("Orc Taking Damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if(!isDead)
            {
                Destroy(gameObject,2.1f);
                if (dungeonSource != null)
                {
                    dungeonSource.totalDeath++;
                }
                if (animator != null)
                {
                    animator.SetBool("isChasing", false);
                    animator.SetBool("isIdle", false);
                    animator.applyRootMotion = true;
                    animator.SetTrigger("deads");
                }
            }
            isDead = true;
        }
        else
        {

        }
    }

    public void ResetStun()
    {
        isStun = false;
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    #region animation event
    public void EnableRootMotion()
    {
        animator.applyRootMotion = true;
    }

    public void DisableRootMotion()
    {
        lastRootMotionPosition = transform.position+animator.deltaPosition;
        lastRootMotionRotation = transform.rotation*animator.deltaRotation;
        animator.applyRootMotion = false;
        transform.position = lastRootMotionPosition;
        transform.rotation = lastRootMotionRotation;
    }

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            lastRootMotionPosition = transform.position + animator.deltaPosition;
            lastRootMotionRotation = transform.rotation * animator.deltaRotation;

            transform.position = lastRootMotionPosition;
            transform.rotation = lastRootMotionRotation;
        }
    }

    #endregion
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
