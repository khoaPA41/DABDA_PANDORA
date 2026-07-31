using System;
using System.Collections;
using UnityEngine;

public class DragonBuffHandler : MonoBehaviour
{
    [SerializeField] private float immortalTime = 3f;
    [SerializeField] private float buffProjectileTime = 3f;

    private DragonHealth _dragonHealth;
    private DragonController _dragonController;

    private void Start()
    {
        _dragonHealth = GetComponent<DragonHealth>();
        _dragonController =  GetComponent<DragonController>();
    }


    public void ActiveBaseOnBuff(BuffType type)
    {
        switch (type)
        {
            case BuffType.Healing:
                Debug.Log("Healing");
                _dragonHealth.Healing(50f);
                break;
            case BuffType.Immortal:
                Debug.Log("Immortal");
                StartCoroutine(CountdownImmortal(immortalTime));
                break;
            case BuffType.Projectile:
                Debug.Log("Projectile");
                StartCoroutine(CountdownBuffProjectile(buffProjectileTime));
                break;
        } 
        
    }
    
    IEnumerator CountdownImmortal(float time)
    {
        _dragonHealth.isImmortal = true;
        yield return new WaitForSecondsRealtime(time);
        _dragonHealth.isImmortal = false;
    }

    IEnumerator CountdownBuffProjectile(float time)
    {
        _dragonController.isBuffProjectile = true;
        yield return new WaitForSecondsRealtime(time);
        _dragonController.isBuffProjectile = false;
    }
}
