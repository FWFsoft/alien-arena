using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    public float health;

    [SerializeField]
    private Animator enemyAnimator;

    [SerializeField]
    private GameObject healthBar;

    public float deathAnimationLength = 0.50f;

    private float maxHealth;
    private float startingScaleX;

    private void Start()
    {
        maxHealth = health;
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
        healthBar.transform.localScale = new Vector3(startingScaleX * health / maxHealth, currentScale.y, currentScale.z);

    }

    public bool isDead()
    {
        return health <= 0;
    }

}
