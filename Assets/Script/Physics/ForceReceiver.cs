using System;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [Header("Physics Settings")] [SerializeField]
    private float drag = .3f;

    private CharacterController _controller;
    public float _verticalVelocity { get; set; }
    public float _horizontalVelocity { get; set; }
    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    private float _coefficientOfMovement = 1f;
    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;
    public bool IsHoldWall = false;
    public bool IsSlideWall = false;
    public event Action FallingEventAction;

    public bool IsActiveFallingAction;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if (_verticalVelocity < 0f && _controller.isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            if (IsSlideWall)
            {
                _verticalVelocity -= 1f * Time.deltaTime;
            }
            else if (IsHoldWall)
            {
                _verticalVelocity = 0f;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * 3 * Time.deltaTime;
                
                if (IsActiveFallingAction) return;
                FallingEventAction?.Invoke();
                IsActiveFallingAction = true;
            }
        }


        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);
    }

    public void AddImpact(Vector3 force)
    {
        _impact += force;
    }

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;
    }

    public void Dash(float dashForce)
    {
        _horizontalVelocity += dashForce;
    }

    public void SetCoefficientOfMovement(float value)
    {
        _coefficientOfMovement = value;
    }

    public float GetCoefficientOfMovement() => _coefficientOfMovement;
}