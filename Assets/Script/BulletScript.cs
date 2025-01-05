using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, IParryable
{
    public Rigidbody rb;

    public float bulletDamage;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject,3f);
    }


    private void OnCollisionEnter(Collision coll)
    {
        IDamageable damageable = coll.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(bulletDamage);
        }
        Destroy(this.gameObject);
    }

    public void Parried()
    {
        rb.velocity = -rb.velocity;
        transform.forward = rb.velocity.normalized;
    }
    
}
