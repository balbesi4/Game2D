using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorCoatController : MonoBehaviour
{
    public GameObject CutScenePanel;
    public GameObject Player;
    public GameObject MainMusic;

    private InventoryManagement inventory;

    private void Start()
    {
        inventory = FindObjectOfType<InventoryManagement>();
    }

    public void OpenBook()
    {
        Player.GetComponent<PlayerAnimation>().IsFreezed = true;
        Player.GetComponent<PlayerMovement>().IsFreezed = true;
        Player.GetComponent<ShootingControl>().IsFreezed = true;
        inventory.IsFreezed = true;

        MainMusic.SetActive(false);
        CutScenePanel.SetActive(true);
    }
}
