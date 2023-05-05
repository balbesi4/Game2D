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
    public float[] RoomBorders;
    public HintSprite ThisHintSprite;
    public int ZombiesToSpawn;
    public AudioClip DoorSound;

    private int zombieCount { get { return zombies.Where(zombie => zombie != null).Count(); } }
    private List<HealthManagement> zombies;
    private Vector3 lastZombiePos;

    public void StartRoomAction()
    {
        AudioSource.PlayClipAtPoint(DoorSound, Camera.main.gameObject.transform.position);
        Doors.SetActive(true);
        zombies = new List<HealthManagement>();

        SpawnZombies(ZombiesToSpawn);
        StartCoroutine(CheckZombies());
    }

    private IEnumerator CheckZombies()
    {
        while (zombieCount > 0)
        {
            lastZombiePos = zombies.Where(zombie => zombie != null).Last().gameObject.transform.position;
            yield return null;
        }

        if (DropItem != null)
            SpawnDropItem();

        FinishRoomAction();
        yield break;
    }

    private void SpawnDropItem()
    {
        var item = Instantiate(DropItem, lastZombiePos, Quaternion.identity);
        if (item.GetComponent<Hint>() != null)
            item.GetComponent<Hint>().ThisHintSprite = ThisHintSprite;
    }

    private void FinishRoomAction()
    {
        AudioSource.PlayClipAtPoint(DoorSound, Camera.main.gameObject.transform.position);
        Doors.SetActive(false);
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
