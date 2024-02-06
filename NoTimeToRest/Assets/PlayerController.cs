using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private float jumpPower;
    public float speed;
    private Vector2 move;
    //private bool isJumping;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    //public void Jump(InputAction.CallbackContext context)
    //{
    //    if (context.performed && !isJumping)
    //    {
    //        isJumping = true;
    //        Rigidbody rb = GetComponent<Rigidbody>();
    //        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }


}
