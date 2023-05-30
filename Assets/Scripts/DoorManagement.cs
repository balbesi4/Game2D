using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DoorManagement : MonoBehaviour
{
    public GameObject Doors;
    public GameObject Zombie;
    public GameObject DropItem;
    public GameObject Enemies;
    public GameObject Objects;
    public float[] RoomBorders;
    public HintSprite ThisHintSprite;
    public int ZombiesToSpawn;

    private List<HealthManagement> zombies;
    private Vector3 lastZombiePos;
    private bool isDropped;

    public int ZombieCount
    {
        get
        {
            return zombies == null
                ? -1
                : zombies.Where(zombie => zombie != null).Count();
        }
    }

    public void StartRoomAction()
    {
        zombies = new List<HealthManagement>();
        Doors.SetActive(true);
        SpawnZombies(ZombiesToSpawn);
    }

    private void Update()
    {
        if (ZombieCount == -1)
            return;
        else if (ZombieCount > 0)
            lastZombiePos = zombies.Where(zombie => zombie != null).Last().gameObject.transform.position;
        else if (ZombieCount == 0 && !isDropped)
        {
            isDropped = true;
            if (DropItem != null)
                SpawnDropItem();
            FinishRoomAction();
        }
    }

    private void SpawnDropItem()
    {
        var item = Instantiate(DropItem, lastZombiePos, Quaternion.identity, Objects.transform);
        if (item.GetComponent<Hint>() != null)
            item.GetComponent<Hint>().ThisHintSprite = ThisHintSprite;
    }

    private void FinishRoomAction()
    {
        Doors.SetActive(false);
        Destroy(GetComponent<DoorManagement>());
    }

    private void SpawnZombies(int count)
    {
        for (var i = 0;  i < count; i++)
        {
            var minX = RoomBorders[0];
            var maxX = RoomBorders[1];
            var minY = RoomBorders[2];
            var maxY = RoomBorders[3];

            var position = new Vector3 (Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            var newZombie = Instantiate(Zombie, position, Quaternion.identity, Enemies.transform);
            zombies.Add(newZombie.GetComponent<HealthManagement>());
        }
    }
}
