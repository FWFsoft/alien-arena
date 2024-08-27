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

    // Isometric movement vectors for diagonal directions
    private Vector2 isoUpRight = new Vector2(1, 0.5f);   // W + D
    private Vector2 isoUpLeft = new Vector2(-1, 0.5f);   // W + A
    private Vector2 isoDownRight = new Vector2(1, -0.5f); // S + D
    private Vector2 isoDownLeft = new Vector2(-1, -0.5f); // S + A

    // Standard movement vectors for cardinal directions
    private Vector2 up = new Vector2(0, 1);    // W
    private Vector2 down = new Vector2(0, -1); // S
    private Vector2 right = new Vector2(1, 0); // D
    private Vector2 left = new Vector2(-1, 0); // A

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

        // Get movement input
        Vector2 moveInput = input.move;

        // Initialize movement
        Vector3 movement = Vector3.zero;

        // Calculate movement based on input
        if (moveInput.y > 0 && moveInput.x > 0) // W + D
        {
            movement = new Vector3(isoUpRight.x, isoUpRight.y, 0);
        }
        else if (moveInput.y > 0 && moveInput.x < 0) // W + A
        {
            movement = new Vector3(isoUpLeft.x, isoUpLeft.y, 0);
        }
        else if (moveInput.y < 0 && moveInput.x > 0) // S + D
        {
            movement = new Vector3(isoDownRight.x, isoDownRight.y, 0);
        }
        else if (moveInput.y < 0 && moveInput.x < 0) // S + A
        {
            movement = new Vector3(isoDownLeft.x, isoDownLeft.y, 0);
        }
        else if (moveInput.y > 0) // W
        {
            movement = new Vector3(up.x, up.y, 0);
        }
        else if (moveInput.y < 0) // S
        {
            movement = new Vector3(down.x, down.y, 0);
        }
        else if (moveInput.x > 0) // D
        {
            movement = new Vector3(right.x, right.y, 0);
        }
        else if (moveInput.x < 0) // A
        {
            movement = new Vector3(left.x, left.y, 0);
        }

        // Normalize movement for consistent speed and apply speed and time
        movement = movement.normalized * speed * Time.deltaTime;

        // Move character
        transform.Translate(movement);

        // Set Animator Parameters
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);

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
