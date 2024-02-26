using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashCooldown;
    public int maxShieldHealth = 100;
    public int currentShieldHealth;
    public int currentCoin;
    private bool isInShield = true;
    public int maxHealth = 100;
    public int currentHealth;
    public Text cooldownText;
    public Text shieldHealth;
    public Text Health;
    public Text Coin;
    private Vector2 move;
    private Camera mainCamera;
    private Animator animator;
    private float dashCooldownTimer;
    private float damageCooldownTimer;
    private bool isDashing;
    private bool isInvulnerable;
    public bool IsDashing => isDashing;
    public bool IsInShield => isInShield;

    public GameObject gameOverPanel;
    public GameObject gameUIPanel;
    public GameObject winScreen;

    

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
        shieldHealth.text = "Current Shield : " + currentShieldHealth;
        Health.text = "Current Health : " + currentHealth;
        Coin.text = "Coin : " + currentCoin;

        if (currentCoin >= 50)
        {
            winScreen.SetActive(true);
            gameUIPanel.SetActive(false);
            Time.timeScale = 0f;
        }

        // Update damage cooldown timer
        if (isInvulnerable)
        {
            damageCooldownTimer -= Time.deltaTime;
            if (damageCooldownTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
        else if (currentShieldHealth > 0)
        {
            isInShield = true;
        }
                
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

    public void TakeShieldDamage(int damage)
    {
        if (!isInvulnerable)
        {
            if (isInShield)
            {
                currentShieldHealth -= damage;
                if (currentShieldHealth <= 0)
                {
                    BreakShield();
                }
                isInvulnerable = true;
                damageCooldownTimer = 1.5f;
            }
        }
    }
    public void ActivateShield()
    {
        isInShield = true;
        currentShieldHealth = maxShieldHealth;
        // TODO: Activate shield visual effect or animation
    }

    // Add this method to break the shield
    private void BreakShield()
    {
        isInShield = false;
        // TODO: Play shield break effect or animation
        // You can also implement cooldown or other mechanics here
    }
    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.Log("Death");
                GameOver();
            }
            isInvulnerable = true;
            damageCooldownTimer = 1.5f;
        }
    }

    public void GameOver()
    {
        // Display the game over panel
        gameOverPanel.SetActive(true);
        gameUIPanel.SetActive(false);
        // Stop time
        Time.timeScale = 0f;

        // Optionally, you can also add code to save the game progress, show the final score, or restart the game
    }
}