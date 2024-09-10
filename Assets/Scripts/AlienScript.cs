using System.Collections;
using System.Collections.Generic;

using Creatures.Api;

using UnityEditor;

using UnityEngine;

public class AlienScript : Creature
{
    public Animator frothyAnimator;

    private void Update()
    {
        if (HealthScript.isDead())
        {
            return;
        }
        var target = findPlayerInRange();
        if (target == null)
        {
            Animator.SetBool("isMoving", false);
            // Add patrol in here
        }
        else
        {
            // Move towards player
            // TODO: Add an attack when a certain distance away
            Animator.SetBool("isMoving", true);
            var isLeft = target.transform.position.x < transform.position.x;

            this.transform.rotation = Quaternion.Euler(new Vector3(0f, isLeft ? 180f : 0f, 0f));
            var step = Speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    private GameObject findPlayerInRange()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject closestPlayer = null;
        float closestDistance = MaximumAttackDistance;
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
