using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Input.IPlayerActions
{
    public Vector2 Movement { get; private set; }
    public bool IsSprint { get; private set; }
    public event Action JumpAction = delegate { };
    public event Action DashAction = delegate { };

    private Input _inputActions;
    
    private void Awake()
    {
        _inputActions = new Input();
        
        _inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
        Debug.Log(Movement);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;
        JumpAction?.Invoke();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            IsSprint = false;
            return;
        }
        IsSprint = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;
        DashAction?.Invoke();
    }
}
