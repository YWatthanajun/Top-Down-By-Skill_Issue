using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret03 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 15f;
    public float DelayShoot = 0.3f;
    public float selfDestructDelay = 1.5f;
    public int bulletCount = 3; // Number of bullets per burst

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", DelayShoot);
    }


    void Shoot()
    {
        FireBurst();
        Invoke("DestroyTurret", selfDestructDelay);
    }

    void FireBurst()
    {
        // Calculate the spacing between each bullet based on the burst count
        float spacingBetweenBullets = 1f;

        // Start firing from the initial position of the fire point
        Vector3 startPosition = firePoint.position;

        // Fire each bullet with a slight position offset
        for (int i = 0; i < bulletCount; i++)
        {
            // Calculate the position offset for this bullet
            Vector3 bulletPosition = startPosition + firePoint.forward * spacingBetweenBullets * i;

            // Fire the bullet at the calculated position
            FireBulletBurst(bulletPosition);
        }
    }
    void FireBulletBurst(Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
    }
    void DestroyTurret()
    {
        // Destroy the turret GameObject
        Destroy(gameObject);
    }
}
