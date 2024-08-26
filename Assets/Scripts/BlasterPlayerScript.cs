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

        // Calculate movement
        Vector2 moveInput = input.move;
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime;

        // Move character
        transform.Translate(movement);

        // Set Animator Parameters
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);

        // Debug logs to see the values
        Debug.Log($"DirectionX: {moveInput.x}, DirectionY: {moveInput.y}");
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