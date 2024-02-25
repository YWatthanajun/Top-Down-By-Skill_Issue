using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 15f;
    public float DelayShoot = 0.3f;
    public float selfDestructDelay = 1.5f;
    public int burstCount = 3; // Number of bullets per burst
    public int spreadCount = 5; // Number of bullets per spread
    public int spiralCount = 4; // Number of bullets per spiral

    void Start()
    {
        
        Invoke("Shoot", DelayShoot);
    }
    void Shoot()
    {
        int randomPattern = Random.Range(0, 4); // Randomly select a pattern from 0 to 3 (inclusive)

        switch (randomPattern) //Fire the selected pattern
        {
            case 0:
                FireSingleShot();
                break;
            case 1:
                FireBurst();
                break;
            case 2:
                FireSpread();
                break;
            case 3:
                FireSpiral();
                break;
        }
        Invoke("DestroyTurret", selfDestructDelay);
    }

    void FireBurst()
    {
        // Calculate the spacing between each bullet based on the burst count
        float spacingBetweenBullets = 1f;

        // Start firing from the initial position of the fire point
        Vector3 startPosition = firePoint.position;

        // Fire each bullet with a slight position offset
        for (int i = 0; i < burstCount; i++)
        {
            // Calculate the position offset for this bullet
            Vector3 bulletPosition = startPosition + firePoint.forward * spacingBetweenBullets * i;

            // Fire the bullet at the calculated position
            FireBulletBurst(bulletPosition);
        }
    }

    void FireSpread()
    {
        // Calculate the angle between each bullet based on the burst count
        float angleBetweenBullets = 360f / spreadCount;

        // Start firing from the initial rotation of the fire point
        Quaternion startRotation = firePoint.rotation;

        // Fire each bullet with a slight rotation offset
        for (int i = 0; i < spreadCount; i++)
        {
            // Calculate the rotation offset for this bullet
            Quaternion bulletRotation = Quaternion.Euler(0f, angleBetweenBullets * i, 0f);

            // Apply the rotation offset to the start rotation
            Quaternion finalRotation = startRotation * bulletRotation;

            // Fire the bullet with the final rotation
            FireBulletSpread(finalRotation);
        }
    }

    void FireSpiral()
    {
        float bulletAngleIncrement = 10f; // Angle increment between bullets
        float bulletRadiusIncrement = 0f; // Radius increment between bullets
        float bulletRadius = -30f; // Initial radius of the spiral

        for (int i = 0; i < spiralCount; i++)
        {
            // Calculate the angle for this bullet
            float bulletAngle = i * bulletAngleIncrement;

            // Calculate the position for this bullet in a spiral pattern
            float x = Mathf.Cos(Mathf.Deg2Rad * bulletAngle) * bulletRadius;
            float z = Mathf.Sin(Mathf.Deg2Rad * bulletAngle) * bulletRadius;
            Vector3 bulletPosition = firePoint.position + new Vector3(x, 0f, z);

            // Fire the bullet at the calculated position with curved trajectory
            FireBulletSpiral(bulletPosition, bulletAngle);

            // Increment the radius for the next bullet
            bulletRadius += bulletRadiusIncrement;
        }
    }

    void FireSingleShot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    void FireBulletBurst(Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
    }

    void FireBulletSpread(Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    void FireBulletSpiral(Vector3 position, float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Calculate the direction vector of the bullet
        Vector3 direction = (firePoint.position - position).normalized;

        // Apply a rotation to the direction to create a curved trajectory
        float curveAngle = angle * 0.1f; // Adjust the curvature by multiplying angle
        Quaternion curveRotation = Quaternion.AngleAxis(curveAngle, Vector3.up);
        direction = curveRotation * direction;

        // Apply the curved direction to the bullet's velocity
        rb.velocity = direction * bulletSpeed;
    }

    void DestroyTurret()
    {
        // Destroy the turret GameObject
        Destroy(gameObject);
    }
}
