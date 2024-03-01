using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage inflicted by the bullet
    public float lifetime = 2f; // Lifetime of the bullet in seconds

    private float lifeTimer;

    void Start()
    {
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

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
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Destroy the bullet if it collides with something other than the player or another bullet
        }
    }
}
