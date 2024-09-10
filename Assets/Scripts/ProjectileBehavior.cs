using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Vector2 explosionPosition;
    [SerializeField]
    public Animator projectileAnimator;
    [SerializeField]
    public CapsuleCollider2D collider2D;

    public Rigidbody2D rigidBody;
    public float timeToLive = 3f;
    public float destructionAnimationLength = 0.3f;

    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        var distance = Vector2.Distance(explosionPosition, new Vector2(transform.position.x, transform.position.y));
        // MousePosition
        //Vector2.Distance(new Vector2())
        if (distance < 0.1)
        {
            TriggerCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerCollision();
    }

    private void TriggerCollision()
    {
        projectileAnimator.SetBool("collided", true);
        collider2D.size = new Vector2(1, 1);
        rigidBody.velocity = new Vector2(0, 0);
        // Make collider bigger
        Destroy(gameObject, destructionAnimationLength);
    }
}
