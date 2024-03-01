using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f; // Time between shots
    public float bulletSpeed = 15f;
    public float delayShoot = 0.3f; // Initial delay before starting to shoot
    public float selfDestructDelay = 1.5f; // Time before the turret self-destructs after starting to shoot
    public int bulletCount = 20; // Number of bullets to fire
    public float rotationAngle = 45f; // Angle to rotate the fire point after each shot

    void Start()
    {
        Invoke("Shoot", delayShoot);
    }
    void Shoot()
    {
        StartCoroutine(ShootRotation());
    }

    IEnumerator ShootRotation()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            FireSpiral();
            RotateFirePoint();
            yield return new WaitForSeconds(fireRate); // Wait for the specified fire rate before continuing the loop
        }
        Invoke("DestroyTurret", selfDestructDelay);
    }

    void FireSpiral()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    void RotateFirePoint()
    {
        firePoint.Rotate(0, rotationAngle, 5);
    }

    void DestroyTurret()
    {
        Destroy(gameObject);
    }
}
