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
    [SerializeField] private LayerMask matchTargetLayer;

    [SerializeField] private Transform footTransform;
    [SerializeField] private Transform bodyTransform;

    [SerializeField] private float footSphereCastRadius;
    [SerializeField] private float bodySphereCastRadius;
    [SerializeField] private float matchTargetCastRadius;

    
    [SerializeField] private float bodyCastDistance;
    [SerializeField] private float matchTargetCastDistance;

    private CharacterController _controller;
    public float _verticalVelocity { get; set; }
    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    private float _coefficientOfMovement = 1f;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;
    
    public bool IsActiveFallingAction;
    public bool IsDash;
    
    public bool IsGrounded { get; private set; }
    public bool IsClimbing { get; set; }
    public bool IsHoldWall { get; set; }
    public bool IsSlideWall { get; set; }
    public bool IsMatchTarget { get; set; }

    public event Action FallingEventAction;
    public event Action OnClimbedAction;
    public bool IsCallClimbedAction { get; set; }
    public Vector3 SurfaceNormal { get; set; }

    public event Action OnHoldWallAction;
    public event Action OnSlideWallAction;
    public event Action<Vector3> OnMatchTargetAction;

    private PlayerAudio _playerAudio;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerAudio = GetComponent<PlayerAudio>();
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
                    _verticalVelocity = -1f;
                }
                else if (IsHoldWall || IsClimbing)
                {
                    _verticalVelocity = 0f;
                }
                else
                {
                    CheckMatchTarget();
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
    
    private void CheckGround()
    {
        IsGrounded = Physics.CheckSphere(footTransform.position, footSphereCastRadius, groundLayer);;
        var hit = CheckSphereCast(footTransform, footSphereCastRadius, -transform.up, groundLayer, bodyCastDistance);

        if (hit.collider is null) return;
        
        var sharedMaterial = hit.collider.sharedMaterial;

        _playerAudio.groundType = sharedMaterial.name switch
        {
            "Grass" => GroundType.Grass,
            "Wood" => GroundType.Wood,
            "Rock" => GroundType.Rock,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void CheckClimb()
    {
        var hit = CheckSphereCast(bodyTransform, bodySphereCastRadius, transform.forward, climbLayer, bodyCastDistance);
        IsClimbing = hit.collider is not null;
        
        SurfaceNormal = hit.normal;

        if (IsCallClimbedAction) return;
        if (!IsClimbing) return;
        IsCallClimbedAction = true;
        OnClimbedAction?.Invoke();
    }
    
    private void CheckHoldWall()
    {
        if (IsHoldWall) return;
        var hit =  CheckSphereCast(bodyTransform, bodySphereCastRadius, transform.forward, holdWallLayer, bodyCastDistance);
        
        IsHoldWall = hit.collider != null;
        transform.SetParent(hit.transform);
        
        if (!IsHoldWall) return;
        OnHoldWallAction?.Invoke();
    }
    
    private void CheckSlideWall()
    {
        if (IsSlideWall) return;
        
        var hit =  CheckSphereCast(bodyTransform, bodySphereCastRadius, transform.forward, slideWallLayer, bodyCastDistance);
        IsSlideWall = hit.collider != null;
        
        if (!IsSlideWall) return;
        OnSlideWallAction?.Invoke();
    }

    private void CheckMatchTarget()
    {
        var hit = CheckSphereCast(footTransform, matchTargetCastRadius, -transform.up, matchTargetLayer, matchTargetCastDistance);
        IsMatchTarget = hit.collider is not null;
        OnMatchTargetAction?.Invoke(hit.point);
    }

    private static RaycastHit CheckSphereCast(Transform pos, float radius, Vector3 direction, LayerMask mask, float distance)
    {
        Physics.SphereCast(pos.position, radius, direction, out var hit, distance, mask);
        return hit;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw foot sphere cast
        Gizmos.DrawWireSphere(footTransform.position, footSphereCastRadius);

        // Draw body sphere cast
        Gizmos.DrawWireSphere(bodyTransform.position, bodySphereCastRadius);
        
        // Draw match target sphere cast
        
        var endPosition = footTransform.position + (-transform.up * matchTargetCastDistance);
        
        Gizmos.DrawWireSphere(endPosition, matchTargetCastRadius);
        Gizmos.DrawLine(footTransform.position,  endPosition);
    }
}