using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
}
