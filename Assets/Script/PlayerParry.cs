using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    public Transform parryPoint;
    public float parryRadius;
    public bool readyToParry = true;
    public float parryCooldown = 0.5f;
    public KeyCode parryKey;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(parryKey) && readyToParry)
        {
            Debug.Log("attacking");
            readyToParry = false;
            animator.SetTrigger("Parry");
            Invoke(nameof(ResetParry), parryCooldown);
        }
    }

    public void Parry()
    {
        Collider[] hitUnit = Physics.OverlapSphere(parryPoint.position, parryRadius);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider unit in hitUnit)
        {
            IParryable par = unit.GetComponent<IParryable>();
            if (par != null)
            {
                PlayerStats.Instance.AddEnergy(25f);
                par.Parried();
            }
        }
    }

    public void ResetParry()
    {
        readyToParry = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (parryPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(parryPoint.position, parryRadius);
        Gizmos.DrawWireSphere(parryPoint.position, parryRadius / 2);
    }
}
