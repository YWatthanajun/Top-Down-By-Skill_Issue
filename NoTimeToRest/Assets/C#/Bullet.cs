using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage inflicted by the bullet
    public float lifetime = 2f; // Lifetime of the bullet in seconds

    private Rigidbody rb;
    private float lifeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeTimer = lifetime;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject); // Destroy the bullet when the lifetime expires
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Check if the player is in shield mode
            if (player.IsInShield)
            {
                // Damage the shield instead of the player
                player.TakeShieldDamage(damage);
            }
            else
            {
                // Damage the player directly
                player.TakeDamage(damage);
            }
        }
        else if (!collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Destroy the bullet if it collides with something other than the player or another bullet
        }

    }
}
