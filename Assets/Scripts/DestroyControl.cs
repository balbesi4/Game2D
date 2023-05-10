using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyControl : MonoBehaviour
{
    public GameObject WoodenWreck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var objects = GetComponentsInParent<Transform>()[1];
        var objectPos = gameObject.transform.position;
        Destroy(collision.gameObject);
        Destroy(gameObject);

        Instantiate(WoodenWreck, objectPos, Quaternion.identity, objects.transform);
    }
}
