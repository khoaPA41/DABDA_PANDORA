using System;
using System.Collections;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class DragonHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private string destroyVfxName;
    [SerializeField] private bool isPlayer;

    [Header("Health UI")] [SerializeField] private Image healthFill;
    [SerializeField] private float timeToUpdateHealthUI;
    private float _currentHealth;

    public bool isImmortal {get; set;}
    public event Action OnDeath;
    public event Action<float> ChangeHealthAction;
    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        healthFill.fillAmount = _currentHealth;
        ChangeHealthAction += UpdateHealthUI;
    }

    private void OnDisable()
    {
        ChangeHealthAction -= UpdateHealthUI;
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
        ChangeHealthAction?.Invoke(_currentHealth);
    }

    public void Healing(float healAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0f, maxHealth);
        ChangeHealthAction?.Invoke(_currentHealth);
    }

    private void UpdateHealthUI(float amount)
    {
        StartCoroutine(UpdateHealth(amount));
    }

    private IEnumerator UpdateHealth(float healthAmount)
    {
        var elapsedTime = 0f;
        var currentFillAmount = healthFill.fillAmount;
        var targetFillAmount = healthAmount / maxHealth;
        while (elapsedTime < timeToUpdateHealthUI)
        {
            elapsedTime += Time.deltaTime;
            var timePercentage = Mathf.Clamp01(elapsedTime / timeToUpdateHealthUI);
            healthFill.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, timePercentage);
            Debug.Log(healthFill.fillAmount);
            yield return null;
        }

        healthFill.fillAmount = targetFillAmount;
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