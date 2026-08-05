using System;
using UnityEngine;
using UnityEngine.Audio;

public enum GroundType
{
    Rock,
    Wood,
    Grass
}

public class PlayerAudio : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource footAudioSource;
    [SerializeField] private AudioSource bodyAudioSource;

    [Header("Ground Sound")]
    [SerializeField] private AudioResource groundMoveResource;
    [SerializeField] private AudioResource groundJumpResource;
    [SerializeField] private AudioResource groundLandingResource;

    [Header("Rock Sound")]
    [SerializeField] private AudioResource rockMoveResource;
    [SerializeField] private AudioResource rockJumpResource;
    [SerializeField] private AudioResource rockLandingResource;
    
    [Header("Wood Sound")]
    [SerializeField] private AudioResource woodMoveResource;
    [SerializeField] private AudioResource woodJumpResource;
    [SerializeField] private AudioResource woodLandingResource;
    
    [Header("Get item Sound")]
    [SerializeField] private AudioClip getItemClip;
    [SerializeField] private AudioClip interactGate;
    [SerializeField] private AudioClip lookAtGate;
    [SerializeField] private AudioClip throwBulletSound;
    
    public GroundType groundType =  GroundType.Rock;
    public bool isGround;

    public void MoveSound()
    {
        footAudioSource.resource = groundType switch
        {
            GroundType.Rock => rockMoveResource,
            GroundType.Wood => woodMoveResource,
            GroundType.Grass => groundMoveResource,
            _ => throw new ArgumentOutOfRangeException()
        };
        footAudioSource.Play();
    }
    
    public void JumpSound()
    {
        footAudioSource.resource = groundType switch
        {
            GroundType.Rock => rockJumpResource,
            GroundType.Wood => woodJumpResource,
            GroundType.Grass => groundJumpResource,
            _ => throw new ArgumentOutOfRangeException()
        };
        footAudioSource.Play();
    }
    
    public void LandingSound()
    {
        footAudioSource.resource = groundType switch
        {
            GroundType.Rock => rockLandingResource,
            GroundType.Wood => woodLandingResource,
            GroundType.Grass => groundLandingResource,
            _ => throw new ArgumentOutOfRangeException()
        };
        footAudioSource.Play();
    }

    public void GetItemSound()
    {
        bodyAudioSource.clip = getItemClip;
        bodyAudioSource.Play();
    }
    
    public void InteractGateSound()
    {
        bodyAudioSource.clip = interactGate;
        bodyAudioSource.Play();
    }
    public void LookAtGateSound()
    {
        bodyAudioSource.clip = lookAtGate;
        bodyAudioSource.Play();
    }
    
    public void ThrowBulletSound()
    {
        bodyAudioSource.Stop();
        bodyAudioSource.clip = throwBulletSound;
        bodyAudioSource.Play();
    }
}
