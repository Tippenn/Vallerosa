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
    public enum Gunmode
    {
        single,
        burst
    }

    [Header("Melee Attack")]
    public float meleeAttackRange = 2f;        // The maximum range of the melee attack
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public float meleeAttackCooldown = 1f;     // Time between attacks
    public bool readyToMeleeAttack = true;
    [SerializeField] private Transform meleeAttackPoint;         // Point in front of the player where the attack is centered
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
    public GameObject VFXBullet;
    //burst mode
    public float rangeAttackBurstCooldown;
    public float rangeAttackBurstInterval = 0.1f;
    public float rangeAttackBurstDamage = 5f;
    public float rangeAttackBurstDrain = 10f;
    public bool readyToBurst = true;
    public LineRenderer lineRenderer;
    //switch weapon mode
    public Gunmode gunmode = Gunmode.single;
    public KeyCode switchModeKey = KeyCode.Q;

    //reload
    public bool isChargeSound;
    public float chargeSoundCooldown;
    public float chargeSoundDuration;

    private void Awake()
    {
        Instance = this;
        stats = GetComponent<PlayerStats>();
        //meleeAttackPoint = GameObject.Find("MeleeAttackPoint").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        muzzleFlash = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();

    }

    void Update()
    {
        if (PlayerStats.Instance.isDead || PlayerStats.Instance.isWin)
        {
            return;
        }

        if (LevelManager.instance)
        {
            if (LevelManager.instance.isPaused)
            {
                return;
            }
        }
        #region range
        if (gunmode == Gunmode.single)
        {
            if (Input.GetKeyDown(rangeAttackKey) && readyToRangeAttack && stats.currentMag > 5f)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.singleShot);
                readyToRangeAttack = false;
                Debug.Log("nembak");
                RangeAttack();

                Invoke(nameof(ResetRangeAttack), rangeAttackCooldown);
            }
        }
        else
        {
            if(!readyToBurst)
            {
                rangeAttackBurstCooldown -= Time.deltaTime;
                if(rangeAttackBurstCooldown <= 0)
                {
                    readyToBurst = true;
                }
            }
            if (Input.GetKey(rangeAttackKey) && readyToRangeAttack && !isCharging)
            {
                if(stats.currentMag > 0f)
                {
                    RangeBurstAttack();
                }
                else
                {
                    lineRenderer.enabled = false;
                }
                
            }
            if(Input.GetKeyUp(rangeAttackKey))
            {
                lineRenderer.enabled = false;
            }
        }
        
        #endregion

        #region melee
        // Trigger attack on left mouse click (or customize the key)
        if (Input.GetKeyDown(meleeAttackKey) && readyToMeleeAttack)
        {
            readyToMeleeAttack = false;
            PlayMeleeAttack();
            Invoke(nameof(ResetMeleeAttack), meleeAttackCooldown);
        }
        #endregion

        #region charging
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCharging = true;          
            animator.SetTrigger("Reload");
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            isCharging = false;
            animator.SetBool("isReloading", false);
        }

        if (isCharging)
        {
            ChargeWeapon();
            if(isChargeSound)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.reload);
                isChargeSound = false;
            }
            else
            {
                chargeSoundDuration += Time.deltaTime;
                if(chargeSoundCooldown <= chargeSoundDuration)
                {
                    isChargeSound = true;
                    chargeSoundDuration = 0f;
                }
            }
        }
        #endregion

        #region switch mode
        //if (Input.GetKeyDown(switchModeKey))
        //{
        //    SwitchGunMode();
        //}

        #endregion
    }
    public void PlayMeleeAttack()
    {
        animator.SetTrigger("Attack");
    }

    //animation trigger
    public void MeleeAttack()
    {
        //cancel reload if reloading
        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("isReloading", false);
        }

        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, meleeAttackRadius, enemyLayers);

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

    public void RangeAttack()
    {
        muzzleFlash.Play();

        animator.SetTrigger("Shoot");
            
        //cancel reload if reloading
        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("isReloading", false);
        }
        
        Rigidbody rb = Instantiate(VFXBullet, rangeAttackPoint.position, fpsCamera.transform.rotation).GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward*10f);
        Destroy(rb.gameObject,1.5f);

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

    public void RangeBurstAttack()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0,rangeAttackPoint.position);
        stats.currentMag -= rangeAttackBurstDrain * Time.deltaTime;
        if (!readyToBurst) return;
        
        RaycastHit hit;

        // Perform the Raycast
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, rangeAttackRange))
        {
            // Try to get the IDamageable component from the hit object
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            // If the hit object has the IDamageable component, deal damage
            if (damageable != null)
            {
                damageable.TakeDamage(rangeAttackBurstDamage);
                readyToBurst = false;
                rangeAttackBurstCooldown = rangeAttackBurstInterval;
            }
            lineRenderer.SetPosition(1, hit.point);
            // You can also add other hit effects (e.g., bullet impact visuals)
            Debug.Log(hit.transform.name + " was hit!");
        }
        else
        {
            Vector3 forwardDirection = fpsCamera.transform.forward;
            lineRenderer.SetPosition(1, rangeAttackPoint.position + forwardDirection * rangeAttackRange);
        }
    }

    public void ChargeWeapon()
    {
        // Increment the charge over time, clamped to maxCharge
        if(stats.currentEnergy > 0 && stats.currentMag < stats.maxMag)
        {
            animator.SetBool("isReloading", true);
            stats.currentEnergy -= rangeChargeRate * Time.deltaTime;
            stats.currentEnergy = Mathf.Clamp(stats.currentEnergy, 0f, stats.maxEnergy);

            stats.currentMag += rangeChargeRate * Time.deltaTime;
            stats.currentMag = Mathf.Clamp(stats.currentMag, 0f, stats.maxMag);
        }
        else
        {
           isCharging = false;
            animator.SetBool("isReloading", false);
        }
        

        //Debug.Log("Charging: " + stats.currentEnergy);
    }

    public void ResetRangeAttack()
    {
        readyToRangeAttack = true;
    }

    public void ResetMeleeAttack()
    {
        //Debug.Log("wtf");
        readyToMeleeAttack = true;
    }

    public void SwitchGunMode()
    {
        if(gunmode == Gunmode.single)
        {
            gunmode = Gunmode.burst;
        }
        else
        {
            gunmode = Gunmode.single;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeAttackRadius);
    }
}
