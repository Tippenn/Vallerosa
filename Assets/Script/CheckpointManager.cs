using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    [SerializeField] private HardCheckPoint[] hardCPList;
    [SerializeField] private SoftCheckPoint[] softCPList;
    public int softCPIndex;
    public int hardCPIndex;

    private void Awake()
    {
        instance = this;
    }

    public void TPBackToSoftCP()
    {
        PlayerMovement.Instance.Teleport(softCPList[softCPIndex].TPPosition.position);
    }

    public void SoftCheckpointTriggered(SoftCheckPoint triggeredCheckpoint)
    {
        // Find the index of the checkpoint in the array
        int index = System.Array.IndexOf(softCPList, triggeredCheckpoint);

        if (index == -1)
        {
            Debug.LogError("Triggered checkpoint is not in the manager's array!");
            return;
        }

        if (softCPIndex != index) // Only update if it's a new checkpoint
        {
            softCPIndex = index;
            Debug.Log($"Checkpoint {index} triggered!");
            // Add additional logic here (e.g., save progress, visual feedback, etc.)
        }
    }
}
