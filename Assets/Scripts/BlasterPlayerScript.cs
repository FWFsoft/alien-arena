using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlasterPlayerScript : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    public float speed = 5.0f;
    [SerializeField]
    Animator animator;
    public float playerHealthPercentage = 1;
    public float maxPlayerHealth = 100;
    private float playerHealth;
    public GameObject blasterCharacter;
    public Transform launchOffset;
    bool movingLeft = false;

    void Start()
    {
        this.playerHealth = maxPlayerHealth;
        transform.position = new Vector3(0, 0, 0);
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Paused
        if (Time.timeScale == 0)
        {
            return;
        }
        // Set animation
        bool isMoving = input.move.x != 0 || input.move.y != 0;
        animator.SetBool("isMoving", isMoving);

        // Set scale for animation direction
        if (isMoving)
        {
            movingLeft = input.move.x < 0;
        }
        int direction = movingLeft ? 1 : -1;
        Vector3 directionVector = new Vector3(direction * Mathf.Abs(blasterCharacter.transform.localScale.x), 
            blasterCharacter.transform.localScale.y, 
            blasterCharacter.transform.localScale.z);
        blasterCharacter.transform.localScale = directionVector;

        // Move character
        Vector3 newPosition = new Vector3(input.move.x, input.move.y, 0) * speed * Time.deltaTime;
        transform.Translate(newPosition);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Receive damage
        // TODO: Based on target
        playerHealth -= 10;
        if (playerHealth == 0)
        {
            print("YOU DIED!");
            return;
        }
        playerHealthPercentage = playerHealth / maxPlayerHealth;
    }
}
