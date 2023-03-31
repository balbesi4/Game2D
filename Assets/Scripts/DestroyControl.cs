using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyControl : MonoBehaviour
{
    public GameObject WoodenWreck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var objectPos = gameObject.transform.position;
        Destroy(collision.gameObject);
        Destroy(gameObject);

        Instantiate(WoodenWreck, objectPos, Quaternion.identity);
    }
}
