using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    // Reference to the player GameObject
    public Transform player;

    // Update is called once per frame

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check if player reference is not null
        if (player != null)
        {
            // Calculate the direction from this GameObject to the player
            Vector3 direction = player.position - transform.position;

            // Calculate the rotation to look at the player
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to this GameObject
            transform.rotation = rotation;
        }

    }
}
