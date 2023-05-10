using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    private DefaultZombieBehaviour defaultBehaviour;
    private BigZombieBehaviour bigBehaviour;

    public GameObject HealthBar;
    public Slider HealthBarSlider;

    public float GetHealth { get { return currentHealth; } }

    private void Awake()
    {
        defaultBehaviour = GetComponent<DefaultZombieBehaviour>();
        bigBehaviour = GetComponent<BigZombieBehaviour>();
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
        if (bigBehaviour != null)
            bigBehaviour.IsShot = true;
        else
            defaultBehaviour.IsShot = true;

        yield return new WaitForSeconds(0.2f);
        if (bigBehaviour != null)
            bigBehaviour.IsShot = false;
        else
            defaultBehaviour.IsShot = false;
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
