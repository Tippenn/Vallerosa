using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;
    public ParticleSystem muzzleFlash;
    private Animator animator;
    public PlayerStats stats;

    [Header("Melee Attack")]
    public float meleeAttackRange = 2f;        // The maximum range of the melee attack
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public float meleeAttackCooldown = 1f;     // Time between attacks
    public bool readyToMeleeAttack = true;
    public Transform melleeAttackPoint;         // Point in front of the player where the attack is centered
    public LayerMask enemyLayers;         // Layer mask to detect enemies
    public KeyCode meleeAttackKey = KeyCode.V;

    [Header("Range Attack")]
    public Camera fpsCamera;
    public int rangeAttackDamage = 10;         // Damage dealt by the ranged attack
    public float rangeAttackRange = 50f;       // Maximum range of the attack
    public float rangeAttackCooldown = 0.6f;     // Time between attacks   
    public bool readyToRangeAttack = true;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public bool isCharging;
    public float rangeChargeRate = 15f;
    public Transform rangeAttackPoint;
    public KeyCode rangeAttackKey = KeyCode.Mouse0;

    private void Awake()
    {
        Instance = this;
        stats = GetComponent<PlayerStats>();
        melleeAttackPoint = GameObject.Find("MeleeAttackPoint").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        muzzleFlash = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();

    }

    void Update()
    {
        if (Input.GetKeyDown(rangeAttackKey) && readyToRangeAttack && stats.currentMag > 5f)
        {
            readyToRangeAttack = false;
            Debug.Log("nembak");
            RangeAttack();

            Invoke(nameof(ResetRangeAttack), rangeAttackCooldown);
        }
        // Trigger attack on left mouse click (or customize the key)
        if (Input.GetKeyDown(KeyCode.V) && readyToMeleeAttack)
        {
            readyToMeleeAttack = false;
            MeleeAttack();
            Invoke(nameof(ResetMeleeAttack), meleeAttackCooldown);
        }

        //Charging Weapon
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCharging = true;
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            isCharging = false;
        }

        if (isCharging)
        {
            ChargeWeapon();
        }
    }

    void MeleeAttack()
    {
        // Play attack animation if necessary
        // animator.SetTrigger("Attack");

        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(melleeAttackPoint.position, meleeAttackRadius);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider unit in hitEnemies)
        {
            Debug.Log("adaUnit");
            IDamageable damageable = unit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("attacking");
                damageable.TakeDamage(meleeAttackDamage);
            }
        }

        // (Optional) Visual feedback like attack sound or particle effects
    }

    void RangeAttack()
    {
        muzzleFlash.Play();

        animator.SetTrigger("Shoot");

        stats.currentMag -= 5f;
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, rangeAttackRange))
        {
            // Try to get the IDamageable component from the hit object
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            // If the hit object has the IDamageable component, deal damage
            if (damageable != null)
            {
                damageable.TakeDamage(rangeAttackDamage);
            }

            // You can also add other hit effects (e.g., bullet impact visuals)
            Debug.Log(hit.transform.name + " was hit!");
        }

    }


    public void ChargeWeapon()
    {
        // Increment the charge over time, clamped to maxCharge
        if(stats.currentEnergy >= 0 && stats.currentMag < stats.maxMag)
        {
            stats.currentEnergy -= rangeChargeRate * Time.deltaTime;
            stats.currentEnergy = Mathf.Clamp(stats.currentEnergy, 0f, stats.maxEnergy);

            stats.currentMag += rangeChargeRate * Time.deltaTime;
            stats.currentMag = Mathf.Clamp(stats.currentMag, 0f, stats.maxMag);
        }
        

        Debug.Log("Charging: " + stats.currentEnergy);
    }

    public void ResetRangeAttack()
    {
        readyToRangeAttack = true;
    }

    public void ResetMeleeAttack()
    {
        readyToMeleeAttack = true;
    }

    //Visualize the attack range in the scene view for debugging purposes
    private void OnDrawGizmosSelected()
    {
        if (melleeAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(melleeAttackPoint.position, meleeAttackRadius);
    }
}
