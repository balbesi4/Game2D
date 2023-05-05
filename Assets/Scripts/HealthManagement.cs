using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    private DefaultZombieBehaviour behaviour;

    public GameObject HealthBar;
    public Slider HealthBarSlider;
    public AudioClip DamageSound;

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
        AudioSource.PlayClipAtPoint(DamageSound, Camera.main.gameObject.transform.position);
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
