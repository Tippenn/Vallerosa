using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //public MuzzleFlash flash;

    [Header("Melee Attack")]
    public float meleeAttackRange = 2f;        // The maximum range of the melee attack
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public float meleeAttackCooldown = 1f;     // Time between attacks
    public bool readyToMeleeAttack = true;
    public Transform attackPoint;         // Point in front of the player where the attack is centered
    public LayerMask enemyLayers;         // Layer mask to detect enemies

    [Header("Range Attack")]
    public int rangeAttackDamage = 10;         // Damage dealt by the ranged attack
    public float rangeAttackRange = 50f;       // Maximum range of the attack
    public float rangeAttackCooldown = 0.6f;     // Time between attacks
    public bool readyToRangeAttack = true;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private void Awake()
    {
        attackPoint = GameObject.Find("MeleeAttackPoint").GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && readyToRangeAttack)
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
    }

    void MeleeAttack()
    {
        // Play attack animation if necessary
        // animator.SetTrigger("Attack");

        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, meleeAttackRadius);

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
        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(attackPoint.forward * bulletSpeed, ForceMode.Impulse);
        
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
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRadius);
    }
}
