using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LavaTouched();
        }
    }

    public void LavaTouched()
    {
        PlayerStats.Instance.TakeDamage(20f);
        CheckpointManager.instance.TPBackToSoftCP();
    }
}
