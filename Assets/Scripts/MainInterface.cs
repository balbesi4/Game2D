using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInterface : MonoBehaviour
{
    public PlayerHealthManagement PlayerHealth;
    public Slider PlayerHealthBar;
    public Text HealthText;

    private void Start()
    {
        HealthText.text = "100";
    }

    private void Update()
    {
        PlayerHealthBar.value = PlayerHealth.Health / PlayerHealth.MaxHealth;
        HealthText.text = $"{PlayerHealth.Health}";
    }
}
