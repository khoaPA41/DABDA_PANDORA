using System;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [Header("Physics Settings")] [SerializeField]
    private float drag = .3f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;
    [SerializeField] private LayerMask holdWallLayer;
    [SerializeField] private LayerMask slideWallLayer;

    [SerializeField] private Transform footTransform;
    [SerializeField] private Transform bodyTransform;

    [SerializeField] private float footSphereCastRadius;
    [SerializeField] private float bodySphereCastRadius;


    private CharacterController _controller;
    public float _verticalVelocity { get; set; }
    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    private float _coefficientOfMovement = 1f;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;
    
    public event Action FallingEventAction;
    public bool IsActiveFallingAction;
    public bool IsDash;
    
    public bool IsGrounded { get; private set; }
    public bool IsClimbing { get; set; }
    public bool IsHoldWall { get; set; }
    public bool IsSlideWall { get; set; }
    

    public event Action OnClimbedAction;
    public event Action OnHoldWallAction;
    public event Action OnSlideWallAction;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGround();
        CheckClimb();
        CheckHoldWall();
        CheckSlideWall();
        if (!IsDash)
        {
            if (_verticalVelocity < 0f && IsGrounded)
            {
                _verticalVelocity = -2f;
            }
            else
            {
                if (IsSlideWall)
                {
                    _verticalVelocity += Physics.gravity.y * Time.deltaTime;
                }
                else if (IsHoldWall || IsClimbing)
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
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);
    }

    public void AddImpact(Vector3 force)
    {
        _impact += force;
        IsDash = true;
    }

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;
    }

    public void SetCoefficientOfMovement(float value)
    {
        _coefficientOfMovement = value;
    }

    public float GetCoefficientOfMovement() => _coefficientOfMovement;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw foot sphere cast
        Gizmos.DrawWireSphere(footTransform.position, footSphereCastRadius);

        // Draw body sphere cast
        Gizmos.DrawWireSphere(bodyTransform.position, bodySphereCastRadius);
    }

    private void CheckGround()
    {
        IsGrounded = Physics.CheckSphere(footTransform.position, footSphereCastRadius, groundLayer);
    }

    private void CheckClimb()
    {
        if (IsClimbing) return;
        IsClimbing = Physics.CheckSphere(bodyTransform.position, bodySphereCastRadius, climbLayer);
        if (!IsClimbing) return;
        OnClimbedAction?.Invoke();
    }
    
    private void CheckHoldWall()
    {
        if (IsHoldWall) return;
        
        RaycastHit hit =  CheckSphereCast(bodySphereCastRadius, holdWallLayer);
        
        IsHoldWall = hit.collider != null;
        transform.SetParent(hit.transform);
        
        if (!IsHoldWall) return;
        OnHoldWallAction?.Invoke();
    }
    
    private void CheckSlideWall()
    {
        if (IsSlideWall) return;
        
        RaycastHit hit =  CheckSphereCast(bodySphereCastRadius, slideWallLayer);
        IsSlideWall = hit.collider != null;
        
        if (!IsSlideWall) return;
        OnSlideWallAction?.Invoke();
    }

    private RaycastHit CheckSphereCast(float radius, LayerMask mask)
    {
        Physics.SphereCast(bodyTransform.position, bodySphereCastRadius, transform.forward, out RaycastHit hit, radius, mask);
        return hit;
    }
    
}