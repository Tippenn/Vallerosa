using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, IParryable
{
    public Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        IDamageable damageable = coll.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(10f);
        }
        Destroy(this.gameObject);
    }

    public void Parried()
    {
        rb.velocity = -rb.velocity;
        transform.forward = rb.velocity.normalized;
    }
    
}
