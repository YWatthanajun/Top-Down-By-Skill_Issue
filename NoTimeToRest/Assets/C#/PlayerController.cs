using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashCooldown;
    public Text cooldownText;
    private Vector2 move;
    private Camera mainCamera;
    private Animator animator;
    private float dashCooldownTimer;
    private bool isDashing;
    public bool IsDashing => isDashing;


    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        dashCooldownTimer = dashCooldown;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && dashCooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
        }
    }

    void Update()
    {
        movePlayer();
        lookAtCursor();
        dashCooldownTimer -= Time.deltaTime;
        cooldownText.text = "Cooldown Dash : " + Mathf.Max(0, Mathf.Ceil(dashCooldownTimer)).ToString("0");
    }

    public void movePlayer()
    {
        // Calculate movement vector
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        // Update player position with constraints
        Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -300f, 300f); // X-axis constraints
        newPosition.z = Mathf.Clamp(newPosition.z, -300f, 300f); // Z-axis constraints

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is not dashing
            if (!collision.gameObject.GetComponent<PlayerController>().IsDashing)
            {
                Debug.Log("Hit");
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float dashTime = 0.2f;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        Vector3 dashDirection = new Vector3(move.x, 0f, move.y);
        Vector3 targetPosition = startPosition + dashDirection * dashSpeed;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -300f, 300f); // X-axis constraints
        targetPosition.z = Mathf.Clamp(targetPosition.z, -300f, 300f); // Z-axis constraints

        while (Time.time < startTime + dashTime)
        {
            float t = (Time.time - startTime) / dashTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

}