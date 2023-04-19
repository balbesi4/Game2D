using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth = 100;

    public GameObject HealthBar;
    public Slider HealthBarSlider;
    public AudioSource DamageSound;

    public float GetHealth { get { return currentHealth; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        HealthBar.SetActive(true);
        currentHealth -= damage;
        DamageSound.Play();
        HealthBarSlider.value = GetHealthPercentage();
        StartCoroutine(StopFromDamage());
        CheckDeath();
    }

    private IEnumerator StopFromDamage()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sprite.color = Color.white;
    }

    private float GetHealthPercentage() => currentHealth / maxHealth;

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DamageSound.Play();
            //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GetComponent<DefaultZombieBehaviour>().CalculateVelocity(new Vector3(0, 0, 0), 0);
            Destroy(gameObject, 0.2f);
        }
    }
}
