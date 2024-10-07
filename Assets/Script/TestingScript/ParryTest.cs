using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryTest : MonoBehaviour
{
    [Header("Range Attack")]
    public int rangeAttackDamage = 10;         // Damage dealt by the ranged attack
    public float rangeAttackCooldown = 1f;     // Time between attacks
    public float currentAttackCooldown = 0f;
    public bool readyToRangeAttack = true;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public Transform attackPoint;         // Point in front of the player where the attack is centered

    void Update()
    {
        currentAttackCooldown += Time.deltaTime;
        if(currentAttackCooldown >= rangeAttackCooldown)
        {
            RangeAttack();
            currentAttackCooldown = 0f;
        }
    }

    void RangeAttack()
    {
        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(attackPoint.forward * bulletSpeed, ForceMode.Impulse);
    }
}
