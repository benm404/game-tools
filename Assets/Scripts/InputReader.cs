using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerController.IMovementActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;
    public Action OnHidePerformed;
    public Action OnDashPerformed;

    private PlayerController controls;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new PlayerController();
        controls.Movement.SetCallbacks(this);
        controls.Movement.Enable();
    }

    public void OnDisable()
    {
        controls.Movement.Disable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnDashPerformed?.Invoke();
    }

    public void OnHide(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnHidePerformed?.Invoke();
    }
}