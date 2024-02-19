using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move;
    private Camera mainCamera;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Update()
    {
        movePlayer();
        lookAtCursor();
    }

    public void movePlayer()
    {
        // Calculate movement vector
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        // Update player position with constraints
        Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -39.5f, 39.5f); // X-axis constraints
        newPosition.z = Mathf.Clamp(newPosition.z, -39.5f, 39.5f); // Z-axis constraints

        // Apply the constrained position
        transform.position = newPosition;

        // Set the IsMoving parameter to true or false
        animator.SetBool("IsMoving", movement != Vector3.zero);
    }

    public void lookAtCursor()
    {
        // Get the world position of the cursor
        Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));

        // Calculate the direction vector from the player to the cursor
        Vector3 lookDirection = cursorWorldPosition - transform.position;

        // Update player rotation
        if (lookDirection != Vector3.zero)
        {
            // Calculate the rotation around the y-axis only
            Quaternion targetRotation = Quaternion.Euler(0f, Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg, 0f);

            // Slerp to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }
}