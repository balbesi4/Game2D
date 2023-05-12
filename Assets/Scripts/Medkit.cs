using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public float HealPoints;
    public string MessageUI;

    private void Start()
    {
        MessageUI = $"Аптечка (+{HealPoints} здоровья)";
    }
}
