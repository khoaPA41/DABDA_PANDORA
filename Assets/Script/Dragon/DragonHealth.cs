using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using Object = UnityEngine.Object;

public class DragonHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private string destroyVfxName;
    [SerializeField] private bool isPlayer;

    private float _currentHealth;

    public bool isImmortal {get; set;}
    public event Action OnDeath;
    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isImmortal) return;
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0f, maxHealth);

        if (_currentHealth <= 0f)
        {
            OnDeath?.Invoke();
            ActiveDestroyVFX();
        }
    }

    public void Healing(float healAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0f, maxHealth);
    }

    private void ActiveDestroyVFX()
    {
        ObjectPooling.Instance.GetPooledObject(destroyVfxName, transform.position).GetComponent<ParticleSystem>().Play();

        if (isPlayer)
        {
            GameManager.Instance.ReturnCheckpoint();
        }
        gameObject.SetActive(false);
    }
}