using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth = 100;
    private DefaultZombieBehaviour behaviour;

    public GameObject HealthBar;
    public Slider HealthBarSlider;

    public float GetHealth { get { return currentHealth; } }

    private void Awake()
    {
        behaviour = GetComponent<DefaultZombieBehaviour>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        HealthBar.SetActive(true);
        currentHealth -= damage;
        HealthBarSlider.value = GetHealthPercentage();
        StartCoroutine(ReactOnDamage());
        CheckDeath();
    }

    private IEnumerator ReactOnDamage()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.red;
        behaviour.IsShot = true;

        yield return new WaitForSeconds(0.2f);
        behaviour.IsShot = false;
        sprite.color = Color.white;
    }

    private float GetHealthPercentage() => currentHealth / maxHealth;

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }
}
