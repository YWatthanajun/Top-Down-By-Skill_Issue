using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret02 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 15f;
    public float DelayShoot = 0.3f;
    public float selfDestructDelay = 1.5f;
    public int bulletCount = 5; // Number of bullets per burst

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", DelayShoot);
    }

    void Shoot()
    {
        FireSpread();
        Invoke("DestroyTurret", selfDestructDelay);
    }

    void FireSpread()
    {
        // Calculate the angle between each bullet based on the burst count
        float angleBetweenBullets = 360f / bulletCount;

        // Start firing from the initial rotation of the fire point
        Quaternion startRotation = firePoint.rotation;

        // Fire each bullet with a slight rotation offset
        for (int i = 0; i < bulletCount; i++)
        {
            // Calculate the rotation offset for this bullet
            Quaternion bulletRotation = Quaternion.Euler(0f, angleBetweenBullets * i, 0f);

            // Apply the rotation offset to the start rotation
            Quaternion finalRotation = startRotation * bulletRotation;

            // Fire the bullet with the final rotation
            FireBulletSpread(finalRotation);
        }
    }
    void FireBulletSpread(Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }
    void DestroyTurret()
    {
        // Destroy the turret GameObject
        Destroy(gameObject);
    }
}
