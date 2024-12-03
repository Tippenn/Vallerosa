using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{

    [Header("References")]
    public RectTransform healthDisplay;
    public RectTransform healthBGDisplay;
    public EnemyAI enemyAI;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        currentHealth = enemyAI.currentHealth;
        maxHealth = enemyAI.maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay();       
    }

    public void UpdateHealthDisplay()
    {
        currentHealth = enemyAI.currentHealth;
        maxHealth = enemyAI.maxHealth;
        healthDisplay.sizeDelta = new Vector2((currentHealth / maxHealth) * healthBGDisplay.sizeDelta.x, healthBGDisplay.sizeDelta.y);
        transform.LookAt(enemyAI.player);
    }
}
