using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedTest : MonoBehaviour
{
    public Rigidbody rb;
    public TMP_Text speedDebug;

    private void Awake()
    {
        speedDebug = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        speedDebug.text = "Speed :" + rb.velocity.magnitude.ToString();
    }
}
