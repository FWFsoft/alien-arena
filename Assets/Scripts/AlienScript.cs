using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlienScript : MonoBehaviour
{
    public Animator animator;
    public Animator frothyAnimator;
    public float maximumAttackDistance = 100;
    [SerializeField]
    public HealthScript healthScript;
    public float speed = 3;
    public bool isDead = false;

    private void Update()
    {
        if (healthScript.isDead())
        {
            return;
        }
        var target = findPlayerInRange();
        if(target == null)
        {
            animator.SetBool("isMoving", false);
            // Add patrol in here
        } else
        {
            // Move towards player
            // TODO: Add an attack when a certain distance away
            animator.SetBool("isMoving", true);
            var isLeft = target.transform.position.x < transform.position.x;

            this.transform.rotation = Quaternion.Euler(new Vector3(0f, isLeft ? 180f : 0f, 0f));
            var step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    private GameObject findPlayerInRange()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject closestPlayer = null;
        float closestDistance = maximumAttackDistance;
        foreach (GameObject player in players)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = player;
            }
        }
        return closestPlayer;
    }
}
