using UnityEngine;
using UnityEngine.InputSystem;

public class DisableMovement : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void DisableInputForSeconds(float seconds)
    {
        playerInput.DeactivateInput();
        Invoke("EnableInput", seconds);
    }

    private void EnableInput()
    {
        playerInput.ActivateInput();
    }
}