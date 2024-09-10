using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEditor;

using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    public float health;

    [SerializeField]
    public Animator enemyAnimator;

    [SerializeField]
    public GameObject healthBar;

    public float deathAnimationLength = 0.50f;

    private float MaxHealth { get; set; }
    private float startingScaleX;

    public HealthScript(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    private void Start()
    {
        // TODO: Race condition here with this and the individual creature.Start() commands
        // If MaxHealth hasn't been set via code, use the value from the inspector (health)
        if (MaxHealth <= 0)
        {
            MaxHealth = health;
        }
        startingScaleX = healthBar.transform.localScale.x;
    }

    public void UpdateHealth(float healthChange)
    {
        health += healthChange;
        if (health <= 0)
        {
            health = 0;
            healthBar.transform.localScale = new Vector3(0, 0, 0);
            enemyAnimator.SetBool("isDead", true);
            Destroy(gameObject, deathAnimationLength);
        }
        var currentScale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3(startingScaleX * health / MaxHealth, currentScale.y, currentScale.z);

    }

    public bool isDead()
    {
        return health <= 0;
    }

}
