using System.Collections;
using UnityEngine;

public class PlayerHealthManagement : MonoBehaviour
{
    public GameObject DeathPanel;
    public GameObject NotificationPanel;
    public GameObject Game;
    public AudioClip DamageSound;
    public AudioClip DeathSound;
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
        AudioSource.PlayClipAtPoint(DamageSound, Camera.main.transform.position);
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
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position);
            Health = 0;
            NotificationPanel.SetActive(false);
            DeathPanel.SetActive(true);
            Game.SetActive(false);
        }
    }
}
