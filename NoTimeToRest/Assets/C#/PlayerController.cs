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
    public int winCollectCoin = 50;
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
    private bool isImmortal = false;
    private bool isImmortalValue = false;

    public bool IsDashing => isDashing;
    public bool IsInShield => isInShield;

    public GameObject gameOverPanel;
    public GameObject gameUIPanel;
    public GameObject winScreen;

    public AudioSource audioSource;
    public AudioSource audioBackgroundSource;
    private int soundwin = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        dashCooldownTimer = dashCooldown;
        Time.timeScale = 1f;

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
        Coin.text = "Coin : " + currentCoin + "  /  " + winCollectCoin;

        if (currentCoin >= winCollectCoin)
        {
            // Pause the audio source
            if (audioBackgroundSource != null)
            {
                audioSource.Pause();
                audioBackgroundSource.Pause();
            }
            if (soundwin == 0)
            {
                soundwin++;
                SoundManager2.instance.audioSoundSource.PlayOneShot(SoundManager2.instance.winSound);
            }
            winScreen.SetActive(true);
            gameUIPanel.SetActive(false);
            Time.timeScale = 0f;
        }

        // Update damage cooldown timer
        if (isInvulnerable && !isImmortal)
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
        newPosition.x = Mathf.Clamp(newPosition.x, -199f, 199f); // X-axis constraints
        newPosition.z = Mathf.Clamp(newPosition.z, -199f, 199f); // Z-axis constraints

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
        targetPosition.x = Mathf.Clamp(targetPosition.x, -199f, 199f); // X-axis constraints
        targetPosition.z = Mathf.Clamp(targetPosition.z, -199f, 199f); // Z-axis constraints

        while (Time.time < startTime + dashTime)
        {
            float t = (Time.time - startTime) / dashTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            SoundManager.instance.audioSoundSource.PlayOneShot(SoundManager.instance.DashSound);
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
                SoundManager.instance.audioSoundSource.PlayOneShot(SoundManager.instance.breakShieldSound);
                currentShieldHealth -= damage;
                if (currentShieldHealth <= 0)
                {
                    BreakShield();
                }
                isInvulnerable = true;
                damageCooldownTimer = 1.5f;
                StartCoroutine(BlinkPlayer());
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
            SoundManager.instance.audioSoundSource.PlayOneShot(SoundManager.instance.damageSound);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                GameOver();
            }
            isInvulnerable = true;
            damageCooldownTimer = 1.5f;
            StartCoroutine(BlinkPlayer());

        }
    }

    public void GameOver()
    {
        if (audioBackgroundSource != null)
        {
            audioSource.Pause();
            audioBackgroundSource.Pause();
        }
        Debug.Log("Death");
        SoundManager2.instance.audioSoundSource.PlayOneShot(SoundManager2.instance.overSound);
        // Display the game over panel
        gameOverPanel.SetActive(true);
        gameUIPanel.SetActive(false);
        // Stop time
        Time.timeScale = 0f;

        // Optionally, you can also add code to save the game progress, show the final score, or restart the game
    }

    IEnumerator BlinkPlayer()
    {
        for (int i = 0; i < 15; i++) // Increase the number of iterations to 15 for a total of 1.5 seconds
        {
            // Set the game object's scale to zero to make it disappear
            this.transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.1f);

            // Set the game object's scale back to its original value to make it reappear
            this.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Invulnerable()
    {
        if (isImmortalValue)
        {
            Debug.Log("(Immortal = off)");
            isInvulnerable = false;
            isImmortal = false;
            isImmortalValue = false;
        }
        else 
        {
            Debug.Log("(Immortal = on)");
            isInvulnerable = true;
            isImmortal = true;
            isImmortalValue = true;
        }
    }
}