using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DoorManagement : MonoBehaviour
{
    public GameObject Doors;
    public GameObject Lockers;
    public GameObject Zombie;
    public GameObject DropItem;

    private int zombieCount { get { return zombies.Where(zombie => zombie != null).Count(); } }
    private int zombiesToSpawn;
    private List<HealthManagement> zombies;
    private Vector3 lastZombiePos;

    public void StartRoomAction()
    {
        Doors.SetActive(true);
        Lockers.SetActive(true);
        zombies = new List<HealthManagement>();

        zombiesToSpawn = Random.Range(3, 6);
        SpawnZombies(zombiesToSpawn);
        StartCoroutine(CheckZombies());
    }

    private IEnumerator CheckZombies()
    {
        while (zombieCount > 0)
        {
            lastZombiePos = zombies.Where(zombie => zombie != null).Last().gameObject.transform.position;
            yield return null;
        }

        Instantiate(DropItem, lastZombiePos, Quaternion.identity);
        FinishRoomAction();
        yield break;
    }

    private void FinishRoomAction()
    {
        Doors.SetActive(false);
        Lockers.SetActive(false);
    }

    private void SpawnZombies(int count)
    {
        for (var i = 0;  i < count; i++)
        {
            var position = new Vector3 (Random.Range(-11, -2), Random.Range(0, 4.5f), 0);
            var newZombie = Instantiate(Zombie, position, Quaternion.identity);
            zombies.Add(newZombie.GetComponent<HealthManagement>());
        }
    }
}
