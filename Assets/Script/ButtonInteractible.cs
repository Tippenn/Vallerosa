using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractible : MonoBehaviour,IDamageable
{
    public UnityEvent onShoot;
    public Transform interactibleCanvas;
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interactibleCanvas.LookAt(player);
    }

    public void TakeDamage(float Damage)
    {
        onShoot.Invoke();
    }
}
