using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret04 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 15f;
    public float DelayShoot = 0.3f;
    public float selfDestructDelay = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", DelayShoot);
    }


    void Shoot()
    {
        FireSingleShot();
        Invoke("DestroyTurret", selfDestructDelay);
    }

    void FireSingleShot()
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
