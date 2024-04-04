using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashCooldown;
    public int maxShieldHealth = 100;
    public int winCollectCoin;
    public int currentShieldHealth;
    public int currentCoin;
    public int percentCoin;
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
    public bool checkPlayerisDead = false;

    public bool IsDashing => isDashing;
    public bool IsInShield => isInShield;

    public GameObject gameOverPanel;
    public GameObject gameUIPanel;
    public GameObject winScreen;

    public AudioSource audioSource;
    public AudioSource audioBackgroundSource;
    private int soundwin = 0;

    public PlayableDirector DeathCutscenedirector;
    public float cutsceneDuration = 6f;
    public GameObject DeathcutsceneCamera;

    public PlayableDirector WinCutscenedirector;
    public float WincutsceneDuration = 6f;
    public GameObject WincutsceneCamera;
    public GameObject WincutsceneCamera01;
    public GameObject WincutsceneGameObject;

    public GameObject EnemySpawner;
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
        percentCoin = ((currentCoin * 100) / winCollectCoin);

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

            WinCutscenedirector.Play();

            StartCoroutine(PlayWinCutscene());
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

    IEnumerator PlayWinCutscene()
    {
        // Set the time scale to 1
        Time.timeScale = 1f;

        // Wait for the cutscene duration
        yield return new WaitForSecondsRealtime(WincutsceneDuration);

        // Set the time scale back to 0
        Time.timeScale = 0f;

        // Set the cutscene camera's parent to inactive
        WincutsceneCamera.SetActive(false);
        WincutsceneCamera01.SetActive(false);
        WincutsceneGameObject.SetActive(false);
        EnemySpawner.SetActive(false);
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

        // Check if gameOverPanel or winScreen is active
        if (gameOverPanel.activeSelf || winScreen.activeSelf)
        {
            return;
        }

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
        checkPlayerisDead = true;
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

        // Play the cutscene
        DeathCutscenedirector.Play();

        // Start the coroutine to play the cutscene
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Set the time scale to 1
        Time.timeScale = 1f;

        // Wait for the cutscene duration
        yield return new WaitForSecondsRealtime(cutsceneDuration);

        // Set the time scale back to 0
        Time.timeScale = 0f;

        // Set the cutscene camera's parent to inactive
        DeathcutsceneCamera.SetActive(false);
        EnemySpawner.SetActive(false);
    }

    IEnumerator BlinkPlayer()
    {
        GameObject playerTrail = GameObject.FindGameObjectWithTag("PlayerTrail");
        if (playerTrail != null)
        {
            bool previousEnabledState = playerTrail.activeSelf;
            for (int i = 0; i < 15; i++)
            {
                // Set the game object's scale to zero to make it disappear
                this.transform.localScale = new Vector3(0, 0, 0);
                playerTrail.SetActive(false);
                yield return new WaitForSeconds(0.1f);

                // Set the game object's scale back to its original value to make it reappear
                this.transform.localScale = new Vector3(1, 1, 1);
                playerTrail.SetActive(previousEnabledState);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                // Set the game object's scale to zero to make it disappear
                this.transform.localScale = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(0.1f);

                // Set the game object's scale back to its original value to make it reappear
                this.transform.localScale = new Vector3(1, 1, 1);
                yield return new WaitForSeconds(0.1f);
            }
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
