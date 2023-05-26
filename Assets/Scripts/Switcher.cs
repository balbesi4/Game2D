using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public GameObject ComputerTrigger;
    public SpriteRenderer SwitcherSprite;
    public Sprite On;
    public Sprite Off;

    public bool TurnedOn = false;

    public void ChangeSwitcher()
    {
        TurnedOn = !TurnedOn;
        ComputerTrigger.SetActive(TurnedOn);
        SwitcherSprite.sprite = TurnedOn ? On : Off;
    }
}
