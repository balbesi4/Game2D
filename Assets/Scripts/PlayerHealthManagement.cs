using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManagement : MonoBehaviour
{
    private SpriteRenderer sprite;

    public float MaxHealth { get; private set; }
    public float Health { get; private set; }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        MaxHealth = 100;
        Health = MaxHealth;
    }

    public void Heal(float healValue)
    {
        Health = healValue + Health > MaxHealth ? MaxHealth : healValue + Health;
    }

    public void DealDamage(float damage)
    {
        Health -= damage;
        StartCoroutine(DamageAnimation());
        CheckDeath();
    }

    private IEnumerator DamageAnimation()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sprite.color = Color.white;
    }

    private void CheckDeath()
    {
        if (Health <= 0)
        {
            Health = 0;
            var scene = SceneManager.GetActiveScene().name;
            Destroy(gameObject);
            SceneManager.LoadScene(scene);
        }
    }
}
