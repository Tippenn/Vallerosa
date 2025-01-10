using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftCheckPoint : MonoBehaviour
{
    public Transform TPPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.instance.SoftCheckpointTriggered(this);
        }
    }
}
