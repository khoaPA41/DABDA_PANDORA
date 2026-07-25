using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Input.IPlayerActions
{
    public Vector2 Movement { get; private set; }
    public Vector2 Look { get; private set; }
    public bool IsSprint { get; private set; }
    public bool IsInteract { get; set; }
    public bool IsAttack { get; set; }

    public bool CursorLocked { get; set; } = true;
    public event Action JumpAction = delegate { };
    public event Action DashAction = delegate { };
    public event Action CrouchAction = delegate { };

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
        // Debug.Log(Movement);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            IsAttack = false;
            Debug.Log(IsAttack);
            return;
        }
        IsAttack = true;
        
        Debug.Log(IsAttack);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsInteract = true;
            return;
        }
        IsInteract = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;
        CrouchAction?.Invoke();
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
    
    public void SetCursor()
    {
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
